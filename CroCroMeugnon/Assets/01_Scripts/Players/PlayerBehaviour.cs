using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public int strength = 1;
    public int mass = 1;
    public float bigStateTime;
    public SpriteRenderer[] spriteRenderers;
    enum PlayerState
    {
        Small,
        Big
    }
    // Start is called before the first frame update
    void Start()
    {
        SetPlayerState(PlayerState.Big);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetPlayerState(PlayerState playerState)
    {
        if(playerState == PlayerState.Small)
        {
            strength += 1;
            mass += 1;
            spriteRenderers[0].gameObject.SetActive(true);
            spriteRenderers[1].gameObject.SetActive(false);
        }
        else
        {
            strength -= 1;
            mass -= 1;
            spriteRenderers[0].gameObject.SetActive(false);
            spriteRenderers[1].gameObject.SetActive(true);
            StartCoroutine("PlayerStateBigCooldown");
        }
    }
    IEnumerator PlayerStateBigCooldown()
    {
        yield return new WaitForSeconds(bigStateTime);
        Debug.Log("Reset en small");
        SetPlayerState(PlayerState.Small);
        yield return null;
    }
}
