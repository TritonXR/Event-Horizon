using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;

public class JPControlShip : MonoBehaviour {
    public GameObject controlObj;
    public bool controlActive = false;
    Rigidbody rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (controlActive)
        {
            //if (Input.GetKeyDown("w"))
            //{
                //transform.position = new Vector3(10, 0, 10);
            //}

            rb.AddForce(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        }

	}
    public void Activate () {
        controlActive = true;
        print("Grabbed single player controller");
        //Camera.main.enabled = false;
        //transform.GetChild(0).gameObject.SetActive(true);
    }
}
