using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class setDisplayNumbers : MonoBehaviour
{
    public TMP_Text displayText;
    public TMP_Text finishText;
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;
    public GameObject button5;
    public GameObject button6;
    public GameObject button7;
    public GameObject button8;
    public GameObject button9;
    public GameObject deleteButton;
    private String numbers;
    // Start is called before the first frame update
    void Start()
    {
        numbers = "";
    }

    // Update is called once per frame
    void Update()
    {
        displayText.text = numbers;
    }

    private void checkCode()
    {
        if(numbers == "327614")//327614
        {
            displayText.color = new Color(0, 200, 0, 255);
            finishText.text = "You did it!";
        } 
        else
        {
            displayText.color = new Color(200, 0, 0, 255);
            GameObject.Find("LampCeilingMain").GetComponent<FlickeringLightController>().StartFlicker();
        }
    }

    public void addNumber(GameObject button)
    {
        if(numbers.Length < 6)
        {
            displayText.color = new Color(255, 255, 255, 255);
            switch(button.tag)
            {
                case ("ElevatorButton1"):
                    numbers = numbers + "1";
                    break;
                case ("ElevatorButton2"):
                    numbers = numbers + "2";
                    break;
                case ("ElevatorButton3"):
                    numbers = numbers + "3";
                    break;
                case ("ElevatorButton4"):
                    numbers = numbers + "4";
                    break;
                case ("ElevatorButton5"):
                    numbers = numbers + "5";
                    break;
                case ("ElevatorButton6"):
                    numbers = numbers + "6";
                    break;
                case ("ElevatorButton7"):
                    numbers = numbers + "7";
                    break;
                case ("ElevatorButton8"):
                    numbers = numbers + "8";
                    break;
                case ("ElevatorButton9"):
                    numbers = numbers + "9";
                    break;
            }
        }
        if(numbers.Length > 0)
        {
            if(button.tag == "ElevatorDeleteButton")
            {
                numbers = numbers.Substring(0, numbers.Length - 1);
            }

        }
        if(numbers.Length == 6){
            checkCode();
        }
            
    }
}

