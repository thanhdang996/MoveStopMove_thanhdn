using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Transform spawnPos;
    [SerializeField] private List<Transform> listSpawnTf = new List<Transform>();
    public List<Transform> ListSpawnPos => listSpawnTf;


    public void AddSpawnPosToList()
    {
        foreach (Transform t in spawnPos)
        {
            ListSpawnPos.Add(t);
        }
    }
}
