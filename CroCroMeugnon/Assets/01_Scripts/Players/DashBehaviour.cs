using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBehaviour : MonoBehaviour
{
    Rigidbody2D rb;

    PlayerMovementController playerMovementController;
    PlayerBehaviour playerBehaviour;
    Vector2 dashedDirection;
    Vector2 dashedPosition;
    float startDashedTime = 0.25f;
    float dashedTime;
    float dashedSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerMovementController = GetComponent<PlayerMovementController>();
        playerBehaviour = GetComponent<PlayerBehaviour>();
        rb = GetComponent<Rigidbody2D>();
        dashedTime = startDashedTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerMovementController.GetIsDashed())
        {
            Debug.Log(gameObject.name+" dashed ! ");
            //Fabric.EventManager.Instance.PostEvent("Dash", gameObject);
            rb.velocity = dashedDirection * playerMovementController.dashSpeed * Time.fixedDeltaTime;
            dashedTime -= Time.fixedDeltaTime;
            if(dashedTime <= 0f)
            {
                StartCoroutine("StuntPlayer");
                playerMovementController.SetIsDashed(false);
                dashedTime = startDashedTime;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.layer == 8)
        {
            //Collision avec un autre joueur
            PlayerMovementController collPlayerMovementManager = hit.gameObject.GetComponent<PlayerMovementController>();
            PlayerBehaviour collPlayerBehaviour = hit.gameObject.GetComponent<PlayerBehaviour>();
            dashedDirection = collPlayerMovementManager.GetDashDirection();
            //dashedSpeed = collPlayerMovementManager.dashSpeed * (collPlayerBehaviour.strength / playerBehaviour.strength);
            dashedSpeed = collPlayerMovementManager.dashSpeed * collPlayerBehaviour.strength;
            dashedTime = startDashedTime * collPlayerBehaviour.strength;
            if (collPlayerMovementManager.GetIsDashing() && !playerMovementController.GetIsDashing())
            {
                
                collPlayerMovementManager.CancelDash();
                playerMovementController.SetIsDashed(true);

            }
            else if (collPlayerMovementManager.GetIsDashing() && playerMovementController.GetIsDashing())
            {
                playerMovementController.CancelDash();
                if (playerBehaviour.strength < collPlayerBehaviour.strength)
                    playerMovementController.SetIsDashed(true);
            }
        }
        else if (hit.gameObject.layer == 10)
        {
            //Collision avec un mur
            playerMovementController.CancelDash();
        }
        /*
        else if (hit.gameObject.layer == 12)
        {
            Debug.Log("hit dino");
            playerBehaviour.Die();
        }
        */
    }

    IEnumerator StuntPlayer()
    {
        playerMovementController.SetIsStunted(true);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(playerMovementController.stuntTime);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerMovementController.SetIsStunted(false);
        yield return null;
    }
}
