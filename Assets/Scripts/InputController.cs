using System;
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

        controls.FreeMoveCamera.TMP.performed += OnTMP;
    }

    private void OnTMP(InputAction.CallbackContext context)
    {
        if (true)
        {

            RaycastHit hit, hitOut;

            string inputPortalName = "HumanPortal";
            string outputPortalName = "GhostPortal";

            GameObject inputPortal = GameObject.Find(inputPortalName);
            GameObject outputPortal = GameObject.Find(outputPortalName);

            GameObject ghostObj = GameObject.Find("GhostCamera");
            Camera ghostCam = ghostObj.GetComponent<Camera>();
            //Camera ghostCam = Camera.main;

            //Debug.DrawLine(outputPortal.transform.position, xxx, Color.white, 120f);

            //var cameraCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));
            var cameraCenter = ghostCam.transform.position;

            if (Physics.Raycast(cameraCenter, ghostCam.transform.forward, out hit, 100))
            {
                Debug.Log(1);
                if (hit.transform.gameObject == inputPortal)
                {
                    Debug.Log(2);
                    Debug.DrawLine(cameraCenter, hit.point, Color.white, 120f);

                    Vector3 fromCamToPortal = hit.point - cameraCenter;

                    float help = hit.transform.gameObject.transform.position.x - hit.point.x;
                    float new_x = hit.transform.gameObject.transform.position.x + help;

                    Vector3 mirrored_x = new Vector3(new_x, hit.point.y, hit.point.z);

                    Vector3 portalCenterPointertoHit = mirrored_x - hit.transform.gameObject.transform.position;

                    Quaternion rotation_difference = outputPortal.transform.rotation * Quaternion.Inverse(hit.transform.gameObject.transform.rotation);

                    Vector3 portalCenterPointerToExit = outputPortal.transform.position - (rotation_difference * portalCenterPointertoHit);


                    Debug.DrawLine(outputPortal.transform.position, portalCenterPointerToExit);
                    Debug.Log(portalCenterPointerToExit);



                    


                    //Debug.DrawRay(portalCenterPointerToExit, outputPortal.transform.up);
                    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    //cube.transform.position = portalCenterPointerToExit;

                    if (Physics.Raycast(portalCenterPointerToExit, (Quaternion.FromToRotation(inputPortal.transform.up, ghostCam.transform.forward)) * outputPortal.transform.up * (-1), out hitOut, 100))
                    {

                        

                        Debug.Log(3);
                        Debug.DrawLine(portalCenterPointerToExit, hitOut.point, Color.white, 120f);
                    }
                    //Debug.DrawLine(portalCenterPointerToExit, outputPortal.transform.forward, Color.white, 300f);
                }
                
            }


        }

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
