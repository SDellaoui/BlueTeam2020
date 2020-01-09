using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrient : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float move_X = Input.GetAxis("Horizontal1");
        float move_Y = Input.GetAxis("Vertical1");
        transform.LookAt(Vector3.up,new Vector3(move_X,move_Y,0));   
    }
}
