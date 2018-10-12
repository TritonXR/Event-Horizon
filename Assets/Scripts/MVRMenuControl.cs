using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MVRMenuControl : MonoBehaviour {
    public JPUINetworkManager nManager;
    // Use this for initialization
    void Start () {
        nManager.IPChange("192.168.1.3");
	}
	
	// Update is called once per frame
	void Update () {
        OVRInput.Controller activeController = OVRInput.GetActiveController();


        if (OVRInput.Get(OVRInput.Button.Any))
        {
            nManager.IPChange("192.168.1.3");
            nManager.Client();
        }
        if(Input.GetKeyDown("w")) {
            nManager.IPChange("192.168.1.3");
            nManager.Client();
        }
    }
}
