using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawnerScript : MonoBehaviour
{
    public GameObject minion;
    public GameObject minionDead;

    Vector2 spawnLocation;
    public float spawnRate = 5.0f;
    public float maxMinions = 20.0f;
    float nextSpawn = 0.0f;

    private GameObject[] getCount;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        nextSpawn -= Time.deltaTime;
        getCount = GameObject.FindGameObjectsWithTag("Minion");
        if (nextSpawn <= 0 && getCount.Length <= maxMinions)
        {
            nextSpawn += spawnRate;
            spawnLocation = new Vector2(Random.Range(-14.0f, 14.0f), Random.Range(-6.0f, 6.0f));
            Instantiate(minion, spawnLocation, Quaternion.identity);
        }
    }
}
