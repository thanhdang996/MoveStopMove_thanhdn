using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPlayer : MonoBehaviour
{
    [SerializeField] private Transform target;
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent= GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = 5;
    }

    private void Update()
    {
        navMeshAgent.SetDestination(target.position);
    }
}
