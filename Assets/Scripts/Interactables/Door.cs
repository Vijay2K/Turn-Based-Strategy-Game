using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractables
{
    [SerializeField] private bool isOpen;

    private Action onInteractionComplete;
    private GridPosition gridPosition;
    private Animator animator;
    private bool isActive;
    private float timer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractablesAtGridPosition(gridPosition, this);

        if(isOpen)
        {
            OpenDoor();
        } else
        {
            CloseDoor();
        }
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
        float interactionTimer = 0.5f;
        timer = interactionTimer;

        if(isOpen)
        {
            CloseDoor();
        } else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("IsOpen", isOpen);
        PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }

    private void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("IsOpen", isOpen);
        PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}
