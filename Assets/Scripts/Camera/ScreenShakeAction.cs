using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeAction : MonoBehaviour
{
    private void Start() 
    {
        ShootAction.onAnyShoot += ShootAction_OnAnyShoot;
        GrenadeProjectile.onGrenadeExplode += GrenadeProjectile_OnGrenadeExplode;
    }

    private void GrenadeProjectile_OnGrenadeExplode(object sender, EventArgs args)
    {
        ScreenShake.Instance.Shake(2f);
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs args)
    {
        ScreenShake.Instance.Shake();
    }
}
