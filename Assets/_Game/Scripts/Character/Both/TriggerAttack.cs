using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttack : MonoBehaviour
{
    private Character characterParent;

    private void Awake()
    {
        characterParent = GetComponentInParent<Character>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            characterParent.ListTarget.Add(character);
            character.ListBeAimed.Add(characterParent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            characterParent.ListTarget.Remove(character);
            character.ListBeAimed.Remove(characterParent);
        }
    }
}
