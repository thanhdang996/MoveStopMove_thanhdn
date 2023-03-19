using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private GameData data;
    public GameData Data { get => data; set => data = value; }


    private Player currentPlayer;
    public Player CurrentPlayer => currentPlayer;
    public static int IdGlobal = 0;

    public int MaxLevel { get; set; } = 2;


    public override void Awake()
    {
        base.Awake();
        IdGlobal = 0;
    }

    private void Start()
    {
        UIManager.Instance.OnNextButton += OnInitLoad;
        //OnInitLoad();
        LoadData();
        LevelManager.Instance.LoadMapAtCurrentLevel();
        UIManager.Instance.OnInitLoadUI();
        currentPlayer = LevelManager.Instance.CurrentLevel.SpawnInitPlayer();
    }

    private void OnInitLoad()
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

    public void AddNewItemToData(int indexWeapon)
    {
        Data.WeaponOwner.Add(indexWeapon);
        SaveData();

        List<int> listWeaponOwner = Data.WeaponOwner;
        int lastIndexItem = listWeaponOwner[listWeaponOwner.Count - 1];
        currentPlayer.AddNewWeapon(lastIndexItem);
    }

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
}
