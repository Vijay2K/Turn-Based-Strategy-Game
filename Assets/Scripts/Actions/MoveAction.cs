using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private int maxGridPosition = 1;
    [SerializeField] private Animator unitAnimator;

    private Vector3 targetPosition;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!isActive) return;

        float stoppingDistance = 0.1f;
        float distance = Vector3.Distance(transform.position, targetPosition);
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        if (distance > stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
            isActive = false;
        }

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidGridPositions();
        return validGridPositionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidGridPositions()
    {
        List<GridPosition> validGridPosition = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for(int x = -maxGridPosition; x <= maxGridPosition; x++)
        {
            for(int z = -maxGridPosition; z <= maxGridPosition; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition gridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.GetIsValidGridPosition(gridPosition)) continue;
                if (unitGridPosition == gridPosition) continue;
                if (LevelGrid.Instance.HasAnyUnitAtGridPosition(gridPosition)) continue;

                validGridPosition.Add(gridPosition);
            }
        }

        return validGridPosition;
    }

    public void Move(GridPosition gridPosition)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;
    }
}
