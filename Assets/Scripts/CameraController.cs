using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moverSpeed;
    [SerializeField] private float rotationSpeed;

    void Update()
    {
        Vector3 moverInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 moverDirection = transform.forward * moverInput.z + transform.right * moverInput.x;
        transform.position += moverDirection * moverSpeed * Time.deltaTime;

        Vector3 rotationVector = new Vector3(0, 0, 0);

        if(Input.GetKey(KeyCode.Q))
        {
            rotationVector.y += -1f;
        }
        else if(Input.GetKey(KeyCode.E))
        {
            rotationVector.y += 1f;
        }

        rotationVector.Normalize();
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }
}
