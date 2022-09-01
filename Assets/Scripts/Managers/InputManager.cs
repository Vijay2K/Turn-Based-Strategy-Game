#define USE_NEW_INPUT_SYSTEM

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerInputAction playerInputAction;

    private void Awake() 
    {
        if(Instance != null)
        {
            Debug.LogError($"More than one instance for {this}");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        playerInputAction = new PlayerInputAction();
    }

    private void OnEnable() 
    {
        playerInputAction.Enable();
    }

    public Vector2 GetMouseScreenPosition()
    {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else        
        return Input.mousePosition;
#endif
    }

    public bool IsMouseButtonDownThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputAction.Player.Click.WasPressedThisFrame();
#else        
        return Input.GetMouseButtonDown(0);
#endif    
    }

    public Vector2 GetMovementInput()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputAction.Player.Movement.ReadValue<Vector2>();
#else
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        return inputVector;
#endif
    }

    public float GetCameraRotateAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputAction.Player.Rotate.ReadValue<float>();
#else
        float rotateAmount = 0f;

        if(Input.GetKey(KeyCode.Q))
        {
            rotateAmount += 1f;
        } else if(Input.GetKey(KeyCode.E)) 
        {
            rotateAmount -= 1f;
        }

        return rotateAmount;
#endif
    }

    public Vector2 GetMouseScrollDelta()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputAction.Player.MouseScrollDelta.ReadValue<Vector2>();
#else         
        return Input.mouseScrollDelta;
#endif  
    }

    private void OnDisable() 
    {
        playerInputAction.Disable();
    }
}
