using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingMultiply : MonoBehaviour
{
    public float addedWalkingMultiplier = 0.5f;

    public Transform vrCamera = null;


    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.localPosition = new Vector3(vrCamera.transform.localPosition.x * addedWalkingMultiplier, 0, vrCamera.transform.localPosition.z * addedWalkingMultiplier);
    }
}
