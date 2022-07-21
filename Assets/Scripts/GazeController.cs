using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeController : MonoBehaviour
{

    [SerializeField]
    private SelectedObject selectedObject;

    private RaycastHit hit;
    private GameObject activeObject = null;
    private Outline outline;

    void Update()
    {
        var cameraCenter = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, GetComponent<Camera>().nearClipPlane));
        if (Physics.Raycast(cameraCenter, this.transform.forward, out hit, 100))
        {
            GameObject obj = hit.transform.gameObject;

            if (!isInteractableObject(obj))
            {
                removeOutline(activeObject);
                return;
            }

            if (obj != activeObject)
            {
                if (activeObject != null)
                {
                    removeOutline(activeObject);
                }
                activeObject = obj;
                addOutline(obj);
            }
        }
        else
        {
            removeOutline(activeObject);
            activeObject = null;
            selectedObject.selectedGameObject = null;
        }
    }

    private void removeOutline(GameObject theObject)
    {
        if (theObject != null)
        {
            outline = theObject.GetComponent<Outline>();
            if (outline != null)
            {
                outline.OutlineWidth = 0f;
            }
        }
    }

    private void addOutline(GameObject myObject)
    {
        if (myObject != null)
        {
            outline = myObject.GetComponent<Outline>();
            if (outline != null)
            {
                selectedObject.selectedGameObject = myObject;
                outline.OutlineWidth = 6f;
            }
        }
    }

    private bool isInteractableObject(GameObject myObject)
    {
        if (myObject.GetComponent<InteractableObject>() != null && myObject.GetComponent<Outline>() == null)
        {
            myObject.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.white;
            outline.OutlineWidth = 0f;
        }
        return (myObject.GetComponent<Outline>() != null);
    }
}
