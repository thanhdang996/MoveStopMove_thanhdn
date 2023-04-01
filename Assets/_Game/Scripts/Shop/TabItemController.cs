using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabItemController : MonoBehaviour
{
    [SerializeField] private int tabIndex;
    public int TabIndex => tabIndex;

    [SerializeField] private UICShopDress uicShopDress;
    public UICShopDress UICShopDress => uicShopDress;

    [SerializeField] private Transform contentTF;
    [SerializeField] private UIItemShop prefabUIItemShop;
    [SerializeField] private HairSO hairSO;

    [SerializeField] private PrefabItemShop currentPrefabItemPlayer;
    [SerializeField] private List<PrefabItemShop> listPrefabItemPlayerContain;

    private UIItemShop currentUIItemShop;
    public UIItemShop CurrentUIItemShop { get => currentUIItemShop; set => currentUIItemShop = value; }
    //public UIItemShop CurrentUIItemShop => currentUIItemShop;
    [SerializeField] private List<UIItemShop> listUIItemShop;
    public List<UIItemShop> ListUIItemShop => listUIItemShop;

    [SerializeField] private Button button;
    private bool isFirstTimeRender = true;

    [SerializeField] private Outline outline;
    public Outline Outline => outline;


    private void OnEnable()
    {
        button.onClick.AddListener(HandleOnClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(HandleOnClick);
    }

    public void InitItem()
    {
        for (int i = 0; i < hairSO.propHair.Length; i++)
        {
            PropHair propItem = hairSO.propHair[i];

            UIItemShop itemShop = Instantiate(prefabUIItemShop, contentTF);
            listUIItemShop.Add(itemShop);
            itemShop.SetId(i);
            itemShop.SetTabItemController(this);
            itemShop.SetSprite(propItem.spriteImage);
            itemShop.SetHairType(propItem.hairType);
            itemShop.SetPrice(propItem.price);

            if (i == 0)
            {
                SetCurrentUIItemShop(listUIItemShop[0]);
                itemShop.Selected();
            }
        }
    }

    public bool HasItemOnPlayer(int idUIITemShop)
    {
        for (int i = 0; i < listPrefabItemPlayerContain.Count; i++)
        {
            if (listPrefabItemPlayerContain[i].Id == idUIITemShop) return true;
        }
        return false;
    }

    public void CheckToAddItemPlayer(int idUIITemShop)
    {
        if (!HasItemOnPlayer(idUIITemShop))
        {
            if (currentPrefabItemPlayer != null)
            {
                DeActivePrefabCurrentPlayer();
            }
            currentPrefabItemPlayer = Instantiate(hairSO.propHair[idUIITemShop].hairAvatarPrefabs, LevelManager.Instance.CurrentPlayer.HairHolderTF);
            currentPrefabItemPlayer.SetId(idUIITemShop);
            listPrefabItemPlayerContain.Add(currentPrefabItemPlayer);
            ActivePrefabCurrentPlayer();
        }
        else
        {
            DeActivePrefabCurrentPlayer();
            for (int i = 0; i < listPrefabItemPlayerContain.Count; i++)
            {
                if (listPrefabItemPlayerContain[i].Id == idUIITemShop)
                {
                    currentPrefabItemPlayer = listPrefabItemPlayerContain[i];
                    ActivePrefabCurrentPlayer();
                    return;
                }
            }
        }
    }

    public void ActiveAllUIItemShop()
    {
        if (isFirstTimeRender)
        {
            InitItem();
            isFirstTimeRender = false;
            return;
        }

        for (int i = 0; i < listUIItemShop.Count; i++)
        {
            listUIItemShop[i].gameObject.SetActive(true);
        }
        SetCurrentUIItemShop(listUIItemShop[0]);
        currentUIItemShop.Selected(); // luon select phan tu dau
    }
    public void DeActiveAllUIItemShop()
    {
        for (int i = 0; i < listUIItemShop.Count; i++)
        {
            listUIItemShop[i].gameObject.SetActive(false);
        }
    }


    public void ActivePrefabCurrentPlayer()
    {
        currentPrefabItemPlayer.gameObject.SetActive(true);
    }
    public void DeActivePrefabCurrentPlayer()
    {
        if (currentPrefabItemPlayer != null)
        {
            currentPrefabItemPlayer.gameObject.SetActive(false);
        }
    }


    public void TurnOnOutLineCurrentButton(int id)
    {
        if (currentUIItemShop != null)
        {
            currentUIItemShop.Outline.enabled = false;

        }
        // vi luc Instantiate UIItemShop set id = index cua vong lap
        SetCurrentUIItemShop(listUIItemShop[id]);
        currentUIItemShop.Outline.enabled = true;
    }

    public void SetCurrentUIItemShop(UIItemShop itemShop)
    {
        currentUIItemShop = itemShop;
    }

    public void HandleOnClick()
    {
        if (uicShopDress.CurrentTabItemController.tabIndex == tabIndex) return;
        uicShopDress.OpenTab(tabIndex);
    }
}