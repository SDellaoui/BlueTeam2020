using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion_Movement : MonoBehaviour
{
    NavMeshAgent navAgent;
    GameObject hero;

    public float speed = 1.0f;
    private float timeLeft;
    public float minTime = 1.0f;
    public float maxTime = 3.0f;
    public float minDistanceToHero = 5.0f;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 currentPosition;
    private Vector2 currentHeroPosition;
    private float distanceToHero;
    private Vector2 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject != null)
        {
            currentPosition = transform.position;
            hero = GameObject.FindGameObjectWithTag("Hero");
            currentHeroPosition = hero.transform.position;
            distanceToHero = Mathf.Sqrt(Mathf.Pow(currentPosition.x - currentHeroPosition.x, 2) + Mathf.Pow(currentPosition.y - currentHeroPosition.y, 2));
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0 && distanceToHero > minDistanceToHero)
            {
                float move_X = Random.Range(-3.0f, 3.0f);
                float move_Y = Random.Range(-3.0f, 3.0f);
                moveDirection = new Vector2(move_X, move_Y);
                moveDirection.Normalize();
                moveDirection *= speed;
                targetPosition = currentPosition + moveDirection;
                timeLeft = Random.Range(minTime, maxTime);
            }
            else if (distanceToHero <= minDistanceToHero)
            {
                moveDirection = currentPosition - currentHeroPosition;
                moveDirection.Normalize();
                moveDirection *= speed;
                targetPosition = currentPosition + moveDirection;
            }
            transform.position = Vector2.Lerp(currentPosition, targetPosition, Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Hero")
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
