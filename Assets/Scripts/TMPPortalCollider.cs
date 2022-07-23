using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMPPortalCollider : MonoBehaviour
{
    [SerializeField]
    private GameStorage gameStorage;

    private void OnTriggerEnter(Collider other)
    {
        GameObject portal = GameObject.FindWithTag("PortalWall");
        Collider collider = portal.GetComponent<Collider>();
        Debug.Log(collider.ClosestPointOnBounds(transform.position));

        //Debug.Log("Cube entered trigger from: " + other.gameObject.name);
        if (other.gameObject.name == "PortalWall")
        {
            //transform.position = GameObject.FindWithTag("PortalWall").transform.position;
            //transform.rotation = gameStorage.portal_wall_base_rotation;
            transform.position = collider.ClosestPointOnBounds(transform.position);
            transform.eulerAngles = new Vector3(0, 180, 0);
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
