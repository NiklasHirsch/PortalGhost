using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Light mylight;
    public bool isActivated;
    public int minTriggerMass = 10;

    private int counter = 0;
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
        if (other.tag == "PPKey" && !isActivated)
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance < 0.7f)
            {
                Rigidbody box = other.GetComponent<Rigidbody>();
                
                if (box != null && mylight != null)
                {
                    if (box.mass >= minTriggerMass)
                    {
                        counter++;
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 0.5f, transform.localScale.z);
                        mylight.intensity = 3;
                        isActivated = true;
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "PPKey" && isActivated && counter == 1)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2f, transform.localScale.z);
            mylight.intensity = 0;
            counter--;
            isActivated = false;
        }
    }
}
