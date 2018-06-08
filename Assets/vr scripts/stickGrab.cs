using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stickGrab : MonoBehaviour
{
    Vector3 initPos;
    Transform ship;
    public bool throttle;
    float rotationsPerMinute = 1.0f;
    public bool used;
    Rigidbody rb;
    // Use this for initialization
    void Start()
    {
        initPos = transform.localPosition;
        ship = transform.parent.parent;
        used = false;
        rb = ship.GetComponent<Rigidbody>();
    }

    public Vector3 getInitPos()
    {
        return initPos;
    }

    public void Picked()
    {
        used = true;
    }

    public void Dropped()
    {
        used = false;
        transform.localPosition = initPos;
        Debug.Log("dropped");
    }

    // Update is called once per frame
    void Update()
    {
        if (used == true)
        {
            Vector3 localPos = transform.localPosition;
            float xDistance = localPos.x - initPos.x;
            float yDistance = localPos.y - initPos.y;
            float zDistance = localPos.z - initPos.z;
            if (throttle == false)
            {
                if (zDistance > 5.0f)
                {
                    zDistance  = 5f;
                }
                if (zDistance < -5f)
                {
                    zDistance = -5f;
                }
                if (xDistance > 5.0f)
                {
                    xDistance = 5f;
                }
                if (xDistance < -5f)
                {
                    xDistance = -5f;
                }
                Vector3 newVec = new Vector3(1.0f * zDistance, 0.0f * xDistance, -0.5f * xDistance);
                ship.Rotate(newVec * 6.0f * rotationsPerMinute * Time.deltaTime);
            }
            else
            {
                //ship.Translate (-1 * ship.forward * Time.deltaTime * zDistance * 10);

                if(Mathf.Abs(xDistance) < 0.5f)
                {
                    xDistance = 0;
                }
                Vector3 newVec = new Vector3(0.0f * zDistance, 1.0f * xDistance, -0.0f * xDistance);
                ship.Rotate(newVec * 6.0f * rotationsPerMinute * Time.deltaTime);

                //print(zDistance);
                if(zDistance > 0.0f)
                {
                    //zDistance  = zDistance * 10;
                }
                if(zDistance < 0f)
                {
                    zDistance = 0f;
                }
                if (rb.velocity.magnitude >= 25.0f)
                {
                    //rb.velocity = ship.forward * zDistance * 25;
                }
                else
                {
                    rb.velocity = ship.forward * 10 * zDistance;
                    //rb.AddForce(ship.forward * zDistance * 25);
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
        }
        else
        {
            transform.localPosition = initPos;
        }
    }
}

/*public class stickGrab : MonoBehaviour {
	Vector3 initPos;
    Vector3 startPos;       // Origin is not 0,0,0, but rather its own position. Needs to be factored in
	Transform ship;
	public bool throttle; 	// If throttle is checked, then only moves forward and backward. 
							// Else, 360 rotation
	float rotationsPerMinute = 0.5f;
	float xMax = 10.0f;
	float zMax = 10.0f;

    Quaternion startRotation;
    Quaternion endRotation;
    float rotationProgress = -1;

    // WHY IS THIS PUBLIC AND WHY IS IT BUGGY WHEN ITS PRIVATE
    public bool used; // True, invis ball is being interacted/grabbed.
	Rigidbody rb;
	// Use this for initialization
	void Start () {
		initPos = transform.localPosition; // When values are changed, need this to visually see ship moving.
        ship = transform.parent.parent;
		used = false;
		rb = ship.GetComponent<Rigidbody> ();

   
        Debug.Log("Starting Coordinates: (" + initPos.x + "," + initPos.y + "," + initPos.z+ ")");
    }

	public Vector3 getInitPos() {
		return initPos;
	}*/

/*
 * Called when ball is interacted with.
 *
 */
/*
public void Picked() {
   used = true;
} 
*/
/*
 * Called when ball is let go.
 */
/*
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
       Vector3 localPos = transform.localPosition;*/
/* Difference between current localPosition and initial localPosition */ /*
float xDistance = localPos.x - initPos.x;
float yDistance = localPos.y - initPos.y;
float zDistance = localPos.z - initPos.z;

float deadZone = 2.0f;

Debug.Log("Distance: (" + xDistance + "," + yDistance + "," + zDistance + ")");*/

/* Debug */
/*print ("xDistance: " + xDistance + ". yDistance: " + yDistance + 
    ", zDistance: " + zDistance);*/

/* Throttle is checked off: 360 rotation instead of forward and backward *//*
if (throttle == false) {
    // Seperate movement and rotation to respective pair of quadrants 
    float ratioXtoZ = xDistance / zDistance;
    //Debug.Log ("RatioXToZ: " + ratioXtoZ);

    // Top quadrant: Dive down 
    if ( zDistance > deadZone ) {
        // Left half 
        if (xDistance < 0 && ratioXtoZ > -1) {
            // Rotate around z axis
            Vector3 newVec = new Vector3(1.0f * zDistance, 0, 0);
            ship.Rotate (newVec * 6.0f * rotationsPerMinute * Time.deltaTime);
        }

        // Right half 
        else  if (xDistance > 0 && ratioXtoZ < 1) {
            Vector3 newVec = new Vector3(1.0f * zDistance, 0, 0);
            ship.Rotate (newVec * 6.0f * rotationsPerMinute * Time.deltaTime);
        }
    }

    // Bottom quadrant: Dive up
    if (zDistance < -1 * deadZone) {
        // Left half 
        if (xDistance < 0 && ratioXtoZ < 1) {
            // Rotate around z axis
            Vector3 newVec = new Vector3(1.0f * zDistance, 0, 0);
            ship.Rotate(newVec * 6.0f * rotationsPerMinute * Time.deltaTime);
        }

        // Right half 
        else if (xDistance > 0 && ratioXtoZ > -1)
        {
            Vector3 newVec = new Vector3(1.0f * zDistance, 0, 0);
            ship.Rotate(newVec * 6.0f * rotationsPerMinute * Time.deltaTime);
        }
    }

    // Left Quadrant: Yaw Left
    if (xDistance < -1 * deadZone)
    {
        // Top half 
        if (zDistance > 0 && ratioXtoZ < -1)
        {
            // Rotate around y axis
            Vector3 newVec = new Vector3(0, 1.0f * xDistance, 0);
            ship.Rotate(newVec * 6.0f * rotationsPerMinute * Time.deltaTime);
        }

        // Bottom half 
        else if (zDistance < 0 && ratioXtoZ > 1)
        {
            Vector3 newVec = new Vector3(0, 1.0f * xDistance, 0);
            ship.Rotate(newVec * 6.0f * rotationsPerMinute * Time.deltaTime);
        }
    }
    // Right Quadrant: Yaw Right
    if (xDistance > deadZone)
    {
        // Top half 
        if (zDistance > 0 && ratioXtoZ > 1)
        {
            // Rotate around y axis
            Vector3 newVec = new Vector3(0, 1.0f * xDistance, 0);
            ship.Rotate(newVec * 6.0f * rotationsPerMinute * Time.deltaTime);
        }

        // Bottom half 
        else if (zDistance < 0 && ratioXtoZ < -1)
        {
            Vector3 newVec = new Vector3(0, 1.0f * xDistance, 0);
            ship.Rotate(newVec * 6.0f * rotationsPerMinute * Time.deltaTime);
        }
    }

    // Movement 
    //Vector3 newVec = new Vector3 (
    //	1.0f * xDistance, 1.0f * yDistance, 1.0f * zDistance);

    // Rotation 
    //ship.Rotate (newVec * 6.0f * rotationsPerMinute * Time.deltaTime);
} else {

    if (rb.velocity.magnitude >= 25.0f) {
        rb.velocity = ship.forward* zDistance * 10;
    } else {
        rb.AddForce(ship.forward * zDistance * 25);
    }

}*/

/* For position within quadrants, use current localPosition */
/*
} else {*/
/* Set initial localPosition to current localPosition *//*
transform.localPosition = initPos;

Debug.Log("Angles: " + ship.rotation.x +","+ ship.rotation.y + ","+ ship.rotation.z);


StartRotating(0.0f);

if (rotationProgress < 1 && rotationProgress >= 0)
{
    Debug.Log("Its printing");
    rotationProgress += Time.deltaTime * 5;

    // Here we assign the interpolated rotation to transform.rotation
    // It will range from startRotation (rotationProgress == 0) to endRotation (rotationProgress >= 1)
    transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress);
}
*/



/*
if (ship == null)
{
    StartCoroutine(RotateImage());
}
*/
/*
}
}

// Call this to start the rotation
void StartRotating(float zPosition)
{

// Here we cache the starting and target rotations
startRotation = transform.rotation;
endRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, zPosition);

// This starts the rotation, but you can use a boolean flag if it's clearer for you
rotationProgress = 0;
}




IEnumerator RotateImage()
{*/
/*
float moveSpeed = 0.1f;
while (ship.transform.rotation.y < 180)
{
    ship.transform.rotation = Quaternion.Lerp(ship.transform.rotation, Quaternion.Euler(0, 180, 0), moveSpeed * Time.time);
    yield return null;
}
ship.transform.rotation = Quaternion.Euler(0, 180, 0);
yield return null;
*/
/*
float moveSpeed = 0.1f;
if (used != true) {
    yield return null;
}
while (rb.transform.rotation.z >= -90 && rb.transform.rotation.z <= 90)
{
    rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, 
        Quaternion.Euler(rb.transform.rotation.x, rb.transform.rotation.y, 0), moveSpeed * Time.time);
    yield return null;
}
rb.transform.rotation = Quaternion.Euler(rb.transform.rotation.x, rb.transform.rotation.y, 0);
yield return null;

}
}*/
