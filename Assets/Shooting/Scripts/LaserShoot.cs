using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LaserShoot : NetworkBehaviour {
	
	public GameObject laser;
	float laserSpeed = 30000;

	// Use this for initialization
	void Start()
	{

	}

	// Fires a laser
	public void Fire()
	{
		GameObject tempLaser = (GameObject)Instantiate(laser, transform.position, transform.parent.rotation * Quaternion.Euler(90, 0, 0));
		Rigidbody tempLaserRigidbody = tempLaser.GetComponent<Rigidbody>();
		tempLaserRigidbody.AddForce(tempLaserRigidbody.transform.up * laserSpeed);
		//Debug.Log ("REG FIRE");
		Destroy(tempLaser, 3f);
        //NetworkServer.Spawn(tempLaser);
	}
	public void FireVR()
	{
		GameObject tempLaser = (GameObject)Instantiate(laser, transform.position, transform.parent.rotation * Quaternion.Euler(90,0, 0));
		Rigidbody tempLaserRigidbody = tempLaser.GetComponent<Rigidbody>();
        tempLaserRigidbody.AddForce(tempLaserRigidbody.transform.up * laserSpeed);
        //Debug.Log ("VR FIRE");
        Destroy(tempLaser, 3f);
        NetworkServer.Spawn(tempLaser);
		Destroy(tempLaser, 3f);
		//NetworkServer.Spawn(tempLaser);
	}

	//Update is called once per frame
	void Update()
	{

	}
}
