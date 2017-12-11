using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public List<Transform> patrolPoints = new List<Transform>();
    public NavMeshAgent agent;

    int activePoint = 0;

    private void Start()
    {
        agent.SetDestination(patrolPoints[activePoint].position);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, patrolPoints[activePoint].position) <= 2)
        {
            activePoint++;

            if (activePoint >= patrolPoints.Count)
                activePoint = 0;

            agent.SetDestination(patrolPoints[activePoint].position);
        }
    }
}
