﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    Rigidbody2D rb;
    public SpriteRenderer sprite;
    public int playerNum = 1;
    public float speed;
    public float dashSpeed;
    public float startDashTime;
    float dashTime;
    public float dashCooldownTime = 2f;
    float dashCooldown;

    bool isDashing = false;
    bool isDashed = false;
    bool preventDash = false;
    float inputDeadZone = 0f;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 dashDirection;
    private Vector2 spriteScale;

    // Start is called before the first frame update
    void Start()
    {
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
            /*
            float move_X = (Mathf.Abs(Input.GetAxis("Horizontal" + playerNum.ToString())) < inputDeadZone) ? 0f : Input.GetAxis("Horizontal" + playerNum.ToString());
            float move_Y = (Mathf.Abs(Input.GetAxis("Vertical" + playerNum.ToString())) < inputDeadZone) ? 0f : Input.GetAxis("Vertical" + playerNum.ToString());
            moveDirection = new Vector2(move_X, move_Y);
            */
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
                sprite.gameObject.transform.localScale = new Vector2(spriteScale.x,spriteScale.y/5);
                isDashing = true;

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
        sprite.gameObject.transform.localScale = new Vector2(spriteScale.x,spriteScale.y);
        Debug.Log("StopDash");
    }
    public Vector2 GetDashDirection()
    {
        return moveDirection;
    }
    public bool GetIsDashing() { return isDashing; }
    public bool GetIsDashed() { return isDashed; }
    public void SetIsDashed(bool state) { isDashed = state; }
}
