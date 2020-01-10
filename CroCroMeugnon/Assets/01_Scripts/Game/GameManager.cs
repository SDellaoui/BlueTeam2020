using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameOver = false;
    bool gameStarted = false;
    int playersCount = 3;
    
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
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Main" && !gameStarted)
        {
            Fabric.EventManager.Instance.PostEvent("Game_Start");
            
            gameStarted = true;
            gameOver = false;
        }
        if (playersCount == 0 && !gameOver)
        {
            Fabric.EventManager.Instance.PostEvent("Game_Over");
            SceneManager.LoadScene("TitleScreen",LoadSceneMode.Single);
            playersCount = 3;
            gameOver = true;
            gameStarted = false;
        }
    }
    public void RemovePlayer() { playersCount -= 1; }
    public void KillEntity(GameObject entity)
    {
        if (entity.layer == 8)
            playersCount -= 1;
        Instantiate(GameObject.Find("MinionSpawner").GetComponent<MinionSpawnerScript>().minionDead, entity.transform.position, Quaternion.identity);
        Destroy(entity);
    }
}
