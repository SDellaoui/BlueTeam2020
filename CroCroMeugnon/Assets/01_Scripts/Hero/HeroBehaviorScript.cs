using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroBehaviorScript : MonoBehaviour
{

    NavMeshAgent navAgent;
    SpriteRenderer sr;

    public float speed;
    public float baseSpeed = 2.0f;
    public float chaseSpeed = 3.0f;
    private float timeLeft;
    public float minTime = 1.0f;
    public float maxTime = 3.0f;
    public float minDistanceToTarget = 7.0f;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 currentPosition;

    GameObject target;
    GameObject currentTarget;
    private Vector2 targetPosition;
    private Vector2 currentTargetPosition;
    private Vector2 directionToTarget;
    private float distanceToTarget;
    private float currentTargetDistance;

    private GameObject[] players;
    private GameObject[] minions;
    private GameObject[] targets;

    private bool hasTarget;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
        sr = GetComponentInChildren<SpriteRenderer>();
        currentTargetDistance = 100.0f;
        hasTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.position;
        timeLeft -= Time.deltaTime;
        targets = new GameObject[0];
        players = GameObject.FindGameObjectsWithTag("Player");
        minions = GameObject.FindGameObjectsWithTag("Minion");
        targets = players.Concat(minions).ToArray();
        
        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                targetPosition = target.transform.position;
                directionToTarget = targetPosition - currentPosition;
                distanceToTarget = directionToTarget.magnitude;
                if (distanceToTarget < currentTargetDistance)
                {
                    currentTargetDistance = distanceToTarget;
                    currentTarget = target;
                }
            }
        }
        Debug.Log(currentTargetDistance);
        Debug.Log(currentTarget);
        if (currentTargetDistance <= minDistanceToTarget && currentTarget != null)
        {
            hasTarget = true;
            speed = chaseSpeed;
            sr.color = Color.red;
        }
        else
        {
            hasTarget = false;
            currentTargetDistance = 100.0f;
            speed = baseSpeed;
            sr.color = Color.white;
        }

        if (timeLeft <= 0)
        {
            float move_X = Random.Range(-3.0f, 3.0f);
            float move_Y = Random.Range(-3.0f, 3.0f);
            moveDirection = new Vector2(move_X, move_Y);
            moveDirection.Normalize();
            moveDirection *= speed;
            timeLeft = Random.Range(minTime, maxTime);
        }
        if (hasTarget && currentTarget != null)
        {
            navAgent.SetDestination(currentTarget.transform.position);
        }
        else
        {
            navAgent.ResetPath();
            transform.Translate(moveDirection * Time.deltaTime);
        }
    }
}
