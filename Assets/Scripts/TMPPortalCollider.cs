using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMPPortalCollider : MonoBehaviour
{
    [SerializeField]
    private GameStorage gameStorage;

    private void OnTriggerEnter(Collider other)
    {
        
        //Debug.Log(collider.ClosestPointOnBounds(transform.position));

        //Debug.Log("Cube entered trigger from: " + other.gameObject.name);
        if (other.gameObject.name == "PortalWall")
        {
            GameObject portal = other.gameObject;
            GameObject portalHuman = GameObject.FindWithTag("HumanPortal");
            Collider collider = portal.GetComponent<Collider>();

            //transform.position = GameObject.FindWithTag("PortalWall").transform.position;
            //transform.rotation = gameStorage.portal_wall_base_rotation;

            
            Vector3 realtiveVector = portal.transform.position - collider.ClosestPointOnBounds(transform.position);
            transform.position = portalHuman.transform.position + realtiveVector;
            transform.eulerAngles = new Vector3(0, 180, 0);

            //Debug.DrawLine(portalHuman.transform.position, portalHuman.transform.position + realtiveVector, Color.white, 10f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("stay");
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Cube exited trigger from: " + other.gameObject.name);
    }
}
