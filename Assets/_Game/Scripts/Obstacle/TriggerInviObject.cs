using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInviObject : MonoBehaviour
{
    [SerializeField] private GameObject solidGO;
    [SerializeField] private GameObject transGO;

    private void OnTriggerEnter(Collider other)
    {
        Player player = Cache.GetPlayer(other);
        if (player != null)
        {
            TransWall();
            player.CurrentTriggerInviObject = this;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Player player = Cache.GetPlayer(other);
        if (player != null)
        {
            ShowWall();
            player.CurrentTriggerInviObject = null;
        }
    }

    public void ShowWall()
    {
        solidGO.SetActive(true);
        transGO.SetActive(false);
    }

    private void TransWall()
    {
        solidGO.SetActive(false);
        transGO.SetActive(true);
    }
}
