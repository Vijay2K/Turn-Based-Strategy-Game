using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;

    private void Awake() 
    {
        if(TryGetComponent<MoveAction>(out MoveAction moverAction))
        {
            moverAction.onStartMoving += MoveAction_OnStartMoving;
            moverAction.onStopMoving += MoveAction_OnStopMoving;
        }

        if(TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.onShoot += ShootAction_OnShoot;
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

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs args)
    {
        unitAnimator.SetTrigger("Shoot");
        Transform bulletProjectileTransform = 
            Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitPosition = args.targetUnit.GetWorldPosition();
        targetUnitPosition.y = shootPointTransform.position.y;
        bulletProjectile.SetUp(targetUnitPosition);
    }
}
