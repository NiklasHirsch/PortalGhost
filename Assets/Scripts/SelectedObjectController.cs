using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObjectController : MonoBehaviour
{
    
    [SerializeField]
    private SelectedObject selectedObject;

    void Update()
    {
        //Debug.Log(selectedObject.selectedGameObject.GetComponent<InteractableObject>().getState());
        //selectedObject.selectedGameObject.GetComponent<InteractableObject>().pull();
        //Debug.Log(selectedObject.selectedGameObject.name);
    }
}