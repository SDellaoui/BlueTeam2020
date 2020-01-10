using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public enum PlayerState
    {
        Small,
        Big,
        Dead
    }
    public PlayerState playerState;
    public int playerNum;
    public int strength = 1;
    public int mass = 1;
    public float bigStateTime;
    public SpriteRenderer[] spriteRenderers;

    Sprite[] playerSmallSprite;
    Sprite[] playerBigSprite;
    Sprite[] playerDeadSprite;
    Sprite[] playerDashSmall;
    Sprite[] playerDashBig;

    // Start is called before the first frame update
    void Start()
    {
        playerSmallSprite = Resources.LoadAll<Sprite>("Sprites/Player" + playerNum.ToString());
        playerBigSprite = Resources.LoadAll<Sprite>("Sprites/Giant_Player" + playerNum.ToString());
        playerDashSmall = Resources.LoadAll<Sprite>("Sprites/Player" + playerNum.ToString()+"_dash");
        playerDashBig = Resources.LoadAll<Sprite>("Sprites/Player" + playerNum.ToString() + "Big_dash");
        playerDeadSprite = Resources.LoadAll<Sprite>("Sprites/Dead_minion");
        spriteRenderers[0].sprite = playerSmallSprite[0];
        spriteRenderers[1].sprite = playerBigSprite[0];
        spriteRenderers[2].sprite = playerDeadSprite[0];
        SetPlayerState(playerState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetPlayerState(PlayerState _playerState)
    {
        if(_playerState == PlayerState.Small)
        {
            strength = 1;
            mass = 1;
            spriteRenderers[0].gameObject.SetActive(true);
            spriteRenderers[1].gameObject.SetActive(false);
            spriteRenderers[2].gameObject.SetActive(false);
        }
        else if(_playerState == PlayerState.Big)
        {
            strength = 2;
            mass = 2;
            spriteRenderers[0].gameObject.SetActive(false);
            spriteRenderers[1].gameObject.SetActive(true);
            spriteRenderers[2].gameObject.SetActive(false);
            StartCoroutine("PlayerStateBigCooldown");
        }
        else
        {
            spriteRenderers[0].gameObject.SetActive(false);
            spriteRenderers[1].gameObject.SetActive(false);
            spriteRenderers[2].gameObject.SetActive(true);

            GetComponent<PlayerMovementController>().enabled = false;
            GetComponent<DashBehaviour>().enabled = false;
        }
        playerState = _playerState;
    }
    IEnumerator PlayerStateBigCooldown()
    {
        yield return new WaitForSeconds(bigStateTime);
        Debug.Log("Reset en small");
        SetPlayerState(PlayerState.Small);
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && playerState == PlayerState.Dead)
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 11)
        {
            if(playerState == PlayerState.Small)
                SetPlayerState(PlayerState.Big);
            Destroy(collision.gameObject);
        }
    }
    public void Die()
    {
        SetPlayerState(PlayerState.Dead);
        Instantiate(GameManager.Instance.minionDeadPrefab, transform.position,Quaternion.identity);
        GameManager.Instance.RemovePlayer();
        Destroy(gameObject);
        
    }
    public void StartDashSprite(bool flip = false)
    {
        return;
        if (flip)
        {
            spriteRenderers[0].sprite = playerDashSmall[0];
            spriteRenderers[1].sprite = playerDashBig[0];
        }
        else
        {
            spriteRenderers[0].sprite = playerDashSmall[1];
            spriteRenderers[1].sprite = playerDashBig[1];
        }
    }
    public void EndDashSprite()
    {
        spriteRenderers[0].sprite = playerSmallSprite[0];
        spriteRenderers[1].sprite = playerBigSprite[0];
    }
}
