using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    [SerializeField] private GameObject aimed;

    [SerializeField] Character currentTarget;
    public Character CurrentTarget { get => currentTarget; set => currentTarget = value; }

    private NavMeshAgent navMeshAgent;
    private IState currentState;


    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = 3;
        ChangeState(new PatrolState());
    }

    public override void OnInit()
    {
        base.OnInit();
        currentTarget = null;
        aimed.SetActive(true);
        navMeshAgent.enabled = true;
    }

    public override void OnDespawn()
    {
        StopMoving();
        base.OnDespawn();
    }

    private void Start()
    {
        navMeshAgent.SetDestination(CurrentTarget.transform.position);
    }

    private void Update()
    {
        currentState?.OnExecute(this);
    }


    public void StopMoving()
    {
        navMeshAgent.velocity = Vector3.zero;
        navMeshAgent.isStopped = true;
    }
    public void StartMoving()
    {
        navMeshAgent.isStopped = false;
    }

    public void ShowAim()
    {
        aimed.SetActive(true);
    }

    public void HideAim()
    {
        aimed.SetActive(false);
    }

    public void MoveToTarget(Character target)
    {
        navMeshAgent.SetDestination(target.transform.position);
    }

    public void ChangeState(IState newState)
    {
        currentState?.OnExit(this);
        currentState = newState;
        currentState?.OnEnter(this);
    }

    public override void CheckTargetNearest()
    {
        float minDistance = float.MaxValue;
        Character currentTargetTmp = null;
        if (CharactersTargeted.Count == 0)
        {
            TargetNearest = null;
        }

        for (int i = 0; i < CharactersTargeted.Count; i++)
        {
            float distance = Vector3.Distance(CharactersTargeted[i].transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                currentTargetTmp = CharactersTargeted[i];
            }
        }
        TargetNearest = currentTargetTmp;
    }
}
