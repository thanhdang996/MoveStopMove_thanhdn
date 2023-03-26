using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LevelController currentLevel;
    public LevelController CurrentLevel => currentLevel;

    private Player currentPlayer;
    public Player CurrentPlayer => currentPlayer;

    [SerializeField] private List<Bot> listBotCurrent = new List<Bot>();
    public List<Bot> ListBotCurrent => listBotCurrent;
    public void RemoveLastMap()
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
    }

    public void LoadMapAtCurrentLevel()
    {
        GameObject go = Resources.Load($"Levels/Level {DataManager.Instance.Data.LevelId}") as GameObject;
        currentLevel = Instantiate(go).GetComponent<LevelController>();
    }


    private void Start()
    {
        MyUIManager.Instance.OnNextButton += OnLoadNextLevel;
        MyUIManager.Instance.OnRetryButton += RevivePlayer;
        LoadNewMap();
    }

    private void LoadNewMap()
    {
        // load map and add spawnpos
        DataManager.Instance.LoadData();
        LoadMapAtCurrentLevel();
        currentLevel.AddSpawnPosToListSpawnPos();

        // set up sound, ui
        SoundManager.Instance.VolumeSetting.LoadValueMusic();
        MyUIManager.Instance.OnInitLoadUI();

        // spawn init player, bots and then show indicator
        SpawnInitPlayer();
        RandomInitBot();
        IndicatorHandle.Instance.AssignTempChacracterToShowIndicator();
    }

    private void OnLoadNextLevel()
    {
        DataManager.Instance.LoadData();
        LoadMapAtCurrentLevel();
        currentLevel.AddSpawnPosToListSpawnPos();

        MyUIManager.Instance.OnInitLoadUI();

        SpawnPlayerNextLevel();
        RandomInitBot();
    }

    private void SpawnInitPlayer()
    {
        currentPlayer = SimplePool.Spawn<Player>(PoolType.Player);
        currentPlayer.TF.position = currentLevel.SpawnPosForPlayerTF.position;
        currentPlayer.PlayerMovement.SetJoystick(MyUIManager.Instance.Joystick);

        currentPlayer.CreateAllWeaponPlayerOwner();
        currentPlayer.ActiveCurrentWeapon();
        currentPlayer.HandleAttackRangeBaseOnRangeWeapon();
        currentPlayer.HandleCamPlayerBaseOnRangeWeapon();
        currentPlayer.OnInit();

        currentPlayer.OnScaleChange += CheckConditionToWin;
        currentPlayer.OnDeath += CheckConditionToLose;
    }

    private void SpawnPlayerNextLevel()
    {
        currentPlayer.TF.position = currentLevel.SpawnPosForPlayerTF.position;
        currentPlayer.ResetLevelCharacter();
        currentPlayer.HandleAttackRangeBaseOnRangeWeapon();
        currentPlayer.HandleCamPlayerBaseOnRangeWeapon();
        currentPlayer.OnInit();
    }

    public void RevivePlayer()
    {
        //SimplePool.Despawn(currentPlayer);
        //currentPlayer = SimplePool.Spawn<Player>(PoolType.Player);
        currentPlayer.OnInit();
        RandomPosNotNearChacracter(self: currentPlayer);

        CheckConditionToWin(); // check lai cho chac
    }


    public void RandomInitBot()
    {
        for (int i = 0; i < CurrentLevel.ListSpawnPosTrigger.Count; i++)
        {
            Bot bot = SimplePool.Spawn<Bot>(PoolType.Bot);
            bot.TF.position = CurrentLevel.ListSpawnPosTrigger[i].TF.position;

            if (bot.WeaponHolderTF.childCount == 0)
            {
                bot.CreateWeaponBotBaseOnPlayerOwner();
            }

            bot.ActiveRandomWeapon();
            bot.HandleAttackRangeBaseOnRangeWeapon();
            bot.OnInit();
        }
    }

    public void RandomOneBot()
    {
        Bot bot = SimplePool.Spawn<Bot>(PoolType.Bot);
        RandomPosNotNearChacracter(bot);

        bot.ActiveRandomWeapon();
        bot.HandleAttackRangeBaseOnRangeWeapon();
        bot.OnInit();
    }

    private void RandomPosNotNearChacracter(Character self)
    {
        for (int i = 0; i < 100; i++)
        {
            SpawnPosTrigger posTrigger = CurrentLevel.ListSpawnPosTrigger[Random.Range(0, CurrentLevel.ListSpawnPosTrigger.Count)];
            if (!posTrigger.IsEmty)
            {
                continue;
            }
            self.TF.position = posTrigger.TF.position; break;
        }
    }

    private void CheckConditionToWin()
    {
        if (currentLevel.NoMoreEnemy && !currentPlayer.IsDead)
        {
            MyUIManager.Instance.ShowPanelWin();
            currentPlayer.DisablePlayerMovement();
            currentPlayer.IsWin = true;

            SoundManager.Instance.PlaySoundSFX2D(SoundType.Win);
            SoundManager.Instance.StopBGSoundMusic();
        }
    }
    private void CheckConditionToLose()
    {
        if (!currentPlayer.IsWin)
        {
            MyUIManager.Instance.ShowPanelLose();
            SoundManager.Instance.StopBGSoundMusic();
        }
    }

}
