using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public PlayerBehaviour[] players;

    // Start is called before the first frame update
    void Start()
    {
        Fabric.EventManager.Instance.PostEvent("Game_Start",gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
