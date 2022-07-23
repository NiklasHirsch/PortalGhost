using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMPPortalWallCollider : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("adsf");
        Debug.Log(transform.GetComponent<Renderer>().bounds.center);
    }

    private void OnCollisionEnter(Collision collisionObject)
    {
        /*
        Debug.Log("hier"); 
        foreach (ContactPoint contact in collisionObject.contacts)
        {
            Debug.Log(contact.point);
            //Debug.Log(contact.normal);
        }
        */
        /*
        Debug.Log("Portal impact from : " + collisionObject.gameObject.name);
        ContactPoint contact = collisionObject.contacts[0];
        Debug.Log("contact");
        Debug.Log(contact);
        //Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        //Debug.Log("rotation");
        //Debug.Log(rotation);
        Vector3 position = contact.point;
        Debug.Log("position");
        Debug.Log(position);
        */
        
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("portal stay");
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Portal exit: " + other.gameObject.name);
    }
}
