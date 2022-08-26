using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler onGrenadeExplode;

    [SerializeField] private ParticleSystem grenadeVfxPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve animationCurve;

    private Vector3 targetPosition;
    private Action onGrenadeBehaviourComplete;
    private float totalDistance;
    private Vector3 positionXY;

    private void Update()
    {
        Vector3 moveDirection = (targetPosition - positionXY).normalized;
        float moverSpeed = 15f;
        positionXY += moveDirection * moverSpeed * Time.deltaTime;

        float distance = Vector3.Distance(positionXY, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;

        float maxHeight = totalDistance / 4f;
        float positionY = animationCurve.Evaluate(distanceNormalized) * maxHeight;

        transform.position = new Vector3(positionXY.x, positionY, positionXY.z);

        float stoppingDistance = 0.1f;
        if(Vector3.Distance(positionXY, targetPosition) < stoppingDistance)
        {
            float explosionRadius = 4f;
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach(Collider collider in colliders)
            {
                if(collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(30);
                }

                if(collider.TryGetComponent<DestructableCrate>(out DestructableCrate destructableCrate))
                {
                    destructableCrate.Damage();
                }
            }

            onGrenadeExplode?.Invoke(this, EventArgs.Empty);
            trailRenderer.transform.parent = null;
            Instantiate(grenadeVfxPrefab, targetPosition + Vector3.up * 1.5f, Quaternion.identity);
            Destroy(gameObject);
            onGrenadeBehaviourComplete?.Invoke();
        }
    }

    public void SetUp(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXY = transform.position;
        positionXY.y = 0;
        totalDistance = Vector3.Distance(positionXY, targetPosition);
    }
}
