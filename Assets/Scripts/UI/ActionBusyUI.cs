using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    private void Start()
    {
        UnitActionSelection.Instance.onBusyChanged += UnitActionSelection_OnBusyChanged;
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UnitActionSelection_OnBusyChanged(object sender, bool isBusy)
    {
        if (isBusy)
            Show();
        else
            Hide();
    }
}
