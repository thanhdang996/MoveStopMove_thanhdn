using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private float timer;
    private float randomTimePatrol;
    private float randomTimeChangeAttackState;
    public void OnEnter(Bot bot)
    {
        timer = 0;
        randomTimePatrol = Random.Range(3f, 4f);
        randomTimeChangeAttackState = Random.Range(0.5f, 1f);

        bot.StartMoving();
        bot.GetRandomPosTargetInMap();
        bot.MoveToTarget();
    }

    public void OnExecute(Bot bot)
    {
        timer += Time.deltaTime;
        bot.CheckTargetNearest();
        if (bot.ListTarget.Count > 0)
        {
            if (!bot.IsAttack && timer > randomTimeChangeAttackState)
            {
                bot.ChangeState(new AttackState());
            }
        }

        if (timer > randomTimePatrol)
        {
            bot.ChangeState(new IdleState());
        }
        else if (bot.IsReachTarget())
        {
            bot.GetRandomPosTargetInMap();
        }
    }

    public void OnExit(Bot bot)
    {

    }
}
