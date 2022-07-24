using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableButton : InteractableObject
{

    public override void floatUp() {
        isActive = true;
    }
    
    public override void fallDown() {
        isActive = false;
    }

    public override void pull() {

    }

    public override void push() {
        GameObject elevatorObj = GameObject.FindGameObjectWithTag("Elevator");
        startPushAnimation();
        if (elevatorObj != null) {

            elevatorObj.GetComponent<setDisplayNumbers>().addNumber(this.gameObject);
        }
    }
    public void startPushAnimation()
    {
        StartCoroutine(pushAnimation());
    }

    IEnumerator pushAnimation()
    {
        AudioSource audioPress = transform.Find("AudioPress").GetComponent<AudioSource>();
        AudioSource audioRelease = transform.Find("AudioRelease").GetComponent<AudioSource>();
        float x1 = transform.position.x;
        audioPress.Play();
        for (float i = x1; i >= x1 - 0.0053f; i -= 0.0002f)
        {
            transform.position = new Vector3(i, transform.position.y, transform.position.z);
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(0.3f);
        audioRelease.Play();
        float x2 = transform.position.x;
        for (float i = x2; i <= x1; i += 0.0002f)
        {
            transform.position = new Vector3(i, transform.position.y, transform.position.z);
            yield return new WaitForSeconds(0.001f);
        }
    }

    public override void nullClassCase()
    {

    }


}
