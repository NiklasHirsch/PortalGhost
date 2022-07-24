using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeController : MonoBehaviour
{

    [SerializeField]
    private SelectedObject selectedObject;

    private RaycastHit hit;
    private GameObject lastSelectedObject = null;
    private Outline outline;

    public LayerMask hitLayerMask;
    public float radius;

    void Update()
    {

        if (selectedObject.selectedGameObject == null || !selectedObject.selectedGameObject.GetComponent<InteractableObject>().isInfloatingStart)
        {
            //var cameraCenter = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, GetComponent<Camera>().nearClipPlane));
            if (Physics.SphereCast(Camera.main.transform.position, radius, Camera.main.transform.forward, out hit, 200, hitLayerMask, QueryTriggerInteraction.UseGlobal))
            //if (Physics.Raycast(cameraCenter, this.transform.forward, out hit, 200))
            {

                GameObject obj = hit.transform.gameObject;
                if (isInteractableObject(obj) && obj != lastSelectedObject)
                {
                    addOutline(obj);
                    selectedObject.selectedGameObject = obj;

                    removeOutline(lastSelectedObject);
                    
                    lastSelectedObject = obj;
                }


                if (!isInteractableObject(obj))
                {
                    selectedObject.selectedGameObject = null;
                   
                    removeOutline(lastSelectedObject);
                }

            }
            else
            {
                removeOutline(lastSelectedObject);
                lastSelectedObject = null;
                selectedObject.selectedGameObject = null;
            }
        }
        /*else {
            addOutline(selectedObject.selectedGameObject);
        }*/
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
                outline.OutlineWidth = 6f;
            }
        }
    }

    private bool isInteractableObject(GameObject myObject)
    {
        if (myObject != null && myObject.GetComponent<InteractableObject>() != null)
        {
            if (myObject.GetComponent<Outline>() == null)
            {
                myObject.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineColor = Color.white;
                outline.OutlineWidth = 0f;
            }
            return true;
        }
        return false;
    }
}
