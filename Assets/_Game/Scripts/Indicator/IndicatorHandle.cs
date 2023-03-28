using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IndicatorHandle : Singleton<IndicatorHandle>
{
    private Camera cam;

    private Transform playerTF;

    private List<Bot> listBotTargets;


    private void Awake()
    {
        cam = Camera.main;
    }

    public void AssignTempChacracterToShowIndicator()
    {
        playerTF = LevelManager.Instance.CurrentPlayer.transform;
        listBotTargets = LevelManager.Instance.ListBotCurrent;
    }


    private void Update()
    {
        if (GameManager.Instance.IsState(GameState.GamePlay))
        {
            for (int i = 0; i < listBotTargets.Count; i++)
            {
                Vector3 toPos = cam.WorldToScreenPoint(listBotTargets[i].TF.position);
                Vector3 fromPos = cam.WorldToScreenPoint(playerTF.position);
                Vector3 dir = (toPos - fromPos).normalized;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                listBotTargets[i].Indicator.RTF.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                float borderSize = 50;
                Vector3 targetPosScreenPoint = toPos;
                bool isOffScreen = targetPosScreenPoint.x <= 0 || targetPosScreenPoint.x >= Screen.width || targetPosScreenPoint.y <= 0 || targetPosScreenPoint.y >= Screen.height;
                if (isOffScreen)
                {
                    listBotTargets[i].Indicator.ShowIndicator();
                    Vector3 cappedTargetScreenPos = targetPosScreenPoint;
                    if (cappedTargetScreenPos.z < 0)
                    {
                        cappedTargetScreenPos *= -1;
                        listBotTargets[i].Indicator.RTF.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
                    }
                    cappedTargetScreenPos.x = Mathf.Clamp(cappedTargetScreenPos.x, borderSize, Screen.width - borderSize);
                    cappedTargetScreenPos.y = Mathf.Clamp(cappedTargetScreenPos.y, borderSize, Screen.height - borderSize);

                    listBotTargets[i].Indicator.RTF.anchoredPosition = cappedTargetScreenPos;
                }
                else
                {
                    listBotTargets[i].Indicator.HideIndicator();
                }
            }
        }
    }
}
