using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InteractableObject : MonoBehaviour
{
    //basic obj variables
    public bool isActive = false;
    public bool isInfloatingStart = false;
    public bool isSelectedThroughPortal = false;
 
    [Header("Shake Settings")]
    public float minimalYitter = -0.01f;
    public float maximalYitter = 0.01f;


    [Header("Hover transition Settings")]
    public bool floatAtStart = false;
    [SerializeField]
    [Range(0,2)]
    private float hoverLevel = 0.5f;
    [SerializeField]
    [Range(0,2)]
    private float secondTillStabilized = 0.2f;

    [Header("Push/Pull Settings")]
    [SerializeField]
    [Range(0,10)]
    private float forceDistance = 2f;

    private float minDistanceObjToHuman = 0.8f;

    [SerializeField]
    private float _durationPerUnitMoved = 0.1f;
    private bool _pullObjectRuntineRunning = false;

    private Vector3 posAfterFloat;
    private float distanceToObj;
    private Vector3 normalizedCameraViewVector;
    private Vector3 cameraCenter;

    private float velocity = 0f;


    [Header("Spring Settings")]
    private GameObject springJointObj;
    private SpringJoint springJoint;
    private float halfHeightOfObj;
    [SerializeField]
    private int standarSpringVal = 4000;

    // [0] = telekinesis; [1] = pull; [2] = push;
    AudioSource[] audioSources;

    public void Start(){

        audioSources = Camera.main.GetComponents<AudioSource>();
        if (floatAtStart){
            floatUp();
        }
    }

    public void FixedUpdate(){
        if (isActive && springJointObj != null) {
            // Central point of camera
            cameraCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));
            normalizedCameraViewVector = Camera.main.transform.forward.normalized;

            
            if (velocity < 0.0f)
            {
                Debug.Log(velocity);
                if (distanceToObj > minDistanceObjToHuman) {
                    Debug.Log("Distance: " + distanceToObj);
                    distanceToObj += velocity;
                }
            }
            else
            {
                distanceToObj += velocity;
            }
            

            //transform.position = cameraCenter + normalizedCameraViewVector * distanceToObj;

            springJointObj.transform.position = cameraCenter + new Vector3(0, halfHeightOfObj, 0) + normalizedCameraViewVector * distanceToObj;
        }
    }

    public bool getState() {
        return isActive;
    }

    public virtual void floatUp() {
        //Debug.Log("float up");
        isInfloatingStart = true;

        if (!isActive) {
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(MoveOverSeconds(gameObject, new Vector3(transform.position.x, (transform.position.y + hoverLevel), transform.position.z), secondTillStabilized, doAfterFloat));
        }
    }

    public virtual void doAfterFloat(){
        //Debug.Log("After");
        isActive = true;
        if (isSelectedThroughPortal)
        {
            string inputPortalName = "HumanPortal";

            GameObject inputPortal = GameObject.Find(inputPortalName);
            gameObject.transform.position = inputPortal.transform.position + inputPortal.transform.forward * (-1.5f);
        }

        // handle audio
        muteAudioSources();
        audioSources[0].mute = false;

        springJointSettings();

        // Pos of Object after floating
        //posAfterFloat = transform.position;

        // Central point of camera
        //cameraCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));

        // Distance from Camera center to the object
        //distanceToObj = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane)), posAfterFloat);

        Debug.Log("DistToObj: " + distanceToObj);
    }

    private void resetSpringJointSettings()
    {
        if (springJoint != null)
        {
            //Debug.Log("resetSpringJoint");
            Destroy(springJoint);
        }
    }

    private void springJointSettings()
    {
        springJointObj = GameObject.FindGameObjectWithTag("SpringJoint");

        if (springJointObj != null)
        {
            //halfHeightOfObj = gameObject.GetComponent<MeshFilter>().mesh.bounds.extents.z / 2;
            //springJointObj.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + halfHeightOfObj);

            if (gameObject.GetComponent<MeshFilter>())
            {
                halfHeightOfObj = gameObject.GetComponent<MeshFilter>().mesh.bounds.extents.y / 2;
            }
            else
            {
                halfHeightOfObj = 0.5f;
            }

            halfHeightOfObj = gameObject.GetComponent<MeshFilter>().mesh.bounds.extents.y / 2;
            springJointObj.transform.position = new Vector3(transform.position.x, transform.position.y + halfHeightOfObj, transform.position.z);

            // Pos of Object after floating
            posAfterFloat = transform.position;

            // Central point of camera
            cameraCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));

            // Distance from Camera center to the object
            distanceToObj = Vector3.Distance(cameraCenter, posAfterFloat);


            addNewspringJoint();
            
        }
    }

    private void addNewspringJoint()
    {
        if (springJointObj.GetComponent<SpringJoint>() != null)
        {
            return;
        }
        //Debug.Log("addSpringJoint");
        springJointObj.AddComponent<SpringJoint>();

        springJoint = springJointObj.GetComponent<SpringJoint>();

        springJoint.connectedBody = gameObject.GetComponent<Rigidbody>();
        springJoint.spring = 10000;
        springJoint.anchor = new Vector3(0, 0.5f, 0);
        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.damper = 500;
        springJoint.minDistance = 0;
        springJoint.maxDistance = 0;
        springJoint.tolerance = 0.025f;
        springJoint.enableCollision = false;
        springJoint.enablePreprocessing = true;
        springJoint.massScale = 1;
        springJoint.connectedMassScale = 1;
    }


    // Moves an Object with a Vetor over a time span seconds
    public IEnumerator MoveOverSeconds (GameObject objectToMove, Vector3 end, float seconds, Action action)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            //Shake
            //objectToMove.transform.position.x = Mathf.Sin(Time.time * speed) * amount;
            Vector3 nextPos = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            nextPos = new Vector3(nextPos.x + Random.Range(minimalYitter, maximalYitter), nextPos.y, nextPos.z + Random.Range(minimalYitter, maximalYitter));
            objectToMove.transform.position = nextPos;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
        doAfterFloat();
    }

    public virtual void fallDown() {
        //Debug.Log("fall down");
        StopAllCoroutines();

        // handle audio
        muteAudioSources();
        resetSpringJointSettings();

        //transform.GetComponent<Rigidbody>().isKinematic = false;
        isActive = false;
        isInfloatingStart = false;
    }

    public virtual void pull() {
        Debug.Log("pull");
        if(isActive){
            //distanceToObj = distanceToObj - 0.1f * forceDistance;

            // handle audio
            muteAudioSources();
            audioSources[2].mute = false;

            velocity = -0.005f * forceDistance;

            //float newDistanceToObj = distanceToObj - 0.1f * forceDistance;
            //StartCoroutine(distanceChange(newDistanceToObj));

            //transform.position = transform.position + Camera.main.transform.forward * forceDistance * Time.deltaTime * -1;
        }
        
    }

    public virtual void push() {
        Debug.Log("push");
        if (isActive){
            //distanceToObj = distanceToObj + 0.1f * forceDistance;

            // handle audio
            muteAudioSources();
            audioSources[1].mute = false;

            velocity = 0.005f * forceDistance;
            //float newDistanceToObj = distanceToObj + 0.1f * forceDistance;
            //StartCoroutine(distanceChange(newDistanceToObj));

            //transform.position = transform.position + Camera.main.transform.forward * forceDistance * Time.deltaTime;
        }
    }

    public virtual void nullClassCase() {
        Debug.Log("null_class");
        if (isActive)
        {
            // handle audio
            muteAudioSources();
            audioSources[0].mute = false;

            velocity = 0f;
        }
    }

    private void muteAudioSources()
    {
        foreach (AudioSource audiSource in audioSources)
        {
            audiSource.mute = true;
        }
    }

    /*
     * Lerp approach
     * in Update
    Vector3 goalLocation = cameraCenter + normalizedCameraViewVector * distanceToObj;
    if (!_pullObjectRuntineRunning)
    {
        StartCoroutine(distanceChange(goalLocation));
    }*/
    public IEnumerator distanceChange(Vector3 goalLocation)
    {
        _pullObjectRuntineRunning = true;
        float time = 0;
        float duration = _durationPerUnitMoved * Vector3.Distance(transform.position, goalLocation);

        Vector3 startPosition = transform.position;

        while (time < duration) {

            transform.position = Vector3.Lerp(startPosition, goalLocation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        _pullObjectRuntineRunning = false;
    }
}