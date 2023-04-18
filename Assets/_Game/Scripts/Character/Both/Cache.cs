using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache 
{
    private static Dictionary<Collider, Character> characters = new Dictionary<Collider, Character>();
    public static Character GetCharacter(Collider collider)
    {
        if (!characters.ContainsKey(collider))
        {
            characters.Add(collider, collider.GetComponent<Character>());
        }

        return characters[collider];
    }


    private static Dictionary<Collider, Player> players = new Dictionary<Collider, Player>();
    public static Player GetPlayer(Collider collider)
    {
        if (!players.ContainsKey(collider))
        {
            players.Add(collider, collider.GetComponent<Player>());
        }

        return players[collider];
    }
}
