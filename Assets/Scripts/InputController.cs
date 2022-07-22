using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class Vector2InputEvent: UnityEvent<float, float> { }

[System.Serializable]
public class ButtonInputEvent : UnityEvent<float> { }

public class InputController : MonoBehaviour
{
    Controls controls;

    public Vector2InputEvent moveInputEvent;
    public Vector2InputEvent lookInputEvent;
    public ButtonInputEvent createPortalInputEvent;

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

        controls.FreeMoveCamera.CreatePortal.performed += OnCreatePortalPerformed;
        controls.FreeMoveCamera.CreatePortal.canceled += OnCreatePortalPerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        GameObject.Find("HumanMale_Character_FREE").GetComponent<Animator>().Play("RunForward");

        Vector2 moveInput = context.ReadValue<Vector2>();
        moveInputEvent.Invoke(moveInput.x, moveInput.y);
    }



    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
        lookInputEvent.Invoke(lookInput.x, lookInput.y);
    }

    private void OnCreatePortalPerformed(InputAction.CallbackContext context)
    {
        var createPortalInput = context.ReadValue<float>();
        createPortalInputEvent.Invoke(createPortalInput);
    }
}
