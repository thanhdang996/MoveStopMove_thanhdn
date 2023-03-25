using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Transform spawnPosForPlayerTF;
    [SerializeField] private Transform spawnPosForBotTF;


    [SerializeField] private List<SpawnPosTrigger> listSpawn = new List<SpawnPosTrigger>();
    public List<SpawnPosTrigger> ListSpawnPosTrigger => listSpawn;


    [SerializeField] private List<Bot> listBotCurrent = new List<Bot>();
    public List<Bot> ListBotCurrent { get => listBotCurrent; set => listBotCurrent = value; }


    [SerializeField] private int totalEnemy = 100;
    public int TotalEnemy { get => totalEnemy; set => totalEnemy = value; }

    public bool NoMoreEnemy => totalEnemy == 0;
    public int NumberBotSpawnInit => spawnPosForBotTF.childCount;


    private void Start()
    {
        AddSpawnPosToListSpawnPos();
        RandomInitBot();

        MyUIManager.Instance.OnRetryButton += OnPrintRetry;
    }

    private void OnPrintRetry()
    {
        print("Retry");
    }

    public void AddSpawnPosToListSpawnPos()
    {
        foreach (Transform t in spawnPosForBotTF)
        {
            ListSpawnPosTrigger.Add(t.GetComponent<SpawnPosTrigger>());
        }
    }


    public Player SpawnInitPlayer()
    {
        //GameObject playerGo = ObjectPooling.Instance.GetGameObject(MyPoolType.Player);
        //playerGo.transform.position = spawnPosForPlayerTF.position;

        //PlayerMovement playerMovement = playerGo.GetComponent<PlayerMovement>();
        //playerMovement.SetJoystick(UIManager.Instance.Joystick);
        //Player player = playerGo.GetComponent<Player>();

        Player player = SimplePool.Spawn<Player>(PoolType.Player);
        player.TF.position = spawnPosForPlayerTF.position;
        player.PlayerMovement.SetJoystick(MyUIManager.Instance.Joystick);


        player.CreateAllWeaponPlayerOwner();
        player.ActiveCurrentWeapon();
        player.OnInit();
        player.HandleAttackRangeBaseOnRangeWeapon();
        player.HandleCamPlayerBaseOnRangeWeapon();

        return player;
    }

    public Player SpawnPlayerNextLevel(Player player)
    {
        player.TF.position = spawnPosForPlayerTF.position;
        player.ResetLevelCharacter();
        player.OnInit();
        player.HandleAttackRangeBaseOnRangeWeapon();
        player.HandleCamPlayerBaseOnRangeWeapon();
        return player;
    }

    public void RevivePlayer()
    {
        //GameObject playerGo = ObjectPooling.Instance.GetGameObject(MyPoolType.Player);
        //Player player = playerGo.GetComponent<Player>();
        Player player = SimplePool.Spawn<Player>(PoolType.Player);
        player.OnInit();

        //TODO: cache transform
        for (int i = 0; i < 100; i++)
        {
            SpawnPosTrigger posTrigger = ListSpawnPosTrigger[Random.Range(0, ListSpawnPosTrigger.Count)];
            if (!posTrigger.IsEmty)
            {
                continue;
            }
            player.TF.position = posTrigger.TF.position; break;
        }
    }

    public void RandomInitBot()
    {
        for (int i = 0; i < ListSpawnPosTrigger.Count; i++)
        {
            //GameObject botGo = ObjectPooling.Instance.GetGameObject(MyPoolType.Bot);
            //Bot bot = botGo.GetComponent<Bot>();
            Bot bot = SimplePool.Spawn<Bot>(PoolType.Bot);

            if (bot.WeaponHolderTF.childCount == 0)
            {
                bot.CreateWeaponBotBaseOnPlayerOwner();
            }
            bot.ActiveRandomWeapon();
            bot.OnInit();
            bot.HandleAttackRangeBaseOnRangeWeapon();
            bot.TF.position = ListSpawnPosTrigger[i].TF.position;
            
        }
    }

    public void RandomOneBot()
    {
        //GameObject botGo = ObjectPooling.Instance.GetGameObject(MyPoolType.Bot);
        //Bot bot = botGo.GetComponent<Bot>();

        Bot bot = SimplePool.Spawn<Bot>(PoolType.Bot);
        for (int i = 0; i < 100; i++)
        {
            SpawnPosTrigger posTrigger = ListSpawnPosTrigger[Random.Range(0, ListSpawnPosTrigger.Count)];
            if (!posTrigger.IsEmty)
            {
                continue;
            }
            bot.TF.position = posTrigger.TF.position;
            break;
        } // xet vi tri xong xuoi moi OnInit()

        bot.ActiveRandomWeapon();
        bot.OnInit();
        bot.HandleAttackRangeBaseOnRangeWeapon();
    }

    public void MinusTotalEnemy()
    {
        totalEnemy--;
    }
}
