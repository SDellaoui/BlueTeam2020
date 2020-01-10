using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroBehaviorScript : MonoBehaviour
{

    NavMeshAgent navAgent;
    SpriteRenderer sr;
    Rigidbody2D rb;

    public float speed;
    public float baseSpeed = 2.0f;
    public float chaseSpeed = 3.0f;
    public float timeLeftScream = 6.0f;
    private float timeLeft;
    public float minTime = 1.0f;
    public float maxTime = 3.0f;
    public float minDistanceToTarget = 7.0f;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 currentPosition;
    
    GameObject target;
    GameObject currentTarget;
    GameObject player;
    GameObject closestPlayer;

    private Vector2 targetPosition;
    private Vector2 currentTargetPosition;
    private Vector2 directionToTarget;
    private float distanceToTarget;
    private float currentTargetDistance;

    private Vector2 playerPosition;
    private Vector2 closestPlayerPosition;
    private Vector2 directionToPlayer;
    private float distanceToPlayer;
    private float closestPlayerDistance;
    
    private GameObject[] players;
    private GameObject[] minions;
    private GameObject[] targets;

    private bool _hasTarget = false;

    private bool hasTarget
    {
        get { return _hasTarget; }
        set
        {
            if (_hasTarget != value)
            {
                _hasTarget = value;
                if (_hasTarget == true)
                {
                    Fabric.EventManager.Instance.PostEvent("Dino_Grawl", gameObject);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentTargetDistance = 100.0f;
        closestPlayerDistance = 100.0f;
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
        foreach (GameObject player in players)
        {
            if (player != null)
            {
                playerPosition = player.transform.position;
                directionToPlayer = playerPosition - currentPosition;
                distanceToPlayer = directionToPlayer.magnitude;
                if (distanceToPlayer < closestPlayerDistance)
                {
                    closestPlayerDistance = distanceToPlayer;
                    closestPlayer = player;
                }
            }
        }
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
            timeLeft = timeLeftScream;
            Fabric.EventManager.Instance.PostEvent("Dino_Scream", gameObject);
        }

        if (hasTarget && currentTarget != null)
        {
            navAgent.SetDestination(currentTarget.transform.position);
            navAgent.speed = speed;
            sr.flipX = ((currentTarget.transform.position.x - currentPosition.x) > 0)?false: true ;
        }
        else
        {
            if (closestPlayer != null)
            {
                navAgent.speed = speed;
                //transform.Translate(moveDirection * Time.deltaTime);
                navAgent.SetDestination(closestPlayer.transform.position);
                sr.flipX = ((moveDirection.x - currentPosition.x) > 0) ? false : true;
            }
            else
            {
                navAgent.ResetPath();
                closestPlayerDistance = 100.0f;
            }
        }

        //Debug.Log(closestPlayer);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.tag == "Minion")
        {
            GameManager.Instance.KillEntity(other.gameObject);
            Fabric.EventManager.Instance.PostEvent("Dino_Gulp", gameObject);
            
        }
    }

    //void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.layer == 10)
    //    {
    //        Debug.Log("Wall hit");
    //        moveDirection = Vector2.zero - currentPosition;
    //        moveDirection.Normalize();
    //        moveDirection *= speed;
    //    }
    //}
}
