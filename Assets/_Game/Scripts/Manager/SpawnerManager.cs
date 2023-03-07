using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : Singleton<SpawnerManager>
{
    private LevelController currentLevel;

    private void Start()
    {
        currentLevel = LevelManager.Instance.CurrentLevel;

        currentLevel.AddSpawnPosToList();
        RandomInitBot();
    }

    public void RandomInitBot()
    {
        for (int i = 0; i < currentLevel.ListSpawnPos.Count; i++)
        {
            GameObject botGo = ObjectPooling.Instance.GetGameObject(ObjectType.Bot);
            Bot bot = botGo.GetComponent<Bot>();
            bot.transform.position = currentLevel.ListSpawnPos[i].position;
            bot.Id = ++GameManager.IdGlobal;
        }
    }

    public void RandomOneBot()
    {
        GameObject botGo = ObjectPooling.Instance.GetGameObject(ObjectType.Bot);
        Bot bot = botGo.GetComponent<Bot>();
        bot.OnInit();
        for (int i = 0; i < 50; i++)
        {
            Transform t = currentLevel.ListSpawnPos[Random.Range(0, currentLevel.ListSpawnPos.Count)];
            if (!t.GetComponent<SpawnPosTrigger>().IsEmty)
            {
                continue;
            }
            bot.transform.position = t.position; break;
        }
        bot.Id = ++GameManager.IdGlobal;
    }
}
