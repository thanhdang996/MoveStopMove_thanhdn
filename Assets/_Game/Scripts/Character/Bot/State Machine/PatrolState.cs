using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private float timer;
    private float randomTime;
    public void OnEnter(Bot bot)
    {
        timer = 0;
        randomTime = Random.Range(3f, 6f);
        bot.StartMoving();
    }

    public void OnExecute(Bot bot)
    {
        bot.CheckTargetNearest();

        if(bot.CharactersTargeted.Count > 0)
        {
            if(!bot.IsAttack)
            {
                bot.ChangeState(new AttackState());
            }
        }
        timer += Time.deltaTime;

        if (timer < randomTime)
        {
            bot.MoveToTarget(bot.CurrentTarget);
        }
        else
        {
            bot.ChangeState(new IdleState());
        }
    }

    public void OnExit(Bot bot)
    {
        
    }
}
