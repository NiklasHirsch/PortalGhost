using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VivePosTracker : MonoBehaviour
{
    [SerializeField]
    private float heightThreshold = 1.3f;

    [SerializeField]
    private SelectedObject selectedObject;

    private float startYPos; 

    // Start is called before the first frame update
    void Start()
    {
        startYPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("startYPos: " + startYPos);
        //Debug.Log("Vive Tracker Pos: " + transform.position.y);

        CheckActivate();

        CheckDeactivate();
    }

    void CheckDeactivate() {
        if (transform.position.y < startYPos + heightThreshold && selectedObject.selectedGameObject != null)
        {
            //Debug.Log("Deactivate");
            InteractableObject activeInteractableObject = selectedObject.selectedGameObject.GetComponent<InteractableObject>();
            if (activeInteractableObject != null && activeInteractableObject.isActive)
            {
                activeInteractableObject.fallDown();
            }
        }
    }

    void CheckActivate() {

        if (transform.position.y > startYPos + heightThreshold && selectedObject.selectedGameObject != null)
        {
            //Debug.Log("Activate");
            InteractableObject activeInteractableObject = selectedObject.selectedGameObject.GetComponent<InteractableObject>();
            if (activeInteractableObject != null && !activeInteractableObject.isActive)
            {
                activeInteractableObject.floatUp();
            }
        }
    }
}
