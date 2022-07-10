using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpinAmount;

    private void Update()
    {
        if (!isActive) return;

        float spinAmount = Time.deltaTime * 360f;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);
        totalSpinAmount += spinAmount;

        if(totalSpinAmount >= 360)
        {
            isActive = false;
            onActionComplete.Invoke();
        }
    }

    public void Spin(Action onSpinCompleted)
    {
        onActionComplete = onSpinCompleted;
        isActive = true;
        totalSpinAmount = 0;
    }

    public override string GetActionName()
    {
        return "Spin";
    }
}
