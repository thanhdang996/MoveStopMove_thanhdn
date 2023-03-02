using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Player : Character
{
    private PlayerMovement playerMovement;

    protected override void Awake()
    {
        base.Awake();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckTargetNearest();

        if (playerMovement.IsMoving()) return;
        if (CharactersTargeted.Count > 0)
        {
            if (!IsAttack)
            {
                AttackCharacter();
            }
        }
    }

    public override void CheckTargetNearest()
    {
        float minDistance = float.MaxValue;
        Character currentTargetTmp = null;
        if (CharactersTargeted.Count == 0)
        {

            if (TargetNearest != null)
            {
                (TargetNearest as Bot).HideAim();
                TargetNearest = null;
            }
        }

        for (int i = 0; i < CharactersTargeted.Count; i++)
        {
            float distance = Vector3.Distance(CharactersTargeted[i].transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                currentTargetTmp = CharactersTargeted[i];
            }
        }
        if (TargetNearest != currentTargetTmp)
        {
            if (TargetNearest != null)
            {
                (TargetNearest as Bot).HideAim();
            }
            TargetNearest = currentTargetTmp;
            (TargetNearest as Bot).ShowAim();
        }
    }
}
