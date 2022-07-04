using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private int maxGridPosition = 1;
    [SerializeField] private Animator unitAnimator;

    private Vector3 targetPosition;
    private Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        targetPosition = transform.position;
    }

    private void Update()
    {
        float stoppingDistance = 0.1f;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
        }
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
                Debug.Log(gridPosition.ToString());
            }
        }

        return validGridPosition;
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
