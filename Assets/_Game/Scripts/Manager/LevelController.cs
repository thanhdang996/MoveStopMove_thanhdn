using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Transform spawnPosForPlayerTF;
    public Transform SpawnPosForPlayerTF { get => spawnPosForPlayerTF; }

    [SerializeField] private Transform spawnPosForBotTF;
    public Transform SpawnPosForBotTF { get => spawnPosForBotTF; }


    [SerializeField] private List<SpawnPosTrigger> listSpawnPosTrigger = new List<SpawnPosTrigger>();
    public List<SpawnPosTrigger> ListSpawnPosTrigger => listSpawnPosTrigger;


    private const int totalEnemy = 11;
    public int EnemyRemain { get; set; } = totalEnemy;
    public bool NoMoreEnemy => EnemyRemain == 0;
    public int NumberBotSpawnInit => spawnPosForBotTF.childCount;

    public void AddSpawnPosToListSpawnPos()
    {
        foreach (Transform t in spawnPosForBotTF)
        {
            ListSpawnPosTrigger.Add(t.GetComponent<SpawnPosTrigger>());
        }
    }

    public void MinusTotalEnemy()
    {
        EnemyRemain--;
    }

    public int GetTotalEnemy()
    {
        return totalEnemy;
    }
}
