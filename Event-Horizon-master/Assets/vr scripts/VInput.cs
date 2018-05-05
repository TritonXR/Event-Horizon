using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VInput : MonoBehaviour {
    public SteamVR_TrackedObject tracked = null;
    public SteamVR_Controller.Device dev;
    private Interaction interact = null;
	private bool grab;
	// Use this for initialization
	void Awake () {
        tracked = GetComponent<SteamVR_TrackedObject>();
		interact = GetComponent<Interaction>();
	}

	void Start() {
	}

	// Update is called once per frame
	void Update () {
        dev = SteamVR_Controller.Input((int)tracked.index);

		#region Trigger
        if (dev.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
			grab = true;
			interact.Pick();
        }
        if (dev.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
			grab = false;
			//Debug.Log ("zzz");
            interact.Drop(dev);
        }
		if(grab == true) {
			//interact.rigbod.transform.position = interact.transform.position;
			interact.joint.connectedBody = interact.rigbod;
			interact.rigbod.velocity = Vector3.zero;
			interact.rigbod.angularVelocity = Vector3.zero;
			return;
		}
		#endregion

    }
}