using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JPPilot : NetworkBehaviour {
    GameObject controller;
    bool active = false;
    public GameObject laser;
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
            controller.GetComponent<JPControlShip>().shipName = this.gameObject.name;
            CmdUpdatePosition(controller.transform.position, controller.transform.rotation);
        }
    }
    public void activate(GameObject obj) {
        print("Activate called");
        controller = obj;
        active = true;
        transform.Find("default").gameObject.GetComponent<Renderer>().enabled = false;
    }
    [Command]
    void CmdUpdatePosition (Vector3 pos, Quaternion rot) {
        print("CmdUpdatPositionCalled");
        transform.position = pos;
        transform.rotation = rot;
    }

    public void ShootVRLaser()
    {
        laser.GetComponent<LaserShoot>().FireVR();
    }
}
