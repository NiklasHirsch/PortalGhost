using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class LSLInput : MonoBehaviour
{
    public SelectedObject selectedObject;
    public string StreamType = "EMBody"; // "EMG RMS";
    public float scaleInput = 0.1f;

    StreamInfo[] streamInfos;
    StreamInlet streamInlet;

    string[] sample;
    private int channelCount = 0;

    void Start()
    {
        Debug.Log("start");
        streamInfos = LSL.LSL.resolve_streams(); //"name", StreamType, 1, 5.0


        if (streamInfos.Length > 0)
        {
            Debug.Log("found stream");
            streamInlet = new StreamInlet(streamInfos[0]);
            channelCount = streamInlet.info().channel_count();
            streamInlet.open_stream();
            Debug.Log("numerb of streams:" + streamInfos.Length  + "\n opened stream: " + streamInlet.info().name());
        }
    }

    void FixedUpdate()
    {
        if (streamInlet == null)
        {
            Debug.Log("stream not found");
        }


        if (streamInlet != null)
        {
            sample = new string[1];
            double lastTimeStamp = streamInlet.pull_sample(sample, 1.0f);
            
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
    void Process(string[] newSample, double timeStamp)
    {
        Debug.Log("Marker " + newSample[0]);
        if (selectedObject.selectedGameObject != null){

            InteractableObject activeObject = selectedObject.selectedGameObject.GetComponent<InteractableObject>();

            if(activeObject.getState()){
                switch(newSample[0]){
                    case "Push":
                        // code block
                        activeObject.push();
                        break;
                    case "Pull":
                        // code block
                        activeObject.pull();
                        break;
                    case "NULL_CLASS":
                    default:
                        // code block
                        break;
                }
            }
        }
        
    }

    void chargeUp()
    {


    }
}