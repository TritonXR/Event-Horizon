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
    bool engaged = false;
    bool idle = false;

    Vector3 dodgeOffset;
    bool dodgeCheck = false;
    bool clearCheck = false;
    bool clearing = false;

    bool turning = false;
    public Vector3 finalTarget;
    float dist;
    bool resetHeightHigh;
    bool resetHeightLow;

    public GameObject clipObj;

    public string mode;
    string avoidObjectName;
    public GameObject avoidObject;

    public LayerMask ignoreMask;
    int countDodge = 0;
    public bool serverControl = false;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        defaultMaterial = transform.GetChild(0).GetComponent<Renderer>().material;
        //movementMode = 1;
        idle = true;
	}

    // Update is called once per frame
    void Update()
    {
        if (!serverControl)
        {
            mode = "Not Server";
            return;
        }
        if (avoiding)
        {
            mode = "Avoiding " + avoidObjectName;
            Vector3 dwn = transform.TransformDirection(-Vector3.up) * rangeDist * 1.5f;
            //Debug.DrawRay(transform.position, dwn, Color.yellow, 0.01f);
            RaycastHit hit;
            if (!clearing)
            {
                if (Physics.Raycast(transform.position, dwn, out hit, rangeDist, ignoreMask))
                {
                    if (!clearCheck)
                    {
                        print("Avoiding " + hit.collider.gameObject.name);
                        targetPos = transform.position + dodgeOffset;
                    }
                    else if (clearCheck)
                    {
                        targetPos = finalTarget;
                        targetPos.y = transform.position.y;
                    }
                }
                else
                {
                    if ((dodgeCheck) && (!clearCheck))
                    {
                        print("Avoid Clear");
                        targetPos = finalTarget;
                        targetPos.y = transform.position.y;
                        clearing = true;
                        StartCoroutine(WaitClear());
                    }
                    else if (clearCheck)
                    {
                        clipObj = null;
                        avoiding = false;
                    }
                    else
                    {
                        targetPos = transform.position + dodgeOffset;
                    }
                }
            }
        }
        else if (resetHeightHigh)
        {
            mode = "Moving downwards" + transform.position.y;
            if (transform.position.y < maxHeight / 2)
            {
                resetHeightHigh = false;
            }
            targetPos = finalTarget;
            targetPos.y = 5f;
        }
        else if (transform.position.y > maxHeight)
        {
            mode = "Above max height " + transform.position.y;
            targetPos = finalTarget;
            targetPos.y = 5f;
            resetHeightLow = true;
            idle = false;
        }
        else if (transform.position.y < minHeight)
        {
            mode = "Below min height " + transform.position.y;
            targetPos = finalTarget;
            targetPos.y = 10f;
            resetHeightHigh = true;
            idle = false;
        }
        else if (resetHeightLow)
        {
            mode = "Moving Upwards" + transform.position.y;
            if (transform.position.y < maxHeight / 4)
            {
                resetHeightLow = false;
            }
            targetPos = finalTarget;
            targetPos.y = 10f;
        }else if ((!avoiding) && (!engaged) && (!idle)) {
            

            if (movementMode == 1)
            {
                finalTarget = target.transform.position;
            }
            else
            {
                finalTarget = targetVector;
            }
            targetPos = finalTarget;

            targetPos = targetPos + (offset * 150.0f);
            targetPos.y = 14f;  

            Vector3 fwd = transform.TransformDirection(Vector3.forward) * rangeDist;
            Debug.DrawRay(transform.position, fwd, Color.yellow, 0.01f);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, fwd, out hit, rangeDist, ignoreMask))
            {
                Dodge(hit);
                mode = "Colliding " + countDodge;
            } else {
                clipObj = null;
                mode = "Normal Movement";
            }
            dist = Vector3.Distance(transform.position, targetPos);
            if(dist < distanceTol/10) {
                idle = true;
            }
        } else if(idle) {
            
            Vector3 fwd = transform.TransformDirection(Vector3.forward) * rangeDist;
            Debug.DrawRay(transform.position, fwd, Color.green, 0.01f);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, fwd, out hit, rangeDist, ignoreMask))
            {
                Dodge(hit);
                mode = "Idle Colliding " + countDodge;
            } else {
                mode = "idle movement";
            }

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
                
                finalTarget = transform.forward * 7.5f;
                targetPos = finalTarget;
            } else {
                idle = false;
                if (movementMode == 1)
                {
                    finalTarget = target.transform.position;

                }
                else
                {
                    finalTarget = targetVector;
                }
            }

        } else {
            mode = "Stuck";
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
            //Debug.DrawRay(transform.position, newDir, Color.red);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
        if(rb.velocity.magnitude > moveSpeed) {
            rb.velocity = transform.forward * moveSpeed;
        } else {
            rb.AddForce(transform.forward * moveSpeed );
        }



	}
    void Dodge(RaycastHit hit) {
        if(hit.collider.gameObject == clipObj) {
            //StartCoroutine(NoClip());

            //return;
        }
        countDodge++;
        clipObj = hit.collider.gameObject;
        avoidObjectName = hit.collider.gameObject.name;
        avoidObject = hit.collider.gameObject;
        avoiding = true;
        clearCheck = false;
        dodgeCheck = false;
        if(transform.position.y < maxHeight) {
            dodgeOffset = dodgeOffset + transform.forward * 5f;
            dodgeOffset.y = 12.0f;

            //print(transform.forward);
        }

        StartCoroutine(WaitDodge());
        StartCoroutine(WaitAvoid());
    }
    IEnumerator WaitDodge()
    {
        //print(Time.time);
        //dodgeCheck = false;
        yield return new WaitForSeconds(3f);
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
    IEnumerator WaitAvoid()
    {
        //print(Time.time);
        //clearCheck = false;
        yield return new WaitForSeconds(15f);
        if (avoiding)
        {
            //print(Time.time);
            clearCheck = false;
            clearing = false;
            avoiding = false;
            if (movementMode == 1)
            {
                finalTarget = target.transform.position;
            }
            else
            {
                finalTarget = targetVector;
            }
            targetPos = finalTarget;
            //StartCoroutine(NoClip());
        }
    }
    IEnumerator NoClip()
    {
        //GetComponent<Collider>().isTrigger = true;
        yield return new WaitForSeconds(5f);
        //GetComponent<Collider>().isTrigger = false;
    }
    public override void SetTargetShip(GameObject ship)
    {
        base.SetTargetShip(ship);
        movementMode = 1;
    }
    public override void SetTargetPosition(Vector3 vector)
    {
        
        base.SetTargetPosition(vector);
        movementMode = 2;
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
    public override void SetSelected(bool selected)
    {
        if (selected)
        {
            this.transform.GetChild(0).GetComponent<Renderer>().material = selectedMaterial;
        }
        else
        {
            this.transform.GetChild(0).GetComponent<Renderer>().material = defaultMaterial;
        }
    }

}
