using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSwitch : MonoBehaviour, IInteractables
{
    [SerializeField] private Material redGlowMaterial;
    [SerializeField] private Material greenGlowMaterial;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private bool isTurnOn;

    private GridPosition gridPosition;
    private Action onInteractionComplete;
    private bool isActive;
    private float timer;

    private void Start() 
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractablesAtGridPosition(gridPosition, this);
        
        SwitchTurnOff();
    }

    private void Update() 
    {
        if(!isActive)
            return;

        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            isActive = false;
            onInteractionComplete?.Invoke();
        }
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        float afterInteractionTimer = 0.5f;
        timer = afterInteractionTimer;

        if(isTurnOn)
        {
            SwitchTurnOff();
        } else 
        {
            SwitchTurnOn();
        }
    }

    private void SwitchTurnOn()
    {
        isTurnOn = true;
        meshRenderer.material = greenGlowMaterial;
    }

    private void SwitchTurnOff()
    {
        isTurnOn = false;
        meshRenderer.material = redGlowMaterial;
    }

}
