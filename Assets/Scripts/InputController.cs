using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class Vector2InputEvent: UnityEvent<float, float> { }

public class InputController : MonoBehaviour
{
    Controls controls;

    public Vector2InputEvent moveInputEvent;
    public Vector2InputEvent lookInputEvent;

    private void Awake()
    {
        controls = new Controls();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        controls.FreeMoveCamera.Enable();

        controls.FreeMoveCamera.Move.performed += OnMovePerformed;
        controls.FreeMoveCamera.Move.canceled += OnMovePerformed;

        controls.FreeMoveCamera.Look.performed += OnLookPerformed;
        controls.FreeMoveCamera.Look.canceled += OnLookPerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        moveInputEvent.Invoke(moveInput.x, moveInput.y);
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
        lookInputEvent.Invoke(lookInput.x, lookInput.y);
    }
}
