using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("zero if");
        if (other.tag == "PPKey")
        {
            Debug.Log("first if");
            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance < 0.5f)
            {
                Debug.Log("second if");
                Rigidbody box = other.GetComponent<Rigidbody>();
                if (box != null)
                {
                    //if (box.mass >= 10)
                    //{
                        Debug.Log("triggered");
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y*0.3f, transform.localScale.z);
                    //}
                }
            }
        }
    }
}
