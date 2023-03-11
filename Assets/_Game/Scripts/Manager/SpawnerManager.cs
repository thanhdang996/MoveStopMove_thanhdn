using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnerManager : Singleton<SpawnerManager>
{
    private LevelController currentLevel;

    [SerializeField] private FixedJoystick joystick;

    private void Start()
    {
        currentLevel = LevelManager.Instance.CurrentLevel;

        currentLevel.AddSpawnPosToList();

        SpawnPlayerInit();
        RandomInitBot();
    }

    public void SpawnPlayerInit()
    {
        GameObject playerGo = ObjectPooling.Instance.GetGameObject(PoolType.Player);
        PlayerMovement playerMovement = playerGo.GetComponent<PlayerMovement>();
        playerMovement.SetJoystick(joystick);

        Player player = playerGo.GetComponent<Player>();
        string stringCurrentWeapon = SaveManager.Instance.LoadPlayer().CurrentWeapon;
        if (System.Enum.TryParse(stringCurrentWeapon, out WeaponType type))
        {
            player.CurrentWeaponType = type;
        }
        else
        {
            Debug.Log("Error weapon");
            return;
        }
        player.OnInit();
        player.ActiveCurretnWeapon();
    }

    public void SpawnPlayer()
    {
        GameObject playerGo = ObjectPooling.Instance.GetGameObject(PoolType.Player);
        Player player = playerGo.GetComponent<Player>();
        player.OnInit();
        player.ActiveCurretnWeapon();

        for (int i = 0; i < 100; i++)
        {
            Transform t = currentLevel.ListSpawnPos[Random.Range(0, currentLevel.ListSpawnPos.Count)];
            if (!t.GetComponent<SpawnPosTrigger>().IsEmty)
            {
                continue;
            }
            player.transform.position = t.position; break;
        }
    }

    private void RandomInitBot()
    {
        for (int i = 0; i < currentLevel.ListSpawnPos.Count; i++)
        {
            GameObject botGo = ObjectPooling.Instance.GetGameObject(PoolType.Bot);
            Bot bot = botGo.GetComponent<Bot>();
            bot.OnInit();
            bot.ActiveCurretnWeapon(randomWeapon: true);
            bot.transform.position = currentLevel.ListSpawnPos[i].position;
            bot.Id = ++GameManager.IdGlobal;

        }
    }

    public void RandomOneBot()
    {
        GameObject botGo = ObjectPooling.Instance.GetGameObject(PoolType.Bot);
        Bot bot = botGo.GetComponent<Bot>();
        bot.OnInit();
        bot.ActiveCurretnWeapon(randomWeapon: true);
        for (int i = 0; i < 100; i++)
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
