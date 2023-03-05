using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    [SerializeField] Character character;

    public void BeginRotate()
    {
        character.RotateToCharacter();
    }

    public void BeginAttack()
    {
        character.Attack();
    }

    //public void EndAttack()
    //{
    //    character.ResetAttack();
    //}
}
