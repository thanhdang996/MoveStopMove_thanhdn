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


    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private bool isRotateZ;

    private void Update()
    {
        if (isRotateZ)
        {
            TF.Rotate(rotateSpeed * Time.deltaTime * Vector3.forward);
        }
        else
        {
            TF.Rotate(rotateSpeed * Time.deltaTime * Vector3.up);
        }
    }
}
