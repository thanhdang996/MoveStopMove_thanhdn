using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : Singleton<ObjectPooling>
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private List<GameObject> bulletObjects;

    public GameObject GetGameObject()
    {
        if(bulletObjects.Count == 0)
        {
            GameObject go = Instantiate(bulletPrefab, transform);
            return go;
        } else
        {
            GameObject go = bulletObjects[0];
            bulletObjects.RemoveAt(0);
            go.SetActive(true);
            return go;
        }
    }

    public void ReturnBullet(GameObject obj)
    {
        bulletObjects.Add(obj); 
        obj.SetActive(false);
    }
}
