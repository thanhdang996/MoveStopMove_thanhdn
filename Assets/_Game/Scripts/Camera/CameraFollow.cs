using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform cameraMain;
    private int standardRange = 10;
    private Vector3 standarVector3 = new Vector3(0, 25, -25);

    [SerializeField] private Vector3 offset;
    public Vector3 Offset => offset;

    private void Awake()
    {
        cameraMain = Camera.main.transform;
    }

    private void LateUpdate()
    {
        cameraMain.position = transform.position + Offset;
    }

    public void ChangeOffSetBaseScale()
    {
        Vector3 oriOffset = offset;
        offset = oriOffset + new Vector3(0, 1f, -1f);
    }
    public void ChangeOffSetBaseRangeWeapon(int rangeWeapon)
    {
        Vector3 oriOffset = standarVector3;
        if(rangeWeapon == standardRange)
        {
            offset = oriOffset;
            return;
        }
        if (rangeWeapon > standardRange) // thay thu rangeWeapon = 11, do moi lan chi scale 0.05 nen phai nhan 2
        {
            offset = oriOffset + new Vector3(0, ((rangeWeapon - standardRange) * 2) + 1, (-(rangeWeapon - standardRange) * 2) - 1);
        }
        else
        {
            offset = oriOffset + new Vector3(0, ((rangeWeapon - standardRange) * 2) - 1, (-(rangeWeapon - standardRange) * 2) + 1);
        }
    }
}

