using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPFighter : JPShip {
    Rigidbody rb;
    JPSquadron leader;

    public Vector3 targetPos;

    public Vector3 offset;
    public Vector3 showTargetVector;
    public GameObject showTarget;

    public bool avoiding = false;
    public bool engaged = false;
    public bool idle = false;

    public Vector3 dodgeOffset;
    public bool dodgeCheck = false;
    public bool clearCheck = false;
    public bool clearing = false;

    public bool turning = false;
    public Vector3 saveNewDir;
    public float dist;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        defaultMaterial = transform.GetChild(0).GetComponent<Renderer>().material;
        movementMode = 1;
	}
	
	// Update is called once per frame
	void Update () {
        

        if (avoiding) {
            print("Avoiding movement");
            Vector3 dwn = transform.TransformDirection(-Vector3.up) * rangeDist * 1.5f;
            Debug.DrawRay(transform.position, dwn, Color.yellow, 0.01f);
            RaycastHit hit;
            if (!clearing)
            {
                if (Physics.Raycast(transform.position, dwn, out hit, rangeDist))
                {
                    if (!clearCheck)
                    {
                        print("Avoiding " + hit.collider.gameObject.name);
                        targetPos = transform.position + dodgeOffset;
                    }
                    else if (clearCheck)
                    {
                        if (movementMode == 1)
                        {
                            targetPos = target.transform.position;
                        }
                        else
                        {
                            targetPos = targetVector;
                        }
                        targetPos.y = transform.position.y;
                    }
                }
                else
                {
                    if ((dodgeCheck) && (!clearCheck))
                    {
                        print("Avoid Clear");
                        if (movementMode == 1)
                        {
                            targetPos = target.transform.position;
                        }
                        else
                        {
                            targetPos = targetVector;
                        }
                        targetPos.y = transform.position.y;
                        clearing = true;
                        StartCoroutine(WaitClear());
                    }
                    else if (clearCheck)
                    {
                        avoiding = false;
                    }
                    else
                    {
                        targetPos = transform.position + dodgeOffset;
                    }
                }
            }
        } else if ((!avoiding) && (!engaged) && (!idle)) {
            print("Normal Movement");
            if (movementMode == 1)
            {
                targetPos = target.transform.position;
            }
            else
            {
                targetPos = targetVector;
            }
            targetPos = targetPos + (offset * 100.0f);


            Vector3 fwd = transform.TransformDirection(Vector3.forward) * rangeDist;
            Debug.DrawRay(transform.position, fwd, Color.yellow, 0.01f);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, fwd, out hit, rangeDist))
            {
                Dodge(hit);
            }
            dist = Vector3.Distance(transform.position, targetPos);
            if(dist < distanceTol) {
                idle = true;
            }
        } else if(idle) {

            print("idle movement");
            if (movementMode == 1)
            {
                targetPos = target.transform.position;
            }
            else
            {
                targetPos = targetVector;
            }
            targetPos = targetPos + (offset * 100.0f);
            dist = Vector3.Distance(transform.position, targetPos);
            if (dist < distanceTol)
            {
                targetPos = transform.forward * 10;
            } else {
                idle = false;
            }
        }
        //showTargetVector = targetPos;
        if (showTarget)
        {
            showTarget.transform.position = targetPos;
        }
        if (!idle)
        {
            Vector3 targetDir = targetPos - transform.position;
            float step = turnSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            Debug.DrawRay(transform.position, newDir, Color.red);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
        rb.velocity = transform.forward * moveSpeed;


	}
    void Dodge(RaycastHit hit) {
        print("There is something in front of the object! " + hit.collider.gameObject.name);
        avoiding = true;
        clearCheck = false;
        dodgeCheck = false;
        if(transform.position.y < maxHeight) {
            dodgeOffset = dodgeOffset + transform.forward * 2f;
            dodgeOffset.y = 12.0f;

            print(transform.forward);
        }

        StartCoroutine(WaitDodge());
    }
    IEnumerator WaitDodge()
    {
        //print(Time.time);
        //dodgeCheck = false;
        yield return new WaitForSeconds(1f);
        //print(Time.time);
        dodgeCheck = true;
    }
    IEnumerator WaitClear()
    {
        //print(Time.time);
        //clearCheck = false;
        yield return new WaitForSeconds(3f);
        //print(Time.time);
        clearCheck = true;
        clearing = false;
    }
    public override void SetTargetShip(GameObject ship)
    {
        base.SetTargetShip(ship);
    }
    public override void SetTargetPosition(Vector3 vector)
    {
        
        base.SetTargetPosition(vector);
        targetVector += offset;
    }
    public void SetOffset(Vector3 vector) {
        offset = vector;
    }
    public void SetController(JPSquadron controller) {
        leader = controller;
        moveSpeed = leader.moveSpeed;
        turnSpeed = leader.turnSpeed;
        rangeDist = leader.rangeDist;
        fireDist = leader.fireDist;
        distanceTol = leader.distanceTol;

    }

}
