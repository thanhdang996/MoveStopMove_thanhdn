using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Transform spawnPosForPlayer;

    [SerializeField] private Transform spawnPosForBot;
    [SerializeField] private List<Transform> listSpawnTf = new List<Transform>();
    public List<Transform> ListSpawnPos => listSpawnTf;


    [SerializeField] private int totalEnemy = 100;
    public int TotalEnemy { get => totalEnemy; set => totalEnemy = value; }

    public bool NoMoreEnemy => totalEnemy == 0;
    public int NumberBotSpawnInit => spawnPosForBot.childCount;

    private void Start()
    {
        AddSpawnPosToListSpawnPos();
        RandomInitBot();

        UIManager.Instance.OnRetryButton += PrintRetry;
    }

    private void PrintRetry()
    {
        print("Retry");
    }

    public void AddSpawnPosToListSpawnPos()
    {
        foreach (Transform t in spawnPosForBot)
        {
            ListSpawnPos.Add(t);
        }
    }


    public Player SpawnInitPlayer()
    {
        GameObject playerGo = ObjectPooling.Instance.GetGameObject(PoolType.Player);
        playerGo.transform.position = spawnPosForPlayer.position;

        PlayerMovement playerMovement = playerGo.GetComponent<PlayerMovement>();
        playerMovement.SetJoystick(UIManager.Instance.Joystick);

        Player player = playerGo.GetComponent<Player>();
        player.ResetLevelCharacter();
        player.CreateAllWeaponPlayerOwner();
        player.ActiveCurrentWeapon();
        player.OnInit();
        player.HandleAttackRangeBaseOnRangeWeapon();
        player.HandleCamPlayerBaseOnRangeWeapon();

        return player;
    }

    public Player SpawnPlayerNextLevel(Player player)
    {
        player.transform.position = spawnPosForPlayer.position;
        player.ResetLevelCharacter();
        player.OnInit();
        player.HandleAttackRangeBaseOnRangeWeapon();
        player.HandleCamPlayerBaseOnRangeWeapon();
        return player;
    }

    public void SpawnPlayer()
    {
        GameObject playerGo = ObjectPooling.Instance.GetGameObject(PoolType.Player);
        Player player = playerGo.GetComponent<Player>();
        player.OnInit();

        for (int i = 0; i < 100; i++)
        {
            Transform t = ListSpawnPos[Random.Range(0, ListSpawnPos.Count)];
            if (!t.GetComponent<SpawnPosTrigger>().IsEmty)
            {
                continue;
            }
            player.transform.position = t.position; break;
        }
    }

    public void RandomInitBot()
    {
        for (int i = 0; i < ListSpawnPos.Count; i++)
        {
            GameObject botGo = ObjectPooling.Instance.GetGameObject(PoolType.Bot);
            Bot bot = botGo.GetComponent<Bot>();

            if (bot.WeaponHolder.childCount == 0)
            {
                bot.CreateWeaponBotBaseOnPlayerOwner();
            }
            bot.ResetLevelCharacter();
            bot.ActiveRandomWeapon();
            bot.OnInit();
            bot.transform.position = ListSpawnPos[i].position;
            bot.Id = ++GameManager.IdGlobal;
        }
    }

    public void RandomOneBot()
    {
        GameObject botGo = ObjectPooling.Instance.GetGameObject(PoolType.Bot);
        Bot bot = botGo.GetComponent<Bot>();
        bot.ActiveRandomWeapon();
        bot.OnInit();

        for (int i = 0; i < 100; i++)
        {
            Transform t = ListSpawnPos[Random.Range(0, ListSpawnPos.Count)];
            if (!t.GetComponent<SpawnPosTrigger>().IsEmty)
            {
                continue;
            }
            bot.transform.position = t.position; break;
        }
        bot.Id = ++GameManager.IdGlobal;
    }

    public void MinusTotalEnemy()
    {
        totalEnemy--;
    }
    public int GetTotalEnemy()
    {
        return totalEnemy;
    }
}
