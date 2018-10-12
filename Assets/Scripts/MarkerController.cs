using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerController : MonoBehaviour {
    public GameObject rotationMarker;
	// Use this for initialization
	void Start () {
        rotationMarker.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setMode (int mode) {
        if(mode == 0) {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(false);
        } else if(mode == 1) {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    public void setRotationMode (bool mode) {
        rotationMarker.SetActive(mode);
        if (mode == false)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }

    } 
}
