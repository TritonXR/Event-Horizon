using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stickGrab : MonoBehaviour {
	Vector3 initPos;
	Transform ship;
	public bool throttle; 	// If throttle is checked, then only moves forward and backward. 
							// Else, 360 rotation
	float rotationsPerMinute = 4.0f;
	float xMax = 10.0f;
	float zMax = 10.0f;

	// WHY IS THIS PUBLIC AND WHY IS IT BUGGY WHEN ITS PRIVATE
	public bool used; // True, invis ball is being interacted/grabbed.
	Rigidbody rb;
	// Use this for initialization
	void Start () {
		initPos = transform.localPosition; // When values are changed, need this to visually see ship moving.
		ship = transform.parent.parent;
		used = false;
		rb = ship.GetComponent<Rigidbody> ();
	}

	public Vector3 getInitPos() {
		return initPos;
	}

	/*
	 * Called when ball is interacted with.
	 *
	 */
	public void Picked() {
		used = true;
	} 

	/*
	 * Called when ball is let go.
	 */
    public void Dropped()
    {
		used = false;
        transform.localPosition = initPos; 	// Once you drop ball, position of ship 
											//should be where you are.
		Debug.Log ("dropped");
    }
	
	// Update is called once per frame
	void Update () {
		// Movement rate and rotation
		if (used == true) {
			Vector3 localPos = transform.localPosition;
			/* Difference between current localPosition and initial localPosition */
			float xDistance = localPos.x - initPos.x;
			float yDistance = localPos.y - initPos.y;
			float zDistance = localPos.z - initPos.z;

			/* Debug */
			/*print ("xDistance: " + xDistance + ". yDistance: " + yDistance + 
				", zDistance: " + zDistance);*/

			/* Throttle is checked off: 360 rotation instead of forward and backward */
			if (throttle == false) {
				// Seperate movement and rotation to respective pair of quadrants 
				float ratioXtoZ = localPos.x / localPos.z;
				//Debug.Log ("RatioXToZ: " + ratioXtoZ);
				Debug.Log("localPos.x: " + localPos.x + ", localPos.z: " + localPos.z);

				// Top quadrant: Dive down 
				if ( localPos.x > 0 /*&& localPos.x <= xMax*/ ) {
					
					// Left half 
					if (localPos.z > 0 && ratioXtoZ > 1) {
						// Rotate around z axis
						Vector3 newVec = new Vector3 (0, 0, -1.0f * zDistance);
						ship.Rotate (newVec * 6.0f * rotationsPerMinute * Time.deltaTime);
					}

					// Right half 
					else if (localPos.z < 0 && ratioXtoZ < -1) {
						Vector3 newVec = new Vector3 (0, 0, -1.0f * zDistance);
						ship.Rotate (newVec * 6.0f * rotationsPerMinute * Time.deltaTime);
					}
				}
				/*
				// Bottom quadrant: Dive up
				if ( localPos.x < 0 && localPos.x >= xMax ) {
					// Left half
					if (localPos.z > 0 && ratioXtoZ > -1) {
						Vector3 newVec = new Vector3 (0, 0, 1.0f * zDistance);
						ship.Rotate (newVec * 6.0f * rotationsPerMinute * Time.deltaTime);
					}
					// Right half 
					else if (localPos.z < 0 && ratioXtoZ < 1) {
						Vector3 newVec = new Vector3 (0, 0, 1.0f * zDistance);
						ship.Rotate (newVec * 6.0f * rotationsPerMinute * Time.deltaTime);
					}
				}*/

				// Movement 
				//Vector3 newVec = new Vector3 (
				//	1.0f * xDistance, 1.0f * yDistance, 1.0f * zDistance);

				// Rotation 
				//ship.Rotate (newVec * 6.0f * rotationsPerMinute * Time.deltaTime);
			} else {
				
				if (rb.velocity.magnitude >= 25.0f) {
					rb.velocity = ship.right * zDistance * 10;
				} else {
					rb.AddForce(ship.right * zDistance * 25);
				}

			}

			/* For position within quadrants, use current localPosition */

		} else {
			/* Set initial localPosition to current localPosition */
			transform.localPosition = initPos;
		}
	}
}