using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosTrigger : MonoBehaviour
{
    [SerializeField] private List<Character> listCharacterInSpawnPos = new List<Character>();

    public List<Character> ListCharacterInSpawnPos { get => listCharacterInSpawnPos; set => listCharacterInSpawnPos = value; }

    public bool IsEmty => ListCharacterInSpawnPos.Count == 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            ListCharacterInSpawnPos.Add(character);
            character.ListInSpawnPos.Add(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            ListCharacterInSpawnPos.Remove(character);
            character.ListInSpawnPos.Remove(this);
        }
    }
}
