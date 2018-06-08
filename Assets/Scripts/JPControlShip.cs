using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;

public class JPControlShip : MonoBehaviour {
    public GameObject controlObj;
    public bool controlActive = false;
	public GameObject gun;
	LaserShoot laser;
	int count = 0;
	int fireRate = 100;
    Rigidbody rb;
    public string shipName;

	public SteamVR_TrackedObject tracked = null;
	public SteamVR_Controller.Device dev;
	private Interaction interact = null;
	private bool grab;
	// Use this for initialization
	void Awake () {
		tracked = GetComponent<SteamVR_TrackedObject>();
		interact = GetComponent<Interaction>();
	}
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (controlActive)
        {
			dev = SteamVR_Controller.Input((int)tracked.index);
			if (dev.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
			{
				Debug.Log ("Shoot");
				count++;
				if (count > fireRate)
				{
					laser.FireVR();
					count = 0;
				}
			}
            //if (Input.GetKeyDown("w"))
            //{
                //transform.position = new Vector3(10, 0, 10);
            //}

            //rb.AddForce(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        }

	}
    public void Activate () {
        controlActive = true;
        print("Grabbed single player controller");
        //Camera.main.enabled = false;
        //transform.GetChild(0).gameObject.SetActive(true);
    }
}
