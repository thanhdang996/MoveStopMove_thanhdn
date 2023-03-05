using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LevelController currentLevel;
    public LevelController CurrentLevel => currentLevel;
}
