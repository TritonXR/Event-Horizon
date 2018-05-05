using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRLaserShoot : MonoBehaviour {
	public GameObject ship;
	LaserShoot laser;
	public SteamVR_TrackedObject tracked = null;
	public SteamVR_Controller.Device dev;
	private Interaction interact = null;
	private bool grab;
	int count = 0;
	int fireRate = 20;
	// Use this for initialization
	void Awake () {
		tracked = GetComponent<SteamVR_TrackedObject>();
		interact = GetComponent<Interaction>();
	}
	// Use this for initialization
	void Start () {
		laser = ship.GetComponent<LaserShoot> ();
	}
	
	// Update is called once per frame
	void Update () {
		dev = SteamVR_Controller.Input((int)tracked.index);
		if (dev.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
		{
			Debug.Log ("Shoot");
			count++;
			if (count > fireRate)
			{
				laser.FireVR();
				count = 0;
			}
		}
	}
}
