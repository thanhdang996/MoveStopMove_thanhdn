using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    public Vector3 Offset => offset;

    //private void Awake()
    //{
    //    offset = transform.position - target.position;
    //}

    private void LateUpdate()
    {
        transform.position = target.position + Offset;
    }

    public void ChangeOffSetBaseScale()
    {
        Vector3 oriOffset = offset;
        offset = oriOffset + new Vector3(0, 1f, -1f);
    }

}
