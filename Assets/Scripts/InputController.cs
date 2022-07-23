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

        controls.FreeMoveCamera.TMPTest.performed += OnTMPTest;
    }

    private void OnDestroy()
    {
        controls.FreeMoveCamera.ToggleMenu.performed -= OnToggleMenuPerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (gameState.gameStarted)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            moveInputEvent.Invoke(moveInput.x, moveInput.y);
        }
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        if (gameState.gameStarted)
        {
            Vector2 lookInput = context.ReadValue<Vector2>();
            lookInputEvent.Invoke(lookInput.x, lookInput.y);
        }
    }

    private void OnCreatePortalPerformed(InputAction.CallbackContext context)
    {
        if (gameState.gameStarted)
        {
            var createPortalInput = context.ReadValue<float>();
            createPortalInputEvent.Invoke(createPortalInput);
        }
    }

    private void OnToggleMenuPerformed(InputAction.CallbackContext context)
    {
        if (gameState.gameStarted)
        {
            gameState.ToggleMenu(gui);
        }
    }

    private void OnTMPTest(InputAction.CallbackContext context)
    {
        if (gameState.gameStarted)
        {

            RaycastHit hit;

            string inputPortalName = "HumanPortal";
            string outputPortalName = "PortalWall";

            GameObject inputPortal = GameObject.Find(inputPortalName);
            GameObject outputPortal = GameObject.Find(outputPortalName);

            Vector3 xxx = transform.TransformPoint(outputPortal.transform.forward);

            Debug.DrawLine(outputPortal.transform.position, xxx, Color.white, 120f);

            var cameraCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));
            if (Physics.Raycast(cameraCenter, Camera.main.transform.forward, out hit, 100))
            {
                /*
                if (hit.transform.gameObject.name == inputPortalName)
                {

                    //Debug.DrawLine(cameraCenter, hit.point, Color.white, 15f);

                    Vector3 fromCamToPortal = hit.point - cameraCenter;

                    Vector3 portalSurfacePointertoHit = hit.point - hit.transform.gameObject.transform.position;

                    Vector3 portalSurfacePointerToExit = outputPortal.transform.position + portalSurfacePointertoHit;

                    Debug.DrawLine(portalSurfacePointerToExit, outputPortal.transform.forward, Color.white, 300f);
                }
                */
            }




                /*
                float m_Thrust = 700f;
                GameObject cube = GameObject.Find("TestCube");
                Rigidbody rigidbody = cube.GetComponent<Rigidbody>();
                rigidbody.AddForce(transform.forward * m_Thrust);
                */
                /*
                RaycastHit hit, hitFromPortal;
                //GameObject ghost = GameObject.FindGameObjectWithTag("GhostCamera");
                GameObject ghost = GameObject.Find("GhostCamera");
                Camera ghostCam = ghost.GetComponent<Camera>();
                var cameraCenter = ghostCam.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, ghostCam.nearClipPlane));
                if (Physics.Raycast(cameraCenter, ghostCam.transform.forward, out hit, 100))
                {
                    if (hit.transform.gameObject.name == "PortalWall")
                    {
                        //Debug.DrawLine(hit.point, ghostCam.transform.position, Color.white, 10f);
                        //GameObject obj = hit.transform.gameObject;

                        Vector3 raycastVector = ghostCam.transform.position - hit.point;



                        GameObject portal = hit.transform.gameObject;
                        Collider collider = portal.GetComponent<Collider>();

                        //transform.position = GameObject.FindWithTag("PortalWall").transform.position;
                        //transform.rotation = gameStorage.portal_wall_base_rotation;

                        //collider.ClosestPointOnBounds(transform.position)
                        Vector3 portalSurfaceVector = portal.transform.position - hit.point;
                        //Debug.DrawLine(hit.point, portal.transform.position, Color.white, 10f);

                        double angle = CalculateAngle(portalSurfaceVector, raycastVector);

                        Vector3 vectorFromPortal = Quaternion.AngleAxis((float)angle, Vector3.forward) * Vector3.right;



                        Vector3 relativeVectorFromCenterToHit = hit.point - portal.transform.position;
                        Vector3 realtvePointOnHumanPortal = GameObject.Find("HumanPortal").transform.position + relativeVectorFromCenterToHit;
                        Debug.DrawLine(realtvePointOnHumanPortal, GameObject.Find("HumanPortal").transform.up, Color.white, 10f);


                        if (Physics.Raycast(hit.point, GameObject.Find("HumanPortal").transform.forward, out hitFromPortal, 100))
                        {

                        }
                    }
                }
                */
            }
    }

    private double CalculateAngle(Vector3 vector1, Vector3 vector2)
    {
        return Math.Round(Vector3.Angle(vector1, vector2), 2);
        /*
        Debug.Log("v1: " + vector1 + "v2: " + vector2);
        double dotProduct = (vector1.x * vector2.x) + (vector1.y * vector2.y) + (vector1.z + vector2.z);
        Debug.Log("dotProxuct: "+ dotProduct);
        double magnitudeVector1 = Math.Sqrt(Math.Pow(vector1.x, 2.0) + Math.Pow(vector1.y, 2.0) + Math.Pow(vector1.z, 2.0));
        Debug.Log("magnitudeVector1: " + magnitudeVector1);
        double magnitudeVector2 = Math.Sqrt(Math.Pow(vector2.x, 2.0) + Math.Pow(vector2.y, 2.0) + Math.Pow(vector2.z, 2.0));
        Debug.Log("magnitudeVector2: " + magnitudeVector2);
        double transformedDotProduct = Math.Acos(dotProduct / (magnitudeVector1 * magnitudeVector2));
        Debug.Log("transformedDotProduct: " + transformedDotProduct);
        return transformedDotProduct;
        */
        /*
        double numerator = (vector1.x * vector2.x) + (vector1.y * vector2.y) + (vector1.z + vector2.z);
        Debug.Log(numerator);
        double denominator = Math.Sqrt(Math.Pow(vector1.x, 2) + Math.Pow(vector1.y, 2) + Math.Pow(vector1.z, 2)) * Math.Sqrt(Math.Pow(vector2.x, 2) + Math.Pow(vector2.y, 2) + Math.Pow(vector2.z, 2));
        Debug.Log(denominator);
        Debug.Log(numerator / denominator);
        Debug.Log(Math.Acos(numerator / denominator));
        return Math.Round(Math.Acos(numerator / denominator), 2);
        */
    }
}
