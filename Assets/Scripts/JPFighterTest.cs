using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPFighterTest : MonoBehaviour {
    Rigidbody rb;
    public GameObject targetShip;
    public float speed = 2.0f;
    public float rotSpeed = 1.0f;
    public Quaternion toRotation;
    public Vector3 direction;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        //var rotation = Quaternion.LookRotation(targetShip.transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotSpeed);
        transform.LookAt(targetShip.transform);
        //Vector3 direction = targetShip.transform.position - transform.position;
        //Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
        //transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotSpeed * Time.time);


        rb.velocity = transform.forward * speed;

    }
    void FixedUpdate()
    {
        
    }
}
