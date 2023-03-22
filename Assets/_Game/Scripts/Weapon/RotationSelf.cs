using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSelf : MonoBehaviour
{
    // cache transform
    private Transform tf;

    public Transform TF
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }


    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private bool isRotateZ;

    private void Update()
    {
        if (isRotateZ)
        {
            TF.Rotate(0, 0, rotateSpeed);
        }
        else
        {
            TF.Rotate(0, rotateSpeed, 0);
        }
    }
}
