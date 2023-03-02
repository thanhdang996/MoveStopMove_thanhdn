using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private float timer;
    private float randomTime;
    public void OnEnter(Bot bot)
    {
        timer = 0;
        randomTime = Random.Range(2f, 4f);
        bot.StopMoving();
    }

    public void OnExecute(Bot bot)
    {
        timer += Time.deltaTime;
        if(timer > randomTime)
        {
            bot.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Bot bot)
    {
        
    }
}
