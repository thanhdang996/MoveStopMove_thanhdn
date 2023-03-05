using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private float speed = 8f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(joystick.Horizontal * speed, 0, joystick.Vertical * speed);
        if (rb.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    public bool IsMoving()
    {
        return Mathf.Abs(joystick.Horizontal) > 0.01f || Mathf.Abs(joystick.Vertical) > 0.01f;
    }

    public void StopMoving()
    {
        rb.velocity = Vector3.zero;
    }
}
