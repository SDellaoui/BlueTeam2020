using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBehaviour : MonoBehaviour
{
    Rigidbody2D rb;

    PlayerMovementController playerMovementController;
    Vector2 dashedDirection;
    Vector2 dashedPosition;
    float startDashedTime = 0.25f;
    float dashedTime;

    // Start is called before the first frame update
    void Start()
    {
        playerMovementController = GetComponent<PlayerMovementController>();
        rb = GetComponent<Rigidbody2D>();
        dashedTime = startDashedTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerMovementController.GetIsDashed())
        {
            Debug.Log(gameObject.name+" dashed ! ");
            rb.velocity = dashedDirection * 850 * Time.fixedDeltaTime;
            dashedTime -= Time.fixedDeltaTime;
            if(dashedTime <= 0f)
            {
                playerMovementController.SetIsDashed(false);
                dashedTime = startDashedTime;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.layer == 8){
            PlayerMovementController collPlayerMovementManager = hit.gameObject.GetComponent<PlayerMovementController>();
        
            if (collPlayerMovementManager.GetIsDashing() && !playerMovementController.GetIsDashing())
            {
                dashedDirection = collPlayerMovementManager.GetDashDirection();
                collPlayerMovementManager.CancelDash();
                playerMovementController.SetIsDashed(true);
                
            }
        }
        else if(hit.gameObject.layer == 10){
            playerMovementController.CancelDash();
        }
    }
}
