using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private float timer;
    private float timerToPatrol = 1.5f;
    public void OnEnter(Bot bot)
    {
        bot.StopMoving();
        bot.AttackCharacter();
        timer = 0;
    }

    public void OnExecute(Bot bot)
    {
        timer += Time.deltaTime;
        if(timer > timerToPatrol)
        {
            bot.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Bot bot)
    {
        
    }
}
