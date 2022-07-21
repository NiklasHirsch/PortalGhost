using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InteractableObject : MonoBehaviour
{
    //basic obj variables
    public bool isActive = false;
    public bool isInTheAir = false;
 
    [Header("Shake Settings")]
    public float minimalYitter = -0.05f;
    public float maximalYitter = 0.05f;


    [Header("Hover transition Settings")]
    public bool floatAtStart = false;
    [SerializeField]
    [Range(0,10)]
    private float hoverLevel = 1;
    [SerializeField]
    [Range(0,10)]
    private float secondTillStabilized = 1;

    [Header("Push/Pull Settings")]
    [SerializeField]
    [Range(0,4)]
    private float distance = 2f;

    public void Start(){
        if(floatAtStart){
            floatUp();
        }
    }

    public void Update(){
    }

    public bool getState() {
        return isActive;
    }

    public virtual void floatUp() {
        Debug.Log("float up");

        transform.GetComponent<Rigidbody>().isKinematic = true;

        StartCoroutine (MoveOverSeconds (gameObject, new Vector3(transform.position.x, (transform.position.y + hoverLevel), transform.position.z), secondTillStabilized, doAfterFloat));
    }

    public virtual void doAfterFloat(){
         Debug.Log("After");
         isInTheAir = true;
         isActive = true;
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
        transform.GetComponent<Rigidbody>().isKinematic = false;
        isInTheAir = false;
        isActive = false;
    }


    public virtual void pull() {
        if(isActive){
            Debug.Log(gameObject.name + " pulled");
            transform.position = transform.position + Camera.main.transform.forward * distance * Time.deltaTime * -1;
        }
        
    }

    public virtual void push() {
        if(isActive){
            Debug.Log(gameObject.name + " pushed");
            transform.position = transform.position + Camera.main.transform.forward * distance * Time.deltaTime;
        }
    }
}