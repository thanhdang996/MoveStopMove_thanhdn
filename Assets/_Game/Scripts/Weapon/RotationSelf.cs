using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSelf : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private bool isRotateZ;

    private void Update()
    {
        if (isRotateZ)
        {
            transform.Rotate(0, 0, rotateSpeed);
        }
        else
        {
            transform.Rotate(0, rotateSpeed, 0);
        }
    }
}
