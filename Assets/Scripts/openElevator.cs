using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openElevator : MonoBehaviour
{
    public PressurePlate redPressurePlateActive;
    public PressurePlate greenPressurePlateActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(redPressurePlateActive.isActivated && greenPressurePlateActive.isActivated) {
            Destroy(gameObject);
            Destroy(this);
            GameObject.Find("LampCeilingMain").GetComponent<FlickeringLightController>().StartFlicker();
        }
    }
}
