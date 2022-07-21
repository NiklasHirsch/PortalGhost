using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public float moveSpeed = 5;
    public float horizontalCameraSensitivity = 20;
    public float verticalCameraSensitivity = 20;

    float mouseX = 0;
    float mouseY = 0;

    float horizontal;
    float vertical;

    float create_portal_pressed = 0;
    bool portal_wall_exists = false;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private PortalPair portals;

    [SerializeField]
    private Crosshair crosshair;

    private void Update()
    {
        if(create_portal_pressed < 1)
        {
            float RotationX = horizontalCameraSensitivity * mouseX * Time.deltaTime;
            float RotationY = verticalCameraSensitivity * mouseY * Time.deltaTime;

            Vector3 CameraRotation = transform.rotation.eulerAngles;
            CameraRotation.x -= RotationY;
            CameraRotation.y += RotationX;
            transform.rotation = Quaternion.Euler(CameraRotation);

            Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            if (!portal_wall_exists)
            {
                GameObject.FindWithTag("PortalWall").transform.position = transform.position - transform.forward;
                GameObject.FindWithTag("PortalWall").transform.rotation = transform.rotation;
  
                // TODO: remove wall again

            }
        }
    }

    private void FirePortal(int portalID, Vector3 pos, Vector3 dir, float distance)
    {
        Debug.Log("1: FIRE");
        RaycastHit hit;
        Physics.Raycast(pos, dir, out hit, distance, ~0);

        Debug.Log($"THE COLIDER IS: {hit.collider}");



        if (hit.collider != null)
        {
            Debug.Log("2: Colider was not NUll");
            // If we shoot a portal, recursively fire through the portal.
            if (hit.collider.tag == "Portal")
            {
                var inPortal = hit.collider.GetComponent<Portal>();

                if (inPortal == null)
                {
                    Debug.Log("PORTAL IS NULL RETURN");
                    return;
                }

                var outPortal = inPortal.OtherPortal;

                // Update position of raycast origin with small offset.
                Vector3 relativePos = inPortal.transform.InverseTransformPoint(hit.point + dir);
                relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
                pos = outPortal.transform.TransformPoint(relativePos);

                // Update direction of raycast.
                Vector3 relativeDir = inPortal.transform.InverseTransformDirection(dir);
                relativeDir = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeDir;
                dir = outPortal.transform.TransformDirection(relativeDir);

                distance -= Vector3.Distance(pos, hit.point);

                FirePortal(portalID, pos, dir, distance);

                return;
            }

            // Orient the portal according to cmera look direction and surface direction.
            var cameraRotation = Camera.main.transform.rotation;
            var portalRight = cameraRotation * Vector3.right;

            if (Mathf.Abs(portalRight.x) >= Mathf.Abs(portalRight.z))
            {
                portalRight = (portalRight.x >= 0) ? Vector3.right : -Vector3.right;
            }
            else
            {
                portalRight = (portalRight.z >= 0) ? Vector3.forward : -Vector3.forward;
            }

            var portalForward = -hit.normal;
            var portalUp = -Vector3.Cross(portalRight, portalForward);

            var portalRotation = Quaternion.LookRotation(portalForward, portalUp);

            // Attempt to place the portal.
            bool wasPlaced = portals.Portals[portalID].PlacePortal(hit.collider, hit.point, portalRotation);

            if (wasPlaced)
            {
                Debug.Log("PORTAL WAS PLACEF");
                crosshair.SetPortalPlaced(portalID, true);
            }
        }
        Debug.Log("WE LEAVE FIRE METHOD");
    }

    public void OnLookInput(float x, float y)
    {
        this.mouseX = x;
        this.mouseY = y;

        Debug.Log($"GhostController: LookInput: {x}, {y}");
    }

    public void OnMoveInput(float horizontal, float vertical)
    {
        this.vertical = vertical;
        this.horizontal = horizontal;
        
        Debug.Log($"GhostController: MoveInput: {vertical}, {horizontal}");
    }

    public void OnCreatePortalInput(float create_portal_pressed)
    {
        this.create_portal_pressed = create_portal_pressed;

        FirePortal(0, transform.position, transform.forward, 250.0f);
        Debug.Log("POOOOOOOOOOOOOOOOOOOOOOOOORTAAAAAAAAAAAAAAAAAAAAAAAAAAAAALLLLLLLLLLLLLL");

        Debug.Log($"InputController: createPortalInput: {create_portal_pressed}");
    }
}
