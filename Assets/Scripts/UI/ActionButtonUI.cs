using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private Button actionButton;

    public void SetBaseAction(BaseAction baseAction)
    {
        actionText.text = baseAction.GetActionName().ToUpper();
        actionButton.onClick.AddListener(() =>
        {
            UnitActionSelection.Instance.SetSelectedAction(baseAction);
        });
    }
}
