using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    //basic obj variables
    private bool isActive = false;

    // shake variables
    [Header("Shake Info")]
    private Vector3 _startPos;
    private float _timer;
    private Vector3 _randomPos;
 
    [Header("Shake Settings")]
    [Range(0f, 2f)]
    public float _time = 0.2f;
    [Range(0f, 2f)]
    public float _distance = 0.1f;
    [Range(0f, 0.1f)]
    public float _delayBetweenShakes = 0f;

    [Header("Hover transition Settings")]
    Vector3 startPosition;
    [SerializeField]
    [Range(1,10)]
    private float hoverLevel = 1;
    [SerializeField]
    [Range(1,100)]
    private float hoverFloatValue = 10;
    private RaycastHit collectorHit;
 
    private bool collectorOnGround = true;
    private float speed = .12f;
    private float step;

    private void Awake()
    {
        _startPos = transform.position;
    }

    public void Start(){
        //floatUp();
    }

    public bool getState() {
        return isActive;
    }

    public virtual void floatUp() {
        Debug.Log("float up");

        transform.GetComponent<Rigidbody>().isKinematic = true;
        step = speed * Time.deltaTime;

        //shaking
        //Begin();

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, (transform.position.y + hoverLevel), transform.position.z), step);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out collectorHit, 5f)) {
            transform.rotation = Quaternion.FromToRotation(Vector3.forward, collectorHit.normal);
        }
        if (transform.position.y >= (transform.position.y + hoverLevel)) {
            collectorOnGround = false;
        }
    }

    public virtual void fallDown() {
        Debug.Log("fall down");

         /*if (GP.getSetTimeOfDayManager.Hour > 8 && GP.getSetTimeOfDayManager.Hour < 17) {
                if (transform.GetComponent<Rigidbody>().isKinematic == false) {
                    transform.GetComponent<Rigidbody>().isKinematic = true;
                }
                float x = transform.position.x;
                float y = Mathf.Sin(Time.timeSinceLevelLoad * 1.5f) / hoverFloatValue;
                float z = transform.position.z;
                transform.position = new Vector3(x, (transform.position.y + y) + hoverLevel, z);
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out collectorHit, 5f)) {
                    transform.rotation = Quaternion.FromToRotation(Vector3.forward, collectorHit.normal);
                }
        } else {
            transform.GetComponent<Rigidbody>().isKinematic = false;
        }*/
    }

    /* 
    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.name == "Terrain") {
            collectorOnGround = true;
        }
    }
    */

    //-------------------------- Shake -----------------------------
 
   private void OnValidate()
   {
       if (_delayBetweenShakes > _time)
           _delayBetweenShakes = _time;
   }
 
   public void Begin()
   {
       StopAllCoroutines();
       StartCoroutine(Shake());
   }
 
   private IEnumerator Shake()
   {
       _timer = 0f;
 
       while (_timer < _time)
       {
           _timer += Time.deltaTime;
 
           _randomPos = _startPos + (Random.insideUnitSphere * _distance);
 
           transform.position = _randomPos;
 
           if (_delayBetweenShakes > 0f)
           {
               yield return new WaitForSeconds(_delayBetweenShakes);
           }
           else
           {
               yield return null;
           }
       }
 
       transform.position = _startPos;
    }

    //-------------------------- Shake End -----------------------------


    public virtual void pull() {
        Debug.Log(gameObject.name + " pulled");
    }

    public virtual void push() {
        Debug.Log(gameObject.name + " pushed");
    }
}