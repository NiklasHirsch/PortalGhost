using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject pressurePlate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "PPKey")
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance < 0.1f)
            {
                Rigidbody box = other.GetComponent<Rigidbody>();
                if (box != null)
                {
                    if (box.mass >= 10)
                    {
                        pressurePlate.transform.position = new Vector3(pressurePlate.transform.position.x, pressurePlate.transform.position.y - 0.2, pressurePlate.transform.position.z);
                    }
                }
            }
        }
    }
}
