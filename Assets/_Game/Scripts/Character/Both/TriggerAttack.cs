using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttack : MonoBehaviour
{
    [SerializeField] private Character characterParent;


    private void OnTriggerEnter(Collider other)
    {
        Character character = Cache.GetCharacter(other);
        if (character != null)
        {
            characterParent.ListTarget.Add(character);
            character.ListBeAimed.Add(characterParent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Character character = Cache.GetCharacter(other);
        if (character != null)
        {
            characterParent.ListTarget.Remove(character);
            character.ListBeAimed.Remove(characterParent);
        }
    }
}
