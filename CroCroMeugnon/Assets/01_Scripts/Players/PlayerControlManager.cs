using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManager : MonoBehaviour
{
    CharacterController characterController;

    public int playerNum = 1;
    public float speed;
    public float dashSpeed;
    public float startDashTime;
    float dashTime;
    public float dashCooldownTime = 2f;
    float dashCooldown;
    
    bool isDashing = false;
    bool preventDash = false;
    float inputDeadZone = 0.35f;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 dashDirection;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        dashTime = startDashTime;
        dashCooldown = dashCooldownTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            if (dashTime > 0f)
            {
                characterController.Move(dashDirection * Time.deltaTime);
                dashTime -= Time.deltaTime;
            }
            else
            {
                characterController.Move(new Vector2(0f,0f));
                isDashing = false;
                preventDash = true;
            }
        }
        else
        {
            float move_X = (Mathf.Abs(Input.GetAxis("Horizontal" + playerNum.ToString())) < inputDeadZone) ? 0f : Input.GetAxis("Horizontal" + playerNum.ToString());
            float move_Y = (Mathf.Abs(Input.GetAxis("Vertical" + playerNum.ToString())) < inputDeadZone) ? 0f : Input.GetAxis("Vertical" + playerNum.ToString());
            moveDirection = new Vector2(move_X, move_Y);
            characterController.Move(moveDirection * speed * Time.deltaTime);

            if(preventDash)
            {
                if (dashCooldown <= 0f)
                {
                    dashTime = startDashTime;
                    dashCooldown = dashCooldownTime;
                    preventDash = false;
                    Debug.Log("dash cooldown over");
                }
                else
                    dashCooldown -= Time.deltaTime;
            }

            if (Input.GetAxis("Dash" + playerNum.ToString()) >= 0.2f && !preventDash)
            {
                dashDirection = moveDirection * dashSpeed;
                isDashing = true;
                
                Debug.Log("dash !");
            }
        }
    }
    public void CancelDash()
    {
        characterController.Move(new Vector2(0f, 0f));
        isDashing = false;
        preventDash = true;
        dashTime = startDashTime;
    }
    public Vector2 GetDashDirection()
    {
        return moveDirection;
    }
    public bool IsDashing() { return isDashing; }
}
