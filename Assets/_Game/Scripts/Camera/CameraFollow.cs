using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform cameraMain;
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

}
