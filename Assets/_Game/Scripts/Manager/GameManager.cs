using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static int IdGlobal = 0;

    public override void Awake()
    {
        base.Awake();
        IdGlobal = 0;
    }
}
