using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stickGrab : MonoBehaviour {
	Vector3 initPos;
	Transform ship;
	public bool throttle;
	float rotationsPerMinute = 1.0f;
	public bool used;
	Rigidbody rb;
	// Use this for initialization
	void Start () {
		initPos = transform.localPosition;
		ship = transform.parent.parent;
		used = false;
		rb = ship.GetComponent<Rigidbody> ();
	}

	public Vector3 getInitPos() {
		return initPos;
	}

	public void Picked() {
		used = true;
	} 

    public void Dropped()
    {
		used = false;
        transform.localPosition = initPos;
		Debug.Log ("dropped");
    }
	
	// Update is called once per frame
	void Update () {
		if (used == true) {
			Vector3 localPos = transform.localPosition;
			float xDistance = localPos.x - initPos.x;
			float yDistance = localPos.y - initPos.y;
			float zDistance = localPos.z - initPos.z;
			if (throttle == false) {
				Vector3 newVec = new Vector3 (1.0f * zDistance, 0.25f * xDistance, -1.0f * zDistance);
				ship.Rotate (newVec * 6.0f * rotationsPerMinute * Time.deltaTime);
			} else {
				//ship.Translate (-1 * ship.forward * Time.deltaTime * zDistance * 10);
				if (rb.velocity.magnitude >= 10.0f) {
					rb.velocity = ship.right * zDistance * 10;
				} else {
					rb.AddForce(ship.right * zDistance * 10);
				}
				//Rigidbody shipRB = ship.gameObject.GetComponent<Rigidbody> ();
				//shipRB.AddForce(transform.forward * 10 * zDistance);
				/*if(shipRB.velocity.magnitude > 20f && shipRB.velocity.magnitude > 0) {
				shipRB.velocity = transform.forward * 20f;
			}
			if(shipRB.velocity.magnitude < -20f && shipRB.velocity.magnitude < 0) {
				shipRB.velocity = transform.forward * -20f;
			}*/
			}
		} else {
			transform.localPosition = initPos;
		}
	}
}