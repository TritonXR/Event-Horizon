using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaRange : MonoBehaviour {
    public GameObject targetShip;
    public string targetName;
    // Use this for initialization
    void Start () {
        targetName = "testship";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider collision)
    {

        if (collision.gameObject.name == targetName)
        {
            targetShip = collision.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == targetShip)
        {
            targetShip = null;
        }
    }

    public void setTargetName(string name)
    {
        targetName = name;
    }
}
