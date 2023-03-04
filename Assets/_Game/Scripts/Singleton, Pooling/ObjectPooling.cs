using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType { Player, Bot, Bullet }

public class ObjectPooling : Singleton<ObjectPooling>
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject botPrefab;
    [SerializeField] private GameObject bulletPrefab;


    [SerializeField] private GameObject parentPlayer;
    [SerializeField] private GameObject parentBot;
    [SerializeField] private GameObject parentBullet;

    private Dictionary<ObjectType, List<GameObject>> dicGameObject = new Dictionary<ObjectType, List<GameObject>>();

    private GameObject GetPrefab(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Player:
                return playerPrefab;
            case ObjectType.Bot:
                return botPrefab;
            case ObjectType.Bullet:
                return bulletPrefab;
            default:
                return null;
        }
    }

    private GameObject GetParent(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Player:
                return parentPlayer;
            case ObjectType.Bot:
                return parentBot;
            case ObjectType.Bullet:
                return parentBullet;
            default:
                return null;
        }
    }


    public GameObject GetGameObject(ObjectType typePrefab)
    {
        List<GameObject> _itemPools = new List<GameObject>();
        if (!dicGameObject.ContainsKey(typePrefab))
        {
            dicGameObject.Add(typePrefab, _itemPools);
        }
        else
        {
            _itemPools = dicGameObject[typePrefab];
        }

        if (_itemPools.Count == 0)
        {
            GameObject go = Instantiate(GetPrefab(typePrefab), GetParent(typePrefab).transform);
            return go;
        }
        else
        {
            GameObject go = _itemPools[0];
            _itemPools.RemoveAt(0);
            go.SetActive(true);
            return go;
        }
    }

    public void ReturnGameObject(GameObject go, ObjectType typePrefab)
    {
        dicGameObject[typePrefab].Add(go);
        go.SetActive(false);
    }
}
