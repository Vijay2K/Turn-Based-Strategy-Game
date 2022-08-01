using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRendererPrefab;
    private Vector3 targetPosition;

    private void Update()
    {
        Vector3 moverDirection = (targetPosition - transform.position).normalized;
        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

        float moverSpeed = 200f;
        transform.position += moverDirection * Time.deltaTime * moverSpeed;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        if(distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;
            trailRendererPrefab.transform.parent = null;
            Destroy(gameObject);
        }

    }

    public void SetUp(Vector3 targetPosition)
    {        
        this.targetPosition = targetPosition;
    }
    
}
