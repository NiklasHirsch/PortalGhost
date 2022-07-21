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
    Vector3 portal_wall_origin = new Vector3(-100, -100, -100);
    Vector3 human_portal_origin = new Vector3(-100, -100, -100);
    Vector3 human_portal_pos = new Vector3(1.086f, 1.136f, 1.585f);
    //Vector3 human_portal_pos = new Vector3(-0.951f, 1.8096f, -2.947f); // (1)


    private void Update()
    {
        if (create_portal_pressed < 1)
        {
            float RotationX = horizontalCameraSensitivity * mouseX * Time.deltaTime;
            float RotationY = verticalCameraSensitivity * mouseY * Time.deltaTime;

            Vector3 CameraRotation = transform.rotation.eulerAngles;
            CameraRotation.x -= RotationY;
            CameraRotation.y += RotationX;
            transform.rotation = Quaternion.Euler(CameraRotation);

            Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            Vector3 pos = transform.position;
            Vector3 dir = (GameObject.FindWithTag("HumanPortal").transform.position - transform.position).normalized;
            Debug.Log($"GhostController: PortalDir: {dir}");

            //GameObject.FindWithTag("PortalCamera").transform.forward = Quaternion.Euler(dir) * GameObject.FindWithTag("PortalCamera").transform.forward;

            //Vector3 forwardVector = Quaternion.Euler(yourVector3Rotation) * Vector3.forward

            //transform.rotation = Quaternion.LookRotation(directionVector);

            GameObject.FindWithTag("PortalCamera").transform.rotation = Quaternion.LookRotation(dir);
        }
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

        if (create_portal_pressed == 1)
        {
            GameObject.FindWithTag("PortalWall").transform.position = transform.position - transform.forward;
            GameObject.FindWithTag("PortalWall").transform.rotation = transform.rotation;

            GameObject.FindWithTag("PortalCamera").transform.position = transform.position;
            GameObject.FindWithTag("PortalCamera").transform.rotation = transform.rotation;

            GameObject.FindWithTag("HumanPortal").transform.position = human_portal_pos;
        }
        else
        {
            GameObject.FindWithTag("PortalWall").transform.position = portal_wall_origin;
            // dont return human portal for debuging
            //GameObject.FindWithTag("HumanPortal").transform.position = human_portal_origin;
        }

        Debug.Log($"InputController: createPortalInput: {create_portal_pressed}");
    }
}
