using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabItem : MonoBehaviour
{
    [SerializeField] private int tabIndex;

    [SerializeField] private UICShopDress uicShopDress;
    public UICShopDress UICShopDress => uicShopDress;

    [SerializeField] private Transform contentTF;
    [SerializeField] private UIItemShop prefabUIItemShop;
    [SerializeField] private SkinSO skinSO;

    [SerializeField] private PrefabItemShop currentPrefabItemPlayer;
    [SerializeField] private List<PrefabItemShop> listPrefabItemPlayerContain;



    private UIItemShop currentUIItemShop;
    public UIItemShop CurrentUIItemShop  => currentUIItemShop;
    [SerializeField] private List<UIItemShop> listUIItemShop;

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
        for (int i = 0; i < skinSO.propsItems.Length; i++)
        {
            PropsItem propItem = skinSO.propsItems[i];

            UIItemShop itemShop = Instantiate(prefabUIItemShop, contentTF);
            listUIItemShop.Add(itemShop);
            itemShop.SetId(i);
            itemShop.SetTabItem(this);
            itemShop.SetSprite(propItem.spriteImage);
            itemShop.SetPrice(propItem.price);

            if (i == 0)
            {
                SetCurrentUIItemShop(0);
                itemShop.Selected();
            }
        }
    }

    public void PreviewItemOnPlayer(int idUIITemShop)
    {
        for (int i = 0; i < listPrefabItemPlayerContain.Count; i++)
        {
            if (listPrefabItemPlayerContain[i].Id == idUIITemShop)
            {
                DeActivePrefabCurrentPlayer();
                currentPrefabItemPlayer = listPrefabItemPlayerContain[i];
                ActivePrefabCurrentPlayer();
                return;
            }
        }
        DeActivePrefabCurrentPlayer();
        currentPrefabItemPlayer = Instantiate(skinSO.propsItems[idUIITemShop].avatarPrefab, LevelManager.Instance.CurrentPlayer.HairHolderTF);
        currentPrefabItemPlayer.SetId(idUIITemShop);

        listPrefabItemPlayerContain.Add(currentPrefabItemPlayer);
        ActivePrefabCurrentPlayer();
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


    public void HandleOutLineButton(int id)
    {
        currentUIItemShop.Outline.enabled = false;
        SetCurrentUIItemShop(id);
        currentUIItemShop.Outline.enabled = true;
    }

    public void SetCurrentUIItemShop(int index)
    {
        // vi luc Instantiate UIItemShop set id = index cua vong lap
        currentUIItemShop = listUIItemShop[index];
    }

    public void HandleOnClick()
    {
        if (uicShopDress.CurrentTabItem.tabIndex == tabIndex) return;
        uicShopDress.OpenTab(tabIndex);
    }
}