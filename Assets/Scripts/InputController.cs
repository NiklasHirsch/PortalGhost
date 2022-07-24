using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class Vector2InputEvent : UnityEvent<float, float> { }

[System.Serializable]
public class ButtonInputEvent : UnityEvent<float> { }

public class InputController : MonoBehaviour
{
    [SerializeField]
    private GameState gameState;

    [SerializeField]
    private SelectedObject selectedObject;

    Controls controls;

    public Vector2InputEvent moveInputEvent;
    public Vector2InputEvent lookInputEvent;
    public ButtonInputEvent createPortalInputEvent;
    protected GameObject gui;

    private void Awake()
    {
        controls = new Controls();
        gui = GameObject.Find("GUI");
        gameState.menuOpen = true;
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

        controls.FreeMoveCamera.ToggleMenu.performed += OnToggleMenuPerformed;
        //controls.FreeMoveCamera.ToggleMenu.canceled += OnToggleMenuPerformed;

        controls.FreeMoveCamera.ActivatePower.performed += OnActivatePower;

        controls.FreeMoveCamera.PowerPush.performed += OnPowerPush;

        controls.FreeMoveCamera.PowerPull.performed += OnPowerPull;

        controls.FreeMoveCamera.PowerStay.performed += OnPowerStay;
    }

    private void OnDestroy()
    {
        controls.FreeMoveCamera.ToggleMenu.performed -= OnToggleMenuPerformed;
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

    private void OnCreatePortalPerformed(InputAction.CallbackContext context)
    {
        var createPortalInput = context.ReadValue<float>();
        createPortalInputEvent.Invoke(createPortalInput);
    }

    private void OnToggleMenuPerformed(InputAction.CallbackContext context)
    {
        if (gameState.gameStarted)
        {
            gameState.ToggleMenu(gui);
        }
    }

    private void OnActivatePower(InputAction.CallbackContext context)
    {
        if (selectedObject != null)
        {
            InteractableObject interactableObj = selectedObject.selectedGameObject.GetComponent<InteractableObject>();
            if (!interactableObj.isActive)
            {
                interactableObj.floatUp();
            }
            else
            {
                //interactableObj.fallDown();
            }
        }

    }

    private void OnPowerPush(InputAction.CallbackContext context)
    {
        
        if (selectedObject != null)
        {
            InteractableObject interactableObj = selectedObject.selectedGameObject.GetComponent<InteractableObject>();
            if (interactableObj.isActive)
            {
                interactableObj.push();
            }
        }
        
    }

    private void OnPowerPull(InputAction.CallbackContext context)
    {
        if (selectedObject != null)
        {
            InteractableObject interactableObj = selectedObject.selectedGameObject.GetComponent<InteractableObject>();
            if (interactableObj.isActive)
            {
                interactableObj.pull();
            }
        }
    }

    private void OnPowerStay(InputAction.CallbackContext context)
    {
        if (selectedObject != null)
        {
            InteractableObject interactableObj = selectedObject.selectedGameObject.GetComponent<InteractableObject>();
            if (interactableObj.isActive)
            {
                interactableObj.nullClassCase();
            }
        }
    }
}
