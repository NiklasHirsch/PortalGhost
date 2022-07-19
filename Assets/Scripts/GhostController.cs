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

    private void Update()
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
}
