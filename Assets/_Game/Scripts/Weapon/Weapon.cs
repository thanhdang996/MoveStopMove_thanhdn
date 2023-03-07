using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected float speed = 20;
    protected Character sourceFireCharacter;
    public Character SourceFireCharacter { get => sourceFireCharacter; set => sourceFireCharacter = value; }
    protected Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
