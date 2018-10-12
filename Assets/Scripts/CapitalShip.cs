using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitalShip : JPShip {

    [Header("CAPITAL SHIP OPTIONS", order = 0)]

    [Header("Movement", order = 1)]
    public float speedMagnitude;
    public bool idle;

    Vector3 targetPos;
    Rigidbody rb;

    // Use this for initialization
    void Start () {
		//target = GameObject.Find ("DefaultTarget");
        defaultMaterial = this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
        rb = GetComponent<Rigidbody>();
        offset = wingmenOffsets[squadNum];
        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        
        //print("Change material " + GetComponent<JPNetworkShip>().teamNumber);
        if ((GetComponent<JPNetworkShip>().teamNumber != 0) && (materialSwitch))
        {
            if (GetComponent<JPNetworkShip>().teamNumber == 2)
            {
                defaultMaterial = altDefaultMaterial;
                this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = altDefaultMaterial;
            }
            print("Change material " + GetComponent<JPNetworkShip>().teamNumber);
            materialSwitch = false;
        }
        if(!isServer) {
            
            return;
        }
        if (warping)
        {
            if (Vector3.Distance(transform.position, warpTarget) < 10f)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                warping = false;
            }
            else
            {
                float step = warpSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, warpTarget, step);
            }
            return;
        }
        if (destroyed)
        {
            return;
        }

        if (health < 0)
        {
            SetDestroyed();
            return;
        }
        if ((controlLock) || (moveLock))
        {
            return;
        }
        float dist = Vector3.Distance(transform.position, targetPos);
        if (movementMode == 1) {
            targetPos = target.transform.position;
            if (dist < distanceTol * 5f)
            {
                idle = true;
            }
        }
        if(movementMode == 2) {
            targetPos = targetVector;
            if (dist < distanceTol)
            {
                idle = true;
            }
        }


        //print(dist);

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
            float step = turnSpeed * 50f * Time.deltaTime;
            //print(Quaternion.RotateTowards(transform.rotation, targetRotation, step));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
            rb.velocity = Vector3.zero;
            dist = Vector3.Distance(transform.position, targetPos);
            if (dist < distanceTol * 5f)
            {
                
            }
            else
            {
                //idle = false;
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
        idle = false;
		print ("Recieved target "+this.target.name);
	}

	public override void SetTargetPosition(Vector3 vector){
        base.SetTargetPosition(vector);
        idle = false;
    }
    public override void SetSelected(bool selected)
    {
        if (selected)
        {
            this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = selectedMaterial;
            hpShow.SetActive(true);
        }
        else
        {
            this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = defaultMaterial;
            if(forceHPShow) {
                hpShow.SetActive(true);
            } else {
                hpShow.SetActive(false);
            }

        }
    }

    public override void SetDestroyed()
    {
        destroyed = true;
        this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().enabled = false;
        transform.position = new Vector3(-10000, -10000);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        base.SetDestroyed();
    }

}
