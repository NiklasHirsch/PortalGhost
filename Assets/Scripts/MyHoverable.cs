using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHoverable: MonoBehaviour {
    
    //[SerializeField]
    //private Terrain terrain;
    Vector3 startPosition;
    float terrainY;
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

    private bool objectIsSelected = false;
 
    void Start () {
        //GP = GameObject.Find("GlobalPlayer").GetComponent<GlobalPlayer>();
        startPosition = transform.position;
    }
 
    
    void Update() {
        
        //terrainY = terrain.SampleHeight(new Vector3(transform.position.x, 0, transform.position.z));
        if (collectorOnGround && objectIsSelected) {
            transform.GetComponent<Rigidbody>().isKinematic = true;
            step = speed * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, (terrainY + hoverLevel), transform.position.z), step);
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out collectorHit, 5f)) {
                transform.rotation = Quaternion.FromToRotation(Vector3.forward, collectorHit.normal);
            }
            if (transform.position.y >= (terrainY + hoverLevel)) {
                collectorOnGround = false;
            }
        } else {
            if (objectIsSelected) {
                if (transform.GetComponent<Rigidbody>().isKinematic == false) {
                    transform.GetComponent<Rigidbody>().isKinematic = true;
                }
                //terrainY = terrain.SampleHeight(new Vector3(transform.position.x, 0, transform.position.z));
                float x = transform.position.x;
                float y = Mathf.Sin(Time.timeSinceLevelLoad * 1.5f) / hoverFloatValue;
                float z = transform.position.z;
                //transform.position = new Vector3(x, (terrainY + y) + hoverLevel, z);
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out collectorHit, 5f)) {
                    transform.rotation = Quaternion.FromToRotation(Vector3.forward, collectorHit.normal);
                }
            } else {
                transform.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        
    }
 
    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.name == "Terrain") {
            collectorOnGround = true;
        }
    }
 
    
}