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
    public PrefabItemShop CurrentPrefabItemPlayer => currentPrefabItemPlayer;

    [SerializeField] private List<PrefabItemShop> listPrefabItemPlayerContain;
    [SerializeField] private List<UIItemShop> listUIItemShop;

    [SerializeField] private Button button;
    private bool isFirstTimeRender = true;


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
                currentPrefabItemPlayer.gameObject.SetActive(false);
            }
            currentPrefabItemPlayer = Instantiate(hairSO.propHair[idUIITemShop].hairAvatarPrefabs, LevelManager.Instance.CurrentPlayer.HairHolderTF);
            currentPrefabItemPlayer.SetId(idUIITemShop);
            listPrefabItemPlayerContain.Add(currentPrefabItemPlayer);
            currentPrefabItemPlayer.gameObject.SetActive(true);
        }
        else
        {
            currentPrefabItemPlayer.gameObject.SetActive(false);
            for (int i = 0; i < listPrefabItemPlayerContain.Count; i++)
            {
                if (listPrefabItemPlayerContain[i].Id == idUIITemShop)
                {
                    currentPrefabItemPlayer = listPrefabItemPlayerContain[i];
                    currentPrefabItemPlayer.gameObject.SetActive(true);
                    return;
                }
            }
        }
    }

    public void ActiveUIItemShop()
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
        listUIItemShop[0].Selected();
    }

    public void DeActiveUIItemShop()
    {
        for (int i = 0; i < listUIItemShop.Count; i++)
        {
            listUIItemShop[i].gameObject.SetActive(false);
        }
    }


    public void TurnOnOffOutLine(int id)
    {
        for (int i = 0; i < listUIItemShop.Count; i++)
        {
            if (listUIItemShop[i].Id == id)
            {
                listUIItemShop[i].Outline.enabled = true;
            }
            else
            {
                listUIItemShop[i].Outline.enabled = false;
            }
        }
    }

    public void HandleOnClick()
    {
        if (uicShopDress.CurrentTabIndex == tabIndex) return;
        uicShopDress.OpenTab(tabIndex);
    }
}