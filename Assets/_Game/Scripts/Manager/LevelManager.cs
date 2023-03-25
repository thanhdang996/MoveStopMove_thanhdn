using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LevelController currentLevel;
    public LevelController CurrentLevel => currentLevel;

    private Player currentPlayer;
    public Player CurrentPlayer => currentPlayer;

    [SerializeField] private List<Bot> listBotCurrent = new List<Bot>();
    public List<Bot> ListBotCurrent => listBotCurrent;

    private void Start()
    {
        MyUIManager.Instance.OnNextButton += OnLoadNextLevel;

        DataManager.Instance.LoadData();
        SoundManager.Instance.VolumeSetting.LoadValueMusic();
        LoadMapAtCurrentLevel();
        MyUIManager.Instance.OnInitLoadUI();
        currentPlayer = CurrentLevel.SpawnInitPlayer();
    }

    private void OnLoadNextLevel()
    {
        DataManager.Instance.LoadData();
        LoadMapAtCurrentLevel();
        MyUIManager.Instance.OnInitLoadUI();
        currentPlayer = CurrentLevel.SpawnPlayerNextLevel(CurrentPlayer);
    }


    public void RemoveLastMap()
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
    }

    public void LoadMapAtCurrentLevel()
    {
        GameObject go = Resources.Load($"Levels/Level {DataManager.Instance.Data.LevelId}") as GameObject;
        currentLevel = Instantiate(go).GetComponent<LevelController>();
    }
}
