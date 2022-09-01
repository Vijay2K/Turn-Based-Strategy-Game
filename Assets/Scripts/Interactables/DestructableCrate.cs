using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableCrate : MonoBehaviour
{
    public static event EventHandler onCrateDestroy;

    [SerializeField] private Transform cratePiecesPrefab;

    private GridPosition gridPosition;

    private void Start() 
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public void Damage()
    {
        Transform cratePiecesTransform = Instantiate(cratePiecesPrefab, transform.position, transform.rotation);
        
        Vector3 randomDirection = new Vector3(
                UnityEngine.Random.Range(-1f, 1f), 
                UnityEngine.Random.Range(-1f, 1f), 
                UnityEngine.Random.Range(-1f, 1f)
            );

        AddExplosionToPieces(cratePiecesTransform, 300f, transform.position + randomDirection, 10f);
        onCrateDestroy?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    private void AddExplosionToPieces(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        foreach(Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }

            AddExplosionToPieces(child, explosionForce, explosionPosition, explosionRadius);
        }
    }

    public GridPosition GetGridPosition() => gridPosition;
}
