using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeAction : MonoBehaviour
{
    private void Start() 
    {
        ShootAction.onAnyShoot += ShootAction_OnAnyShoot;
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs args)
    {
        ScreenShake.Instance.Shake();
    }
}
