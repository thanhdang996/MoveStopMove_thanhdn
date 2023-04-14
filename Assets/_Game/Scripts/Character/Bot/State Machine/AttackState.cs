using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private float timer;
    private float randomTimerToPatrol; // nen bang time reset attack, de thap se chuyen sang patroll de ne hon
    public void OnEnter(Bot bot)
    {
        timer = 0;
        randomTimerToPatrol = Random.Range(0.5f, 0.8f); // nen de min la 0.5f, neu nho hon, chua kip nem da chuyen sang patrolState
        bot.StopMoving();
        bot.AttackCharacter();
    }

    public void OnExecute(Bot bot)
    {
        timer += Time.deltaTime;
        if(timer > randomTimerToPatrol)
        {
            bot.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Bot bot)
    {
        
    }
}
