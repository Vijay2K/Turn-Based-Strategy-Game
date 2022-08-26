using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;

    public void SetUp(Transform originalRootBone)
    {
        MatchAllChildTransform(originalRootBone, ragdollRootBone);
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        ApplyExplosionToRagdoll(ragdollRootBone, 300f, transform.position + randomDirection, 10f);
    }

    private void MatchAllChildTransform(Transform root, Transform clone)
    {
        foreach(Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if(cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MatchAllChildTransform(child, cloneChild);
            }
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach(Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody))
            {
                childRigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }

}
