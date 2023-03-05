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
        for (int i = 0; i < 3; i++)
        {
            GameObject botGo = ObjectPooling.Instance.GetGameObject(ObjectType.Bot);
            Bot bot = botGo.GetComponent<Bot>();
            bot.transform.position = currentLevel.ListSpawnPos[i];
            bot.Id = ++GameManager.IdGlobal;
        }
    }

    public void RandomOneBot()
    {
        GameObject botGo = ObjectPooling.Instance.GetGameObject(ObjectType.Bot);
        Bot bot = botGo.GetComponent<Bot>();
        bot.OnInit();
        bot.transform.position = currentLevel.ListSpawnPos[Random.Range(0, currentLevel.ListSpawnPos.Count)];
        bot.Id = ++GameManager.IdGlobal;
    }
}
