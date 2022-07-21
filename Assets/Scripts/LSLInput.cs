using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class LSLInput : MonoBehaviour
{
    public SelectedObject selectedObject;
    public string StreamType = "Markers"; // "EMG RMS";
    public float scaleInput = 0.1f;

    StreamInfo[] streamInfos;
    StreamInlet streamInlet;

    string[] sample;
    private int channelCount = 0;

    void Start()
    {
        Debug.Log("start");
        //streamInfos = LSL.LSL.resolve_streams(); //"name", StreamType, 1, 5.0
        streamInfos = LSL.LSL.resolve_stream("type", StreamType, 1, 0.01);

        if (streamInfos.Length > 0)
        {
            Debug.Log("found stream");
            streamInlet = new StreamInlet(streamInfos[0]);
            channelCount = streamInlet.info().channel_count();
            streamInlet.open_stream();
            Debug.Log("numerb of streams:" + streamInfos.Length  + "\n opened stream: " + streamInlet.info().name());
        }
    }

    void Update()
    {
        if (streamInlet == null)
        {
            Debug.Log("stream not found");
        }

        if (streamInlet != null)
        {
            sample = new string[1];
            //sample = new float[channelCount];
            double lastTimeStamp = streamInlet.pull_sample(sample, 0.01f);
            
            if (lastTimeStamp != 0.0)
            {
                Process(sample, lastTimeStamp);
                while ((lastTimeStamp = streamInlet.pull_sample(sample, 0.0f)) != 0)
                {
                    Debug.Log("sending");
                    Process(sample, lastTimeStamp);
                }
            }
            
        }
    }
    
    void Process2(float[] newSample, double timeStamp) {

    }

    void Process(string[] newSample, double timeStamp)
    {

         Debug.Log(selectedObject.selectedGameObject);
        if (selectedObject.selectedGameObject != null){
           
            InteractableObject activeObject = selectedObject.selectedGameObject.GetComponent<InteractableObject>();
             Debug.Log("Test " + activeObject.getState());

            if(activeObject.getState()){
                switch(newSample[0]){
                    case "Push":
                        Debug.Log("Marker Push");
                        activeObject.push();
                        break;
                    case "Pull":
                        Debug.Log("Marker Pull");
                        activeObject.pull();
                        break;
                    case "NULL_CLASS":
                    default:
                        Debug.Log("Marker Pull");
                        break;
                }
            }
        }
        
    }

    void chargeUp()
    {


    }
}