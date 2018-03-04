using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JPPilot : NetworkBehaviour {
    GameObject controller;
    bool active = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(active) {
            transform.position = controller.transform.position;
            transform.rotation = controller.transform.rotation;
            CmdUpdatePosition(controller.transform.position, controller.transform.rotation);
        }
    }
    public void activate(GameObject obj) {
        controller = obj;
        active = true;
    }
    [Command]
    void CmdUpdatePosition (Vector3 pos, Quaternion rot) {
        print("CmdUpdatPositionCalled");
        transform.position = pos;
        transform.rotation = rot;
    }
}
