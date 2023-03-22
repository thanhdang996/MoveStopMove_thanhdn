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
    public GameData Data { get => data; set => data = value; }


    private Player currentPlayer;
    public Player CurrentPlayer => currentPlayer;
    public static int IdGlobal = 0;

    public int MaxLevel { get; set; } = 2;


    protected override void Awake()
    {
        base.Awake();
        IdGlobal = 0;
    }

    private void Start()
    {
        UIManager.Instance.OnNextButton += OnLoadNextLevel;

        LoadData();
        LevelManager.Instance.LoadMapAtCurrentLevel();
        UIManager.Instance.OnInitLoadUI();
        currentPlayer = LevelManager.Instance.CurrentLevel.SpawnInitPlayer();
    }

    public void ChangeState(GameState state) => this.state = state;

    public bool IsState (GameState state) => this.state == state;

    private void OnLoadNextLevel()
    {
        LoadData();
        LevelManager.Instance.LoadMapAtCurrentLevel();
        UIManager.Instance.OnInitLoadUI();
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
            Data = data;
        }
        else
        {
            Debug.LogError("Chua co file save, auto tao data moi va save");
            Data = new GameData();
            SaveData();
        }
    }

    public void AddCoin()
    {
        Data.Coin++;
    }

    public void AddLevel()
    {
        Data.LevelId++;
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
