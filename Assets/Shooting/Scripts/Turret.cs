using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

	public Quaternion angle;
	public DRange range;
	public GameObject ship;
	public LaserShoot laser;
	public bool stationary = false;
	int count = 0;

    Vector3 startVector;

	// Use this for initialization
	void Start () {
		range = GetComponentInChildren<DRange>();
		angle = transform.rotation;
		laser = GetComponentInChildren<LaserShoot>();
        startVector = transform.localRotation.eulerAngles;
	}

	// Update is called once per frame
	void Update () {
		ship = range.ship;
		Quaternion current = transform.localRotation;
		if (ship != null && stationary == false)
		{
			Vector3 shipPos = ship.transform.position;
			Vector3 thePos = shipPos - transform.position;
			angle = Quaternion.LookRotation(thePos);
			transform.localRotation = Quaternion.Slerp(current, angle, Time.deltaTime);
		}
		else
		{
            Vector3 zero = new Vector3(0, 0, 0);
            //Vector3 zero = startVector;
			transform.localRotation = Quaternion.Slerp(current, Quaternion.LookRotation(zero), Time.deltaTime);
		}
		if (ship != null)
		{
			count++;
			if (count > 100)
			{
				laser.Fire();
				count = 0;
			}
		}
	}
}
