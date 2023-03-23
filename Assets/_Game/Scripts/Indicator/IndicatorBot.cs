using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IndicatorBot : MonoBehaviour
{
    private Camera cam;

    private Transform playerTF;

    private List<Bot> listBotTargets;


    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        UIManager.Instance.OnNextButton += OnSearchAllTargetBotNextLevel;
        playerTF = GameManager.Instance.CurrentPlayer.transform;
        listBotTargets = LevelManager.Instance.CurrentLevel.ListBotCurrent;
    }

    private void OnSearchAllTargetBotNextLevel()
    {
        listBotTargets = LevelManager.Instance.CurrentLevel.ListBotCurrent;
    }

    private void Update()
    {
        for (int i = 0; i < listBotTargets.Count; i++)
        {
            Vector3 toPos = cam.WorldToScreenPoint(listBotTargets[i].TF.position);
            Vector3 fromPos = cam.WorldToScreenPoint(playerTF.position);
            Vector3 dir = (toPos - fromPos).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            listBotTargets[i].IndicatorGO.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            float borderSize = 50;
            Vector3 targetPosScreenPoint = toPos;
            bool isOffScreen = targetPosScreenPoint.x <= 0 || targetPosScreenPoint.x >= Screen.width || targetPosScreenPoint.y <= 0 || targetPosScreenPoint.y >= Screen.height;
            //print($"{targetPosScreenPoint} - {isOffScreen}");
            if (isOffScreen)
            {
                listBotTargets[i].IndicatorGO.SetActive(true);
                Vector3 cappedTargetScreenPos = targetPosScreenPoint;
                if (cappedTargetScreenPos.z < 0)
                {
                    cappedTargetScreenPos *= -1;
                    listBotTargets[i].IndicatorGO.transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
                }
                cappedTargetScreenPos.x = Mathf.Clamp(cappedTargetScreenPos.x, borderSize, Screen.width - borderSize);
                cappedTargetScreenPos.y = Mathf.Clamp(cappedTargetScreenPos.y, borderSize, Screen.height - borderSize);

                listBotTargets[i].IndicatorGO.transform.position = cappedTargetScreenPos;
            }
            else
            {
                listBotTargets[i].IndicatorGO.SetActive(false);
            }
        }
    }
}
