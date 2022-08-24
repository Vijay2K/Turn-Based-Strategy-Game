using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool isInvert;
    private Transform cameraTransform;

    private void Awake() 
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate() 
    {
        if(isInvert)
        {
            Vector3 camDirection = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position + camDirection * -1);
        }
        else
        {
            transform.LookAt(cameraTransform.position);
        }
    }
}
