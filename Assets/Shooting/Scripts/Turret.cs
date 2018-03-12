using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Turret : NetworkBehaviour {

	public Quaternion angle;
	public DRange range;
	public GameObject ship;
	public LaserShoot laser;
	public bool stationary = false;
    public float turnSpeed = 1.0f;
	int count = 0;

    Vector3 startVector;
    Quaternion startRotation;

    public int fireRate = 15;

	// Use this for initialization
	void Start () {
        range = transform.parent.GetComponent<DRange>();
		angle = transform.rotation;
		laser = GetComponentInChildren<LaserShoot>();
        startVector = transform.localRotation.eulerAngles;
        startRotation = transform.localRotation;
	}

	// Update is called once per frame
	void Update () {
       
		ship = range.ship;
        if(ship != null) {
            if(!stationary) {
                Vector3 targetDir = ship.transform.position - transform.position;
                float step = turnSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
                //Debug.DrawRay(transform.position, newDir, Color.red);
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        } else {
            if (!stationary)
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, startRotation, Time.deltaTime * turnSpeed);
            }
        }
		/*Quaternion current = transform.localRotation;
		if (ship != null && stationary == false)
		{
			Vector3 shipPos = ship.transform.position;
			Vector3 thePos = shipPos - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, thePos, Time.deltaTime, 0.0F);
            if (current != angle)
			    transform.localRotation = Quaternion.LookRotation(newDir);
		}
		else
		{
            Vector3 zero = new Vector3(0, 0, 0);
            //Vector3 zero = startVector;
            if(current != Quaternion.LookRotation(zero))
			    transform.localRotation = Quaternion.Slerp(current, Quaternion.LookRotation(zero), Time.deltaTime);
		}*/
		if (ship != null)
		{
			count++;
            if (count > fireRate)
			{
				laser.Fire();
				count = 0;
			}
		}
	}
}
