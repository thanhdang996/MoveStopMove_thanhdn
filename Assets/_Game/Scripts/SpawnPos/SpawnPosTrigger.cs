using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosTrigger : MonoBehaviour
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

    [SerializeField] private List<Character> listCharacterInSpawnPos = new List<Character>();

    public List<Character> ListCharacterInSpawnPos { get => listCharacterInSpawnPos; set => listCharacterInSpawnPos = value; }

    public bool IsEmty => ListCharacterInSpawnPos.Count == 0;

    private void OnTriggerEnter(Collider other)
    {
        Character character = Cache.GetCharacter(other);
        if (character != null)
        {
            ListCharacterInSpawnPos.Add(character);
            character.ListInSpawnPos.Add(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Character character = Cache.GetCharacter(other);
        if (character != null)
        {
            ListCharacterInSpawnPos.Remove(character);
            character.ListInSpawnPos.Remove(this);
        }
    }
}
