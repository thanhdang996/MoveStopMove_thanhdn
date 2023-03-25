using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public enum GameState { MainMenu, GamePlay, Finish }

public class GameManager : Singleton<GameManager>
{
    private GameState state;

    public void ChangeState(GameState state) => this.state = state;

    public bool IsState (GameState state) => this.state == state;

}
