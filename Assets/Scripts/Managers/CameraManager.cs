using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameobject;

    private void Start() 
    {
        BaseAction.onActionStarted += BaseAction_OnActionStarted;
        BaseAction.onActionStopped += BaseAction_OnActionStopped;

        HideActionCamera();
    }

    private void BaseAction_OnActionStarted(object sender, EventArgs args)
    {        
        switch(sender)
        {
            case ShootAction shooterAction:
                Unit shooterUnit = shooterAction.GetUnit();
                Unit targetUnit = shooterAction.GetTargetUnit();

                Vector3 cameraHeight = Vector3.up * 1.7f;
                Vector3 shooterDirection = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shooterDirection * shoulderOffsetAmount;

                Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraHeight + shoulderOffset + (shooterDirection * -1);

                actionCameraGameobject.transform.position = actionCameraPosition;
                actionCameraGameobject.transform.LookAt(targetUnit.GetWorldPosition() + cameraHeight);

                ShowActionCamera();
                break;
        }
    }

    private void BaseAction_OnActionStopped(object sender, EventArgs args)
    {
        switch(sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }

    public void ShowActionCamera()
    {
        actionCameraGameobject.SetActive(true);
    }

    public void HideActionCamera()
    {
        actionCameraGameobject.SetActive(false);
    }
}
