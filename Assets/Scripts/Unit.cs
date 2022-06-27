using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Animator unitAnimator;

    private Vector3 targetPosition;

    private void Update()
    {
        float stoppingDistance = 0.01f;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if(distance > stoppingDistance)
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

        if (Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetPosition());
        }
    }

    private void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
