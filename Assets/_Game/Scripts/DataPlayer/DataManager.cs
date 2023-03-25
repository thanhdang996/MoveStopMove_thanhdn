using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    [SerializeField] private GameData data;
    public GameData Data => data;

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
