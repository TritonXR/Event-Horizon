using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitalShip : JPShip {
    Vector3 targetPos;
    public bool idle;
    Rigidbody rb;
    public float speedMagnitude;
	// Use this for initialization
	void Start () {
		//target = GameObject.Find ("DefaultTarget");
        defaultMaterial = this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!isServer) {
            //print("is Not server");
            return;
        }
        //print("is server");
        if(movementMode == 1) {
            targetPos = target.transform.position;
        }
        if(movementMode == 2) {
            targetPos = targetVector;
        }

        float dist = Vector3.Distance(transform.position, targetPos);
        if (dist < distanceTol)
        {
            idle = true;
        }
        if(!idle) {
            targetPos.y = transform.position.y;
            Vector3 targetDir = targetPos - transform.position;
            float step = turnSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            Debug.DrawRay(transform.position, newDir, Color.red);
            Quaternion rotateTo = Quaternion.LookRotation(newDir);
            rotateTo.x = 0;
            rotateTo.z = 0;
            transform.rotation = rotateTo;
            //rb.velocity = transform.forward * moveSpeed;
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = transform.forward * moveSpeed;
            }
            else
            {
                rb.AddForce(transform.forward * moveSpeed * 10000);
            }
            speedMagnitude = rb.velocity.magnitude;
        } else if(idle) {
            dist = Vector3.Distance(transform.position, targetPos);
            if (dist < distanceTol)
            {
                
            }
            else
            {
                idle = false;
            }
        }
		/*if (movementMode == 1) {
            if (Vector3.Distance (transform.position, target.transform.position) > fireDist) {
				var rotation = Quaternion.LookRotation (target.transform.position);
				if ((rotation.eulerAngles.y - 0.5f<transform.rotation.eulerAngles.y)&&(transform.rotation.eulerAngles.y<rotation.eulerAngles.y + 0.5f)) {
						transform.position = Vector3.MoveTowards (transform.position, target.transform.position, moveSpeed);
				} else {
					transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * turnSpeed);
				}
			}
		} else if(movementMode == 2) {
			var rotation = Quaternion.LookRotation (targetVector);
			if ((rotation.eulerAngles.y - 0.5f<transform.rotation.eulerAngles.y)&&(transform.rotation.eulerAngles.y<rotation.eulerAngles.y + 0.5f)) {
					transform.position = Vector3.MoveTowards (transform.position, targetVector, moveSpeed);
			} else {
				transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * turnSpeed);
			}
		}*/

	}

	public override void SetTargetShip(GameObject ship){
		
        base.SetTargetShip(ship);

		print ("Recieved target "+this.target.name);
	}

	public override void SetTargetPosition(Vector3 vector){
        base.SetTargetPosition(vector);
		
	}
    public override void SetSelected(bool selected)
    {
        if (selected)
        {
            this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = selectedMaterial;
        }
        else
        {
            this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = defaultMaterial;
        }
    }

}
