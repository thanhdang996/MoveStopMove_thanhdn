using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IndicatorBot : MonoBehaviour
{
    private Camera cam;

    private Transform playerTF;

    private List<Transform> listBotTargetsTF;
    public List<Transform> ListBotTargetsTF { get => listBotTargetsTF; set => listBotTargetsTF = value; }


    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        UIManager.Instance.OnNextButton += OnSearchAllTargetBotNextLevel;
        playerTF = GameManager.Instance.CurrentPlayer.transform;
        listBotTargetsTF = LevelManager.Instance.CurrentLevel.ListBot;
    }

    private void OnSearchAllTargetBotNextLevel()
    {
        listBotTargetsTF = LevelManager.Instance.CurrentLevel.ListBot;
    }

    private void Update()
    {
        for (int i = 0; i < listBotTargetsTF.Count; i++)
        {
            Vector3 toPos = cam.WorldToScreenPoint(ListBotTargetsTF[i].position);
            Vector3 fromPos = cam.WorldToScreenPoint(playerTF.position);
            Vector3 dir = (toPos - fromPos).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            listBotTargetsTF[i].GetComponent<Bot>().IndicatorGO.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            float borderSize = 50;
            Vector3 targetPosScreenPoint = cam.WorldToScreenPoint(ListBotTargetsTF[i].position);
            bool isOffScreen = targetPosScreenPoint.x <= 0 || targetPosScreenPoint.x >= Screen.width || targetPosScreenPoint.y <= 0 || targetPosScreenPoint.y >= Screen.height;
            //print($"{targetPosScreenPoint} - {isOffScreen}");
            if (isOffScreen)
            {
                listBotTargetsTF[i].GetComponent<Bot>().IndicatorGO.SetActive(true);
                Vector3 cappedTargetScreenPos = targetPosScreenPoint;
                if (cappedTargetScreenPos.z < 0)
                {
                    cappedTargetScreenPos *= -1;
                    listBotTargetsTF[i].GetComponent<Bot>().IndicatorGO.transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
                }

                cappedTargetScreenPos.x = Mathf.Clamp(cappedTargetScreenPos.x, borderSize, Screen.width - borderSize);
                cappedTargetScreenPos.y = Mathf.Clamp(cappedTargetScreenPos.y, borderSize, Screen.height - borderSize);

                listBotTargetsTF[i].GetComponent<Bot>().IndicatorGO.transform.position = cappedTargetScreenPos;
            }
            else
            {
                listBotTargetsTF[i].GetComponent<Bot>().IndicatorGO.SetActive(false);
            }
        }
    }
}
