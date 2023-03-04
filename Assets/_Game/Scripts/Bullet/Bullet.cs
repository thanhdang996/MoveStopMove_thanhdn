using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20;
    [SerializeField] private float timeDestroy = 0.5f;
    [SerializeField] private float scalePerKill = 0.1f;
    private Character sourceFireCharacter;
    public Character SourceFireCharacter { get => sourceFireCharacter; set => sourceFireCharacter = value; }
    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void MoveAndAutoDestroy()
    {
        rb.AddForce(speed * transform.forward, ForceMode.Impulse);
        Invoke(nameof(OnDespawn), timeDestroy);
    }

    private void OnDespawn()
    {
        rb.velocity= Vector3.zero;
        SourceFireCharacter = null;
        if (!gameObject.activeSelf) return;
        ObjectPooling.Instance.ReturnGameObject(gameObject, ObjectType.Bullet);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            SourceFireCharacter.CharactersTargeted.Remove(character); // remove character from source
            Vector3 oriScale = SourceFireCharacter.transform.localScale;
            SourceFireCharacter.transform.localScale = oriScale + (Vector3.one * scalePerKill);
            character.OnDespawn();
            OnDespawn();
        }
    }
}
