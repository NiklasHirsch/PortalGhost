using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{

    [SerializeField]
    private GameStorage gameStorage;


    public float moveSpeed = 5;
    public float horizontalCameraSensitivity = 20;
    public float verticalCameraSensitivity = 20;

    float mouseX = 0;
    float mouseY = 0;

    float horizontal;
    float vertical;

    float create_portal_pressed = 0;
    Vector3 portal_wall_origin = new Vector3(-100, -100, -100);
    Quaternion portal_wall_base_rotation;
    Vector3 human_portal_origin = new Vector3(-100, -100, -100);
    Vector3 human_portal_pos = new Vector3(-0.413f, 1.73f, 2.953f);
    //Quaternion human_portal_base_rotation;
    //Vector3 human_portal_pos = new Vector3(-0.951f, 1.8096f, -2.947f); // (1)

    bool human_portal_entered = false;
    bool portal_wall_entered = false;
    int timer = 0;

    Quaternion inate_rotation;

    private void Update()
    {
        if (create_portal_pressed < 1)
        {
            if (human_portal_entered && timer == 0)
            {
                transform.position = GameObject.FindWithTag("PortalWall").transform.position;
                transform.rotation = portal_wall_base_rotation;
                human_portal_entered = false;
                timer = 300;
                
            }
            if (portal_wall_entered && timer == 0)
            {
                transform.position = GameObject.FindWithTag("HumanPortal").transform.position;
                transform.eulerAngles = new Vector3(0, 180, 0);
                portal_wall_entered = false;
                timer = 300;
                
            }

            float RotationX = horizontalCameraSensitivity * mouseX * Time.deltaTime;
            float RotationY = verticalCameraSensitivity * mouseY * Time.deltaTime;

            Vector3 CameraRotation = transform.rotation.eulerAngles;
            CameraRotation.x -= RotationY;
            CameraRotation.y += RotationX;
            transform.rotation = Quaternion.Euler(CameraRotation);

            Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            //Vector3 dir = (GameObject.FindWithTag("HumanPortal").transform.position - Camera.main.transform.position).normalized;
            //GameObject.FindWithTag("PortalCamera").transform.rotation = portal_wall_base_rotation * Quaternion.LookRotation(dir);

            //GameObject.FindWithTag("PortalCamera").transform.rotation = Quaternion.Inverse(Camera.main.transform.rotation); 

            Vector3 distance = (GameObject.FindWithTag("HumanPortal").transform.position - Camera.main.transform.position).normalized;

            GameObject.FindWithTag("PortalCamera").transform.rotation = portal_wall_base_rotation * Camera.main.transform.rotation;
            GameObject.FindWithTag("PortalCamera").transform.position = GameObject.FindWithTag("PortalWall").transform.position + distance;


            //Debug.Log($"GhostController: {Camera.main.transform.rotation}");

            if (timer > 0)
            {
                timer -= 1;
                Debug.Log($"GhostController: {timer}");
            }
        }
    }

    public void OnLookInput(float x, float y)
    {
        this.mouseX = x;
        this.mouseY = y;

        //Debug.Log($"GhostController: LookInput: {x}, {y}");
    }

    public void OnMoveInput(float horizontal, float vertical)
    {
        this.vertical = vertical;
        this.horizontal = horizontal;
        
        //Debug.Log($"GhostController: MoveInput: {vertical}, {horizontal}");
    }

    public void OnCreatePortalInput(float create_portal_pressed)
    {
        this.create_portal_pressed = create_portal_pressed;

        if (create_portal_pressed == 1)
        {
            GameObject.FindWithTag("PortalWall").transform.position = transform.position; // - transform.forward; spawn at distance
            GameObject.FindWithTag("PortalWall").transform.rotation = transform.rotation;
            portal_wall_base_rotation = transform.rotation;
            gameStorage.portal_wall_base_rotation = portal_wall_base_rotation;

            GameObject.FindWithTag("PortalCamera").transform.position = transform.position;
            GameObject.FindWithTag("PortalCamera").transform.rotation = transform.rotation;

            inate_rotation = transform.rotation;

            GameObject.FindWithTag("HumanPortal").transform.position = human_portal_pos;
            //human_portal_base_rotation = GameObject.FindWithTag("HumanPortal").transform.rotation;
        }
        else
        {
            // dont return portal wall for debuging
            //GameObject.FindWithTag("PortalWall").transform.position = portal_wall_origin;
            // dont return human portal for debuging
            //GameObject.FindWithTag("HumanPortal").transform.position = human_portal_origin;
        }

        //Debug.Log($"InputController: createPortalInput: {create_portal_pressed}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ghost entered trigger from: " + other.gameObject.name);     
        if(other.gameObject.tag == "HumanPortal")
        {
            human_portal_entered = true;      
        }
        if (other.gameObject.tag == "PortalWall")
        {
            portal_wall_entered = true;       
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("stay");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Ghost exited trigger from: " + other.gameObject.name);

        if (other.gameObject.tag == "HumanPortal")
        {
            human_portal_entered = false;
        }
        if (other.gameObject.tag == "PortalWall")
        {
            portal_wall_entered = false;
        }
    }
}
