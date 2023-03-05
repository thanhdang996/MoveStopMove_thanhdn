using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Transform spawnPos;
    [SerializeField] private List<Vector3> listSpawnPos = new List<Vector3>();
    public List<Vector3> ListSpawnPos => listSpawnPos;


    public void AddSpawnPosToList()
    {
        foreach (Transform t in spawnPos)
        {
            ListSpawnPos.Add(t.position);
        }
    }


   
}
