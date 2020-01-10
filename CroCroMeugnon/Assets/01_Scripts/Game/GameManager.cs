using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject minionDeadPrefab;
    private void Awake()     {
        if (Instance == null) 
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        } 
        else {
            Destroy(this); 
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Fabric.EventManager.Instance.PostEvent("Game_Start");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }
}
