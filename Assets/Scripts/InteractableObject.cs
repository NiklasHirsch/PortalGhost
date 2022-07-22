using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InteractableObject : MonoBehaviour
{
    //basic obj variables
    public bool isActive = false;
 
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

    [SerializeField]
    private float _durationPerUnitMoved = 0.1f;
    private bool _pullObjectRuntineRunning = false;

    private Vector3 posAfterFloat;
    private float distanceToObj;
    private Vector3 normalizedCameraViewVector;
    private Vector3 cameraCenter;

    public void Start(){
        if (floatAtStart){
            floatUp();
        }
    }

    public void FixedUpdate(){
        if (isActive) {
            // Central point of camera
            cameraCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));
            normalizedCameraViewVector = Camera.main.transform.forward.normalized;

            distanceToObj += velocity;
            transform.position = cameraCenter + normalizedCameraViewVector * distanceToObj;
            
            /*
             * Lerp approach
            Vector3 goalLocation = cameraCenter + normalizedCameraViewVector * distanceToObj;
            if (!_pullObjectRuntineRunning)
            {
                StartCoroutine(distanceChange(goalLocation));
            }*/
        }
    }

    public bool getState() {
        return isActive;
    }

    public virtual void floatUp() {
        Debug.Log("float up");

        if (!isActive) {
            transform.GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(MoveOverSeconds(gameObject, new Vector3(transform.position.x, (transform.position.y + hoverLevel), transform.position.z), secondTillStabilized, doAfterFloat));
        }
    }

    public virtual void doAfterFloat(){
        Debug.Log("After");
        isActive = true;

        // springJointSettings();

        // Pos of Object after floating
        posAfterFloat = transform.position;

        // Central point of camera
        cameraCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));

        // Distance from Camera center to the object
        distanceToObj = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane)), posAfterFloat);

        Debug.Log("DistToObj: " + distanceToObj);
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
        Debug.Log("fall down");
        StopAllCoroutines();
        transform.GetComponent<Rigidbody>().isKinematic = false;
        isActive = false;
    }

    float velocity = 0f;
    public virtual void pull() {
        Debug.Log("pull");
        if(isActive){
            //distanceToObj = distanceToObj - 0.1f * forceDistance;

            velocity = -0.1f * forceDistance * 0.03f;

            //float newDistanceToObj = distanceToObj - 0.1f * forceDistance;
            //StartCoroutine(distanceChange(newDistanceToObj));

            //transform.position = transform.position + Camera.main.transform.forward * forceDistance * Time.deltaTime * -1;
        }
        
    }

    public virtual void push() {
        Debug.Log("push");
        if (isActive){
            //distanceToObj = distanceToObj + 0.1f * forceDistance;

            velocity = 0.1f * forceDistance * 0.03f;
            //float newDistanceToObj = distanceToObj + 0.1f * forceDistance;
            //StartCoroutine(distanceChange(newDistanceToObj));

            //transform.position = transform.position + Camera.main.transform.forward * forceDistance * Time.deltaTime;
        }
    }

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

    private void springJointSetting() {
        GameObject springJointObj = GameObject.FindGameObjectWithTag("SpringJoint");
        SpringJoint springJoint = springJointObj.GetComponent<SpringJoint>();
        // TODO springJoint.connectedBody = ;
    }

}