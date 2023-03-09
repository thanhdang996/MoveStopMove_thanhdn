using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType {None, Player, Bot, Bullet, Boomerang }

public class ObjectPooling : Singleton<ObjectPooling>
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject botPrefab;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject boomerangPrefab;


    [SerializeField] private GameObject parentPlayer;
    [SerializeField] private GameObject parentBot;
    [SerializeField] private GameObject parentWeapon;

    private Dictionary<PoolType, List<GameObject>> dicGameObject = new Dictionary<PoolType, List<GameObject>>();

    private GameObject GetPrefab(PoolType type)
    {
        switch (type)
        {
            case PoolType.Player:
                return playerPrefab;
            case PoolType.Bot:
                return botPrefab;
            case PoolType.Bullet:
                return bulletPrefab;
            case PoolType.Boomerang:
                return boomerangPrefab;
            default:
                return null;
        }
    }

    private GameObject GetParent(PoolType type)
    {
        switch (type)
        {
            case PoolType.Player:
                return parentPlayer;
            case PoolType.Bot:
                return parentBot;
            case PoolType.Bullet:
                return parentWeapon;
            case PoolType.Boomerang:
                return parentWeapon;
            default:
                return null;
        }
    }


    public GameObject GetGameObject(PoolType typePrefab)
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

    public void ReturnGameObject(GameObject go, PoolType typePrefab)
    {
        dicGameObject[typePrefab].Add(go);
        go.SetActive(false);
    }
}
