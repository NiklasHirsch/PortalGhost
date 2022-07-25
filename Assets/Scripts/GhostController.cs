using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GhostController : MonoBehaviour
{
    public float moveSpeed = 5;
    public float horizontalCameraSensitivity = 20;
    public float verticalCameraSensitivity = 20;

    float mouseX = 0;
    float mouseY = 0;

    float horizontal;
    float vertical;

    bool portal_spawned = false;

    Vector3 portal_wall_origin = new Vector3(-100, -100, -100);
    Quaternion portal_wall_base_rotation;
    Vector3 human_portal_origin = new Vector3(-100, -100, -100);
    Vector3 human_portal_pos = new Vector3(-0.4287394f, 1.723f, 2.978f);
    //Quaternion human_portal_base_rotation;

    // Debug portal collision
    //bool human_portal_entered = false;
    //bool portal_wall_entered = false;
    //int timer = 0;

    Vector3 innate_position;
    Vector3 innate_forward;

    Vector3 innate_human_position;

    private void Update()
    {
        if (portal_spawned)
        {
			// Debug: portal collision
			//if (human_portal_entered && timer == 0)
			//{
			//    transform.position = GameObject.FindWithTag("PortalWall").transform.position;
			//    transform.rotation = portal_wall_base_rotation;
			//    human_portal_entered = false;
			//    timer = 300;

			//}
			//if (portal_wall_entered && timer == 0)
			//{
			//    transform.position = GameObject.FindWithTag("HumanPortal").transform.position;
			//    transform.eulerAngles = new Vector3(0, 180, 0);
			//    portal_wall_entered = false;
			//    timer = 300;

			//}

			// Debug: ghost can moce freely
			float RotationX = horizontalCameraSensitivity * mouseX * Time.deltaTime;
			float RotationY = verticalCameraSensitivity * mouseY * Time.deltaTime;
			Vector3 CameraRotation = transform.rotation.eulerAngles;
			CameraRotation.x -= RotationY;
			CameraRotation.y += RotationX;
			transform.rotation = Quaternion.Euler(CameraRotation);
			Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
			transform.position += moveDirection * moveSpeed * Time.deltaTime;

			// Portal visuals:
			// Get directional vectors from the human to the HumanPortal
			Vector3 dir = (GameObject.FindWithTag("HumanPortal").transform.position - Camera.main.transform.position).normalized;
            Vector3 distance = GameObject.FindWithTag("HumanPortal").transform.position - Camera.main.transform.position;

            // get base rotation of the camera (workaround to counteract VR-Tracking)
            GameObject.FindWithTag("PortalCamera").transform.rotation = portal_wall_base_rotation;

            // change camera distance and cliping plane to account for player distance to HumanPortal. This is important to mimic the real distance between the player and objects seen through the portal.
            GameObject.FindWithTag("PortalCamera").transform.position = innate_position - (GameObject.FindWithTag("PortalCamera").transform.forward * distance.magnitude);
            GameObject.FindWithTag("PortalCameraView").GetComponent<Camera>().nearClipPlane = distance.magnitude;

            // adjust FoV depending on the humans distance to the HumanPortal (depricated / not needed? / calculated in VR automaticaly)
            // GameObject.FindWithTag("PortalCameraView").GetComponent<Camera>().fieldOfView = (float)(2 * Math.Atan(60 / (2 * distance.magnitude)));

            // account for VR-Tracking (position) (workaround)
            Vector3 v = Camera.main.transform.position - innate_human_position;
            GameObject.FindWithTag("PortalCamera").transform.position += GameObject.FindWithTag("PortalCamera").transform.right * v.x;
            GameObject.FindWithTag("PortalCamera").transform.position += GameObject.FindWithTag("PortalCamera").transform.up * v.y;

            // stop the VR-Tracking moving the PortalCamera if the player orbits around the HumanPortal (workaround)
            float angle = Vector3.Angle(dir, GameObject.FindWithTag("HumanPortal").transform.right);
            if (angle > 0 && angle < 85)
            {
                float inv_angle = (angle - 90) * -1;
                GameObject.FindWithTag("PortalCamera").transform.position += ((GameObject.FindWithTag("PortalCamera").transform.forward * distance.magnitude) / 90) * inv_angle;
            }

            if (angle >= 85 && angle < 96) // tries to smooth out the PortalCamera movement around 90� to HumanPortal
            {
                GameObject.FindWithTag("PortalCamera").transform.position += ((GameObject.FindWithTag("PortalCamera").transform.forward * distance.magnitude) / 100) * 5;
            }

            if (angle >= 96 && angle < 180)
            {
                angle = (angle - 90);
                GameObject.FindWithTag("PortalCamera").transform.position += ((GameObject.FindWithTag("PortalCamera").transform.forward * distance.magnitude) / 90) * angle;
            }

            // account for VR-Tracking (rotation) (workaround)
            GameObject.FindWithTag("PortalCamera").transform.rotation *= Camera.main.transform.rotation;

            // Debug portal collision
            //if (timer > 0)
            //{
            //    timer -= 1;
            //    Debug.Log($"GhostController: {timer}");
            //}
        }
        else // ghost movement
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
    }

    public void OnLookInput(float x, float y)
    {
        this.mouseX = x;
        this.mouseY = y;
    }

    public void OnMoveInput(float horizontal, float vertical)
    {
        this.vertical = vertical;
        this.horizontal = horizontal;
    }

    public void OnCreatePortalInput(float create_portal_pressed)
    {
        if (create_portal_pressed == 1) //
        {
            portal_spawned = true;

            GameObject.FindWithTag("PortalWall").transform.position = transform.position;
            GameObject.FindWithTag("PortalWall").transform.rotation = transform.rotation;

            portal_wall_base_rotation = transform.rotation;

            GameObject.FindWithTag("PortalCamera").transform.position = transform.position;
            GameObject.FindWithTag("PortalCamera").transform.rotation = transform.rotation;

            innate_position = transform.position;
            innate_forward = transform.forward;
            innate_human_position = Camera.main.transform.position;

            GameObject.FindWithTag("HumanPortal").transform.position = human_portal_pos;

            // take account for HumanPortal rotation (not implemented yet)
            //human_portal_base_rotation = GameObject.FindWithTag("HumanPortal").transform.rotation;
        }
        else
        {
            // dont return portal wall for debuging
            //GameObject.FindWithTag("PortalWall").transform.position = portal_wall_origin;
            // dont return human portal for debuging
            //GameObject.FindWithTag("HumanPortal").transform.position = human_portal_origin;

            //portal_spawned = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug portal collision
        //Debug.Log("Ghost entered trigger from: " + other.gameObject.name);     
        //if(other.gameObject.tag == "HumanPortal")
        //{
        //    human_portal_entered = true;      
        //}
        //if (other.gameObject.tag == "PortalWall")
        //{
        //    portal_wall_entered = true;       
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        //
    }

    private void OnTriggerExit(Collider other)
    {
        // Debug portal collision
        //Debug.Log("Ghost exited trigger from: " + other.gameObject.name);
        //if (other.gameObject.tag == "HumanPortal")
        //{
        //    human_portal_entered = false;
        //}
        //if (other.gameObject.tag == "PortalWall")
        //{
        //    portal_wall_entered = false;
        //}
    }
}
