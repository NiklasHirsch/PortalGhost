using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Light mylight;
    // Start is called before the first frame update
    void Start()
    {
        mylight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PPKey")
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance < 0.5f)
            {
                Rigidbody box = other.GetComponent<Rigidbody>();
                
                if (box != null && mylight != null)
                {
                    //if (box.mass >= 10)
                    //{
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 0.3f, transform.localScale.z);
                    mylight.intensity = 3;
                    //}
                }
            }
        }
    }
}
