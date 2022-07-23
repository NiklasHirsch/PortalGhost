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

        if (elevatorObj != null) {

            elevatorObj.GetComponent<setDisplayNumbers>().addNumber(this.gameObject);
        }
    }

    public override void nullClassCase()
    {

    }


}
