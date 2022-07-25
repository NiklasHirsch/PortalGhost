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

    private bool hitNonInteractableLastTime = true;

    void Update()
    {

        if (selectedObject.selectedGameObject == null || !selectedObject.selectedGameObject.GetComponent<InteractableObject>().isInfloatingStart)
        {
            //var cameraCenter = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, GetComponent<Camera>().nearClipPlane));
            if (Physics.SphereCast(Camera.main.transform.position, radius, Camera.main.transform.forward, out hit, 200, hitLayerMask, QueryTriggerInteraction.UseGlobal))
            //if (Physics.Raycast(cameraCenter, this.transform.forward, out hit, 200))
            {
                string inputPortalName = "HumanPortal";
                string outputPortalName = "GhostPortal";

                GameObject inputPortal = GameObject.Find(inputPortalName);
                GameObject outputPortal = GameObject.Find(outputPortalName);
                //Debug.Log("Obj" + hit.transform.gameObject);
                if (hit.transform.gameObject == inputPortal)
                {
                    RaycastHit hit2, hitOut;

                    //GameObject ghostObj = GameObject.Find("GhostCamera");
                    Camera camToTrack = Camera.main;

                    var cameraCenter = camToTrack.transform.position;

                    if (Physics.Raycast(cameraCenter, camToTrack.transform.forward, out hit2, 200))
                    {
                       
                        if (hit2.transform.gameObject == inputPortal)
                        {
                            Debug.DrawLine(cameraCenter, hit2.point, Color.white, 120f);

                            Vector3 fromCamToPortal = hit2.point - cameraCenter;
                            //Vector3 fromCamToPortal = cameraCenter - hit2.point;


                            Vector3 portalCenterPointertoHit = hit2.point - hit2.transform.gameObject.transform.position;

                            Quaternion rotation_difference = outputPortal.transform.rotation * Quaternion.Inverse(inputPortal.transform.rotation);

                            Vector3 portalCenterPointerToExit = outputPortal.transform.position + (rotation_difference * portalCenterPointertoHit);

                            Vector3 bla = portalCenterPointerToExit + (outputPortal.transform.rotation * fromCamToPortal).normalized * 0.3f;

                            //if (Physics.Raycast(portalCenterPointerToExit, (outputPortal.transform.rotation * fromCamToPortal).normalized, out hitOut, 100))
                            if(Physics.SphereCast(portalCenterPointerToExit, radius, outputPortal.transform.rotation * fromCamToPortal, out hitOut, 200, hitLayerMask, QueryTriggerInteraction.UseGlobal))
                            {
                                Debug.DrawLine(portalCenterPointerToExit, hitOut.point, Color.white, 120f);
                                GameObject obj = hitOut.transform.gameObject;
                                Debug.Log("Raycast: " + obj);

                                obj.GetComponent<InteractableObject>().isSelectedThroughPortal = true;
                                hitObjResolve(obj);

                            }
                        }
                    }


                }
                else
                {
                    GameObject obj = hit.transform.gameObject;
                    obj.GetComponent<InteractableObject>().isSelectedThroughPortal = false;
                    hitObjResolve(obj);
                }

            }
            else
            {
                removeOutline(lastSelectedObject);
                //lastSelectedObject = null;
                hitNonInteractableLastTime = true;
                selectedObject.selectedGameObject = null;
            }
        }
        /*else {
            addOutline(selectedObject.selectedGameObject);
        }*/
    }

    private void hitObjResolve(GameObject obj)
    {
        if (isInteractableObject(obj) && obj != lastSelectedObject)
        {
            addOutline(obj);
            selectedObject.selectedGameObject = obj;

            removeOutline(lastSelectedObject);

            lastSelectedObject = obj;
            hitNonInteractableLastTime = false;
        }

        if (isInteractableObject(obj) && obj == lastSelectedObject && hitNonInteractableLastTime)
        {
            removeOutline(lastSelectedObject);

            addOutline(obj);
            selectedObject.selectedGameObject = obj;


            lastSelectedObject = obj;
            hitNonInteractableLastTime = false;
        }



        if (!isInteractableObject(obj))
        {
            selectedObject.selectedGameObject = null;
            hitNonInteractableLastTime = true;
            removeOutline(lastSelectedObject);
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
                if (outline == null)
                {
                    addOutline(myObject);
                }
                
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineColor = Color.white;
                outline.OutlineWidth = 0f;
            }
            return true;
        }
        return false;
    }
}
