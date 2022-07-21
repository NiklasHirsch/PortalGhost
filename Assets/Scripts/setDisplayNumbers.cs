using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class setDisplayNumbers : MonoBehaviour
{
    public TMP_Text displayText;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        displayText.text = DateTime.Now.ToString();
    }
}
