using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float cam_moverSpeed;
    [SerializeField] private float cam_rotationSpeed;
    [SerializeField] private float cam_zoomSpeed;

    private const float MIN_FOLLOW_Y_OFFSET = 2;
    private const float MAX_FOLLOW_Y_OFFSET = 12;
    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }

    void Update()
    {
        HandleMovement();

        HandleRotation();

        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector2 moverInput = InputManager.Instance.GetMovementInput();
        Vector3 moverDirection = transform.forward * moverInput.y + transform.right * moverInput.x;
        transform.position += moverDirection * cam_moverSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);
        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();

        rotationVector.Normalize();
        transform.eulerAngles += rotationVector * cam_rotationSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        float zoomValue = 1f;
        if (InputManager.Instance.GetMouseScrollDelta().y > 0)
        {
            targetFollowOffset.y -= zoomValue;
        }
        else if (InputManager.Instance.GetMouseScrollDelta().y < 0)
        {
            targetFollowOffset.y += zoomValue;
        }

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset,
            Time.deltaTime * cam_zoomSpeed);
    }

}
