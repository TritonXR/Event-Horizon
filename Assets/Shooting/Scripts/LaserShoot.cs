using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;

public class LaserShoot : MonoBehaviour {
	
	public GameObject laser;
	float laserSpeed = 30000;
    public GameObject slaveGun;
    LaserShoot slaveLaserShoot;
    bool slaveGunExists = false;
    public int teamNum;
	// Use this for initialization
	void Start()
	{
        if(slaveGun) {
            slaveGunExists = true;
        }
        if(slaveGunExists) {
            slaveLaserShoot = slaveGun.GetComponent<LaserShoot>();
        }
	}

	// Fires a laser
	public void Fire()
	{

		GameObject tempLaser = (GameObject)Instantiate(laser, transform.position, transform.parent.rotation * Quaternion.Euler(90, 0, 0));
		Rigidbody tempLaserRigidbody = tempLaser.GetComponent<Rigidbody>();
		tempLaserRigidbody.AddForce(tempLaserRigidbody.transform.up * laserSpeed);

        tempLaser.GetComponent<Projectile>().teamNum = teamNum;
		//Debug.Log ("REG FIRE");
		Destroy(tempLaser, 3f);
        //NetworkServer.Spawn(tempLaser);
	}
	public void FireVR()
	{
        //transform.localScale = transform.localScale * 2.1f;
		GameObject tempLaser = (GameObject)Instantiate(laser, transform.position, transform.parent.rotation * Quaternion.Euler(90, 0, 0));
		Rigidbody tempLaserRigidbody = tempLaser.GetComponent<Rigidbody>();
        tempLaserRigidbody.AddForce(tempLaserRigidbody.transform.up * laserSpeed);
        //Debug.Log ("VR FIRE");
        Destroy(tempLaser, 3f);
        //NetworkServer.Spawn(tempLaser);
		Destroy(tempLaser, 3f);
		//NetworkServer.Spawn(tempLaser);
	}

	//Update is called once per frame
	void Update()
	{

	}
}
