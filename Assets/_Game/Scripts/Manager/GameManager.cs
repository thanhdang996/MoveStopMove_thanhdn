using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public enum GameState { MainMenu, GamePlay, Finish }

public class GameManager : Singleton<GameManager>
{
    GameState state;

    [SerializeField] private GameData data;
    public GameData Data => data;


    private Player currentPlayer;
    public Player CurrentPlayer => currentPlayer;

    public int MaxLevel { get; set; } = 2;


    private void Start()
    {
        MyUIManager.Instance.OnNextButton += OnLoadNextLevel;

        LoadData();
        SoundManager.Instance.VolumeSetting.LoadValueMusic();
        LevelManager.Instance.LoadMapAtCurrentLevel();
        MyUIManager.Instance.OnInitLoadUI();
        currentPlayer = LevelManager.Instance.CurrentLevel.SpawnInitPlayer();
    }

    public void ChangeState(GameState state) => this.state = state;

    public bool IsState (GameState state) => this.state == state;

    private void OnLoadNextLevel()
    {
        LoadData();
        LevelManager.Instance.LoadMapAtCurrentLevel();
        MyUIManager.Instance.OnInitLoadUI();
        currentPlayer = LevelManager.Instance.CurrentLevel.SpawnPlayerNextLevel(CurrentPlayer);
    }

    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.txt";
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        formatter.Serialize(stream, Data);
        stream.Close();
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/player.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            this.data = data;
        }
        else
        {
            Debug.LogError("Chua co file save, auto tao data moi va save");
            data = new GameData();
            SaveData();
        }
    }

    public void AddCoinData()
    {
        Data.Coin++;
    }

    public void AddLevelData()
    {
        Data.LevelId++;
    }

    public void SetBGVolumeData(float value)
    {
        Data.BGMusicVolume = value;
    }
    public void SetSFXVolumeData(float value)
    {
        Data.SFXVolume = value;
    }

    public void AddNewItemToData(int indexWeaponOnShop)
    {
        Data.WeaponOwner.Add(indexWeaponOnShop);
        SaveData();

        currentPlayer.AddNewWeapon(indexWeaponOnShop);
    }

#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            print("save");
            SaveData();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            print("load");
            LoadData();
        }
    }
#endif

}
