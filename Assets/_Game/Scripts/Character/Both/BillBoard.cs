using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
        GetComponent<Canvas>().worldCamera = cam.GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
