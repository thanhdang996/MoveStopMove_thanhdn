using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSelf : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 5f;

    private void Update()
    {
        transform.Rotate(0, 5, 0);
    }
}
