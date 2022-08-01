using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;

    private void Awake() 
    {
        if(TryGetComponent<MoveAction>(out MoveAction moverAction))
        {
            moverAction.onStartMoving += MoveAction_OnStartMoving;
            moverAction.onStopMoving += MoveAction_OnStopMoving;
        }
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs args)
    {
        unitAnimator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs args)
    {
        unitAnimator.SetBool("IsWalking", false);
    }
}
