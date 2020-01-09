using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossBehaviour : MonoBehaviour
{
    NavMeshAgent navAgent;
    public GameObject target;
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
        navAgent.SetDestination(target.transform.position);
    }
}
