using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MyPoolType { None, Player, Bot, Axe, Boomerang, Cream, Indicator }

public class ObjectPooling : Singleton<ObjectPooling>
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject botPrefab;
    [SerializeField] private GameObject axePrefab;
    [SerializeField] private GameObject boomerangPrefab;
    [SerializeField] private GameObject creamPrefab;
    [SerializeField] private GameObject arrowPrefab;


    [SerializeField] private GameObject parentPlayer;
    [SerializeField] private GameObject parentBot;
    [SerializeField] private GameObject parentWeapon;
    [SerializeField] private GameObject parentIndicator;

    private Dictionary<MyPoolType, List<GameObject>> dicGameObject = new Dictionary<MyPoolType, List<GameObject>>();

    private GameObject GetPrefab(MyPoolType type)
    {
        switch (type)
        {
            case MyPoolType.Player:
                return playerPrefab;
            case MyPoolType.Bot:
                return botPrefab;
            case MyPoolType.Axe:
                return axePrefab;
            case MyPoolType.Boomerang:
                return boomerangPrefab;
            case MyPoolType.Cream:
                return creamPrefab;
            case MyPoolType.Indicator:
                return arrowPrefab;
            default:
                return null;
        }
    }

    private GameObject GetParent(MyPoolType type)
    {
        switch (type)
        {
            case MyPoolType.Player:
                return parentPlayer;
            case MyPoolType.Bot:
                return parentBot;
            case MyPoolType.Axe:
                return parentWeapon;
            case MyPoolType.Boomerang:
                return parentWeapon;
            case MyPoolType.Cream:
                return parentWeapon;
            case MyPoolType.Indicator:
                return parentIndicator;
            default:
                return null;
        }
    }


    public GameObject GetGameObject(MyPoolType typePrefab)
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

    public void ReturnGameObject(GameObject go, MyPoolType typePrefab)
    {
        dicGameObject[typePrefab].Add(go);
        go.SetActive(false);
    }
}
