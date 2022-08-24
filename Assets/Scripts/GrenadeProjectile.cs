using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    private Vector3 targetVector;
    private Action onGrenadeBehaviourComplete;

    private void Update() 
    {
        Vector3 moveDirection = (targetVector - transform.position).normalized;
        float moverSpeed = 5f;
        transform.position += moveDirection * moverSpeed * Time.deltaTime;

        float stoppingDistance = 0.1f;
        if(Vector3.Distance(transform.position, targetVector) < stoppingDistance)
        {
            float explosionRadius = 4f;
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach(Collider collider in colliders)
            {
                if(collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(30);
                }
            }
            Destroy(gameObject);

            onGrenadeBehaviourComplete?.Invoke();
        }
    }

    public void SetUp(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        this.targetVector = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }
}
