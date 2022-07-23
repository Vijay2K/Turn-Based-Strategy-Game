using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private Button actionButton;
    [SerializeField] private GameObject selectedVisualUI;

    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;

        actionText.text = baseAction.GetActionName().ToUpper();
        actionButton.onClick.AddListener(() =>
        {
            UnitActionSelection.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedAction = UnitActionSelection.Instance.GetSelectedAction();
        selectedVisualUI.SetActive(baseAction == selectedAction);
    }
}
