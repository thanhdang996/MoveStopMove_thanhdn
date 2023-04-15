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

    private int maxLevel = 2;

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
        // load map. create player, bot, indicator
        LoadNewMap();
        SpawnPlayerBotAndShowIndicator();

        // show mainmenu
        UIManager.Instance.OpenUI<UICMainMenu>();

        // load music first
        SoundManager.Instance.SetMusicVolume(DataManager.Instance.Data.BGMusicVolume);
        SoundManager.Instance.SetSFXVolume(DataManager.Instance.Data.SFXVolume);
    }

    private void LoadNewMap()
    {
        DataManager.Instance.LoadData();
        LoadMapAtCurrentLevel();
        currentLevel.AddSpawnPosToListSpawnPos();
        currentLevel.SetEnemyRemainEqualTotalEnemy();
    }

    private void SpawnPlayerBotAndShowIndicator()
    {
        SpawnInitPlayer();
        RandomInitBot();
        IndicatorHandle.Instance.AssignTempChacracterToShowIndicator();
    }

    private void SpawnInitPlayer()
    {
        currentPlayer = SimplePool.Spawn<Player>(PoolType.Player);
        currentPlayer.TF.position = currentLevel.SpawnPosForPlayerTF.position;

        currentPlayer.LoadHat();
        currentPlayer.LoadPant();
        currentPlayer.LoadShield();
        currentPlayer.LoadSet();

        currentPlayer.CreateAllWeaponPlayerOwner();
        currentPlayer.GetCurrentWeaponDataAndActive();
        currentPlayer.HandleAttackRangeBaseOnRangeWeapon();
        currentPlayer.HandleCamPlayerBaseOnRangeWeapon();
        currentPlayer.OnInit();
        currentPlayer.CanvasShowLevel.gameObject.SetActive(false);

        currentPlayer.OnDeath += CheckConditionToLose; // cac action  dki 1 lan duy nhat luc tao
    }

    private void SpawnPlayerNextLevel()
    {
        currentPlayer = SimplePool.Spawn<Player>(PoolType.Player);
        currentPlayer.TF.position = currentLevel.SpawnPosForPlayerTF.position;
        currentPlayer.ResetLevelCharacter();
        currentPlayer.HandleAttackRangeBaseOnRangeWeapon();
        currentPlayer.HandleCamPlayerBaseOnRangeWeapon();
        currentPlayer.OnInit();
    }

    // retry
    public void RevivePlayer()
    {
        SimplePool.Despawn(currentPlayer);
        currentPlayer = SimplePool.Spawn<Player>(PoolType.Player);
        currentPlayer.OnInit();
        currentPlayer.ActiveCurrentWeapon();
        RandomPosNotNearChacracter(self: currentPlayer);

        CheckConditionToWin(); // check lai cho chac neu nhu ko con enemy nao thi sau do hien win
    }


    public void RandomInitBot()
    {
        for (int i = 0; i < CurrentLevel.ListSpawnPosTrigger.Count; i++)
        {
            Bot bot = SimplePool.Spawn<Bot>(PoolType.Bot);
            bot.TF.position = CurrentLevel.ListSpawnPosTrigger[i].TF.position;

            bot.CreateWeaponBotBaseOnPlayerOwner();
            bot.ActiveRandomWeapon();

            bot.RandomColorBotAndSetCanvasLevelColor(i);
            bot.OnInit();
            bot.HandleAttackRangeBaseOnRangeWeapon();
            bot.CanvasShowLevel.gameObject.SetActive(false);
            bot.ChangeState(null);
            bot.OnDeath += CheckConditionEnemyRemainToSpawnAndCheckWin; // cac action  dki 1 lan duy nhat luc tao
        }
    }

    public void ReviveAllRandomBot()
    {
        for (int i = 0; i < CurrentLevel.ListSpawnPosTrigger.Count; i++)
        {
            Bot bot = SimplePool.Spawn<Bot>(PoolType.Bot);
            bot.TF.position = CurrentLevel.ListSpawnPosTrigger[i].TF.position;

            bot.ActiveRandomWeapon();
            bot.OnInit();
            bot.HandleAttackRangeBaseOnRangeWeapon();
            bot.ChangeState(null);
            bot.CanvasShowLevel.gameObject.SetActive(false);
        }
    }

    public void RandomNextLevelBot()
    {
        for (int i = 0; i < CurrentLevel.ListSpawnPosTrigger.Count; i++)
        {
            Bot bot = SimplePool.Spawn<Bot>(PoolType.Bot);
            bot.TF.position = CurrentLevel.ListSpawnPosTrigger[i].TF.position;

            bot.ActiveRandomWeapon();
            bot.OnInit();
            bot.HandleAttackRangeBaseOnRangeWeapon();
            bot.ChangeState(new PatrolState());
        }
    }

    public void RandomOneBot()
    {
        Bot bot = SimplePool.Spawn<Bot>(PoolType.Bot);
        RandomPosNotNearChacracter(bot);

        bot.ActiveRandomWeapon();
        bot.OnInit();
        bot.HandleAttackRangeBaseOnRangeWeapon();
        bot.ChangeState(new PatrolState());
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


    private void CheckConditionToLose()
    {
        if (!currentPlayer.IsWin)
        {
            UIManager.Instance.OpenUI<UICLoseLevel>();
            SoundManager.Instance.StopBGSoundMusic();
        }
    }

    private void CheckConditionEnemyRemainToSpawnAndCheckWin(Bot bot)
    {
        if (CurrentLevel.EnemyRemain >= CurrentLevel.NumberBotSpawnInit)
        {
            StartCoroutine(ReturnBotToPoolAndSpawnOneBot(bot));
        }
        else
        {
            StartCoroutine(ReturnBotToPool(bot));
        }

        CheckConditionToWin();

        IEnumerator ReturnBotToPoolAndSpawnOneBot(Bot bot)
        {
            yield return new WaitForSeconds(bot.TimeDelayRespawn);
            SimplePool.Despawn(bot);
            RandomOneBot();
        }

        IEnumerator ReturnBotToPool(Bot bot)
        {
            yield return new WaitForSeconds(bot.TimeDelayRespawn);
            SimplePool.Despawn(bot);
        }
    }

    private void CheckConditionToWin()
    {
        if (currentLevel.NoMoreEnemy && !currentPlayer.IsDead)
        {
            if (DataManager.Instance.Data.LevelId == maxLevel)
            {
                UIManager.Instance.OpenUI<UICFinalWin>();
            }
            else
            {
                UIManager.Instance.OpenUI<UICWinLevel>();
            }

            currentPlayer.DisablePlayerMovement();
            currentPlayer.IsWin = true;
        }
    }


    //start level
    public void OnStartGame()
    {
        GameManager.Instance.ChangeState(GameState.GamePlay);

        // handle active range sprite and animate cam when start game
        currentPlayer.AttackRangeSpriteRender.enabled = true;
        currentPlayer.CanvasShowLevel.gameObject.SetActive(true);
        currentPlayer.Cam.AnimateCamera();

        for (int i = 0; i < listBotCurrent.Count; i++)
        {
            listBotCurrent[i].ChangeState(new PatrolState());
            listBotCurrent[i].CanvasShowLevel.gameObject.SetActive(true);
        }
    }

    // back main menu
    public void OnBackToMainMenu()
    {
        SimplePool.CollectAll();
        StopAllCoroutines();

        GameManager.Instance.ChangeState(GameState.MainMenu);

        for (int i = listBotCurrent.Count - 1; i >= 0; i--)
        {
            listBotCurrent[i].SetPropWhenDeath();
            listBotCurrent.RemoveAt(i);
        }
        CurrentLevel.EnemyRemain = CurrentLevel.GetTotalEnemy();
        currentPlayer.ResetLevelCharacter(); // reset player moi revive bot, vi level bot base on player
        RevivePlayer();
        currentPlayer.HandleAttackRangeBaseOnRangeWeapon(); // reset size player
        currentPlayer.HandleCamPlayerBaseOnRangeWeapon(); //reset offsetCam
        currentPlayer.TF.position = currentLevel.SpawnPosForPlayerTF.position;

        ReviveAllRandomBot();

        // handle deactive range sprite and reset pos cam to init pos
        currentPlayer.AttackRangeSpriteRender.enabled = false;
        currentPlayer.CanvasShowLevel.gameObject.SetActive(false);
        currentPlayer.Cam.SetCamBachToInit();
    }

    // next level
    public void OnLoadNextLevel()
    {
        SimplePool.CollectAll();
        StopAllCoroutines();

        RemoveLastMap();
        DataManager.Instance.Data.AddLevelToData();
        DataManager.Instance.SaveData();

        DataManager.Instance.LoadData();
        LoadMapAtCurrentLevel();
        currentLevel.AddSpawnPosToListSpawnPos();
        currentLevel.SetEnemyRemainEqualTotalEnemy();
        UIManager.Instance.GetUI<UICGamePlay>().UI_UpdateEnemeRemainText();

        SpawnPlayerNextLevel();
        RandomNextLevelBot();
    }
}
