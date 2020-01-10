using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    Rigidbody2D rb;
    public SpriteRenderer sprite;
    
    public float speed;
    public float dashSpeed;
    public float startDashTime;
    float dashTime;
    public float dashCooldownTime = 2f;
    float dashCooldown;
    public float stuntTime = 1f;

    bool isDashing = false;
    bool isDashed = false;
    bool isStunted = false;
    bool preventDash = false;

    float inputDeadZone = 0f;

    int playerNum;
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 dashDirection;
    private Vector2 spriteScale;

    // Start is called before the first frame update
    void Start()
    {
        playerNum = GetComponent<PlayerBehaviour>().playerNum;
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        dashCooldown = dashCooldownTime;
        spriteScale = sprite.transform.localScale;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isDashed)
            return;
        if (isDashing)
        {
            if (dashTime > 0f)
            {
                rb.velocity = dashDirection * Time.fixedDeltaTime;
                dashTime -= Time.deltaTime;
            }
            else
            {
                CancelDash();
            }
            
        }
        else
        {
            moveDirection = new Vector2(Input.GetAxis("Horizontal" + playerNum.ToString()), Input.GetAxis("Vertical" + playerNum.ToString()));
            if(moveDirection.magnitude < inputDeadZone)
                moveDirection = Vector2.zero;
            else
                moveDirection = moveDirection.normalized * ((moveDirection.magnitude - inputDeadZone) / (1 - inputDeadZone));

            rb.velocity = moveDirection * speed * Time.fixedDeltaTime;

            if (preventDash)
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
                if(moveDirection.x >= 0)
                {
                    GetComponent<PlayerBehaviour>().StartDashSprite();
                }
                else
                {
                    GetComponent<PlayerBehaviour>().StartDashSprite(true);
                }
                
                isDashing = true;
                Fabric.EventManager.Instance.PostEvent("Player_Dash", gameObject);
                Debug.Log("Start Dash !");
            }
            
        }
    }
    public void CancelDash()
    {
        rb.velocity = new Vector2(0f, 0f);
        isDashing = false;
        preventDash = true;
        dashTime = startDashTime;
        //sprite.gameObject.transform.localScale = new Vector2(spriteScale.x,spriteScale.y);
        GetComponent<PlayerBehaviour>().EndDashSprite();
        Debug.Log("StopDash");
    }
    public Vector2 GetDashDirection()
    {
        return moveDirection;
    }
    public bool GetIsDashing() { return isDashing; }
    public bool GetIsDashed() { return isDashed; }
    public void SetIsDashed(bool state) { isDashed = state; }
    public void SetIsStunted(bool state) { isStunted = state;  }
}
