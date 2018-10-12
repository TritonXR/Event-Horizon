using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JPShip : NetworkBehaviour {
    [Header("GENERIC SHIP OPTIONS", order = 0)]
    [Header ("Network Player", order = 1)]
    public JPNetworkPlayer playerController;

    [Header("Movement Locks")]
    public bool controlLock = false;
    public bool moveLock = true;

    [Header("Rotation")]
    public bool rotationControl = false;

    [Header("Movement Mode")]
    public int idleMode = 0;

    [Header("Movement Speeds")]
    public float moveSpeed = 0.01f;
    public float turnSpeed = 0.2f;

    [Header("Targeting Options")]
    public GameObject target;
    public float fireDist = 5f;
    public float rangeDist = 20.0f;

    [Header("Movement Options")]
    public int movementMode = 0; //0 = stop 1 = target ship 2 = vector
    [SyncVar]
    public int specMode = 0;
    public Vector3 targetVector;
    public Quaternion targetRotation;
    public float distanceTol = 10.0f;
    public float maxHeight = 10.0f;
    public float minHeight = 0;

    [Header("Materials")]
    public Material defaultMaterial;
    public Material selectedMaterial;
    public Material altDefaultMaterial;
    public bool materialSwitch = true;

    [Header("Health")]
    public bool healthSyncVarSpacer;
    [SyncVar]
    public int maxHealth = 1000;
    [SyncVar]
    public int health = 0;
    [SyncVar]
    public float healthPercent = 1;
    public bool destroyed = false;
    public GameObject hpShow;
    public bool forceHPShow = false;

    [Header("Ship Value")]
    public int shipValue = 10;

    [Header("Team Organization")]
    public bool teamOrgznizationSyncVarSpacer;
    [SyncVar]
    public int teamNum = 0;

    [Header("Squadrons")]
    public GameObject[] wingmen;
    public Vector3[] wingmenOffsets;
    public Vector3 offset;
    public int squadNum = 0;
    public bool lead = true;
    public JPShip leadController;
    public bool fighter = true;

    [Header("Skills")]
    public BaseSkill[] skills = new BaseSkill[3];

    [Header("Weapons")]
    public bool weaponsSyncVarSpacer;
    [SyncVar]
    public int fireRate = 15;

    [Header("Warp")]
    public bool warping = false;
    public Vector3 warpTarget;
    public bool warpRotLock = false;
    public int warpSpeed = 250;

    [Header("Debug")]
    public bool showDebug = false;

	// Use this for initialization
	void Start () {
        //defaultMaterial = this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
        if(lead) {
            leadController = this;
        }
        offset = wingmenOffsets[squadNum];
        health = maxHealth;
        if (forceHPShow)
        {
            hpShow.SetActive(true);
        }
        if(!isServer) {
            GetComponent<Collider>().isTrigger = true;
        }
        //hpShow = transform.Find("HP").gameObject;
        //hpShow.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if(warping) {
            if(Vector3.Distance(transform.position, warpTarget) < 10f) {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                warping = false;
            } else {
                float step = warpSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, warpTarget, step);
            }
            return;
        }
	}
    public virtual void SetDestroyed() {
        playerController.DecrementFleetHealth(shipValue);
    }


    public virtual void SetTargetShip(GameObject ship)
    {
        if(lead) {
            for (int count = 1; count < wingmen.Length; count ++) {
                wingmen[count].GetComponent<JPShip>().SetTargetShip(ship);
            }
        }
        this.target = ship;
        this.targetVector = ship.transform.position;
        this.targetRotation = Quaternion.LookRotation(ship.transform.position);
        movementMode = 1;
        moveLock = false;
        //print("Recieved target " + this.target.name);
    }
    public virtual void SetTargetPosition(Vector3 vector)
    {
        if (lead)
        {
            for (int count = 0; count < wingmen.Length; count++)
            {
                wingmen[count].GetComponent<JPShip>().SetPos(vector);
            }
        }
        this.targetVector = vector;
        //this.targetRotation = Quaternion.LookRotation(vector);
        movementMode = 2;
    }
    public virtual void SetTargetRotation(Quaternion rot)
    {
        if (lead)
        {
            for (int count = 1; count < wingmen.Length; count++)
            {
                wingmen[count].GetComponent<JPShip>().SetTargetRotation(rot);
            }
        }
        this.targetRotation = rot;
        movementMode = 2;
    }
    public virtual void SetPos (Vector3 vector) {
        this.targetVector = vector;
        //this.targetRotation = Quaternion.LookRotation(vector);
        movementMode = 2;
        moveLock = false;
    }
    public virtual void SetSelected (bool selected) {
        if (lead)
        {
            print("Lead Selected " + wingmen.Length);
            for (int count = 1; count < wingmen.Length; count++)
            {

                //print("select " + count);
                wingmen[count].GetComponent<JPShip>().SetSelected(selected);
            }

        }
        if (selected)
        {
            this.transform.GetChild(0).GetComponent<Renderer>().material = selectedMaterial;
            hpShow.SetActive(true);
        }
        else
        {
            this.transform.GetChild(0).GetComponent<Renderer>().material = defaultMaterial;
            hpShow.SetActive(false);
            if(forceHPShow) {
                hpShow.SetActive(true);
            }
        }
        moveLock = false;
    }


	private void OnCollisionEnter(Collision collision)
	{
        //health++;
	}
	private void OnTriggerEnter(Collider other)
	{
        if (showDebug)
        {
            print("hit " + other.gameObject.name);
        }
        if (other.gameObject.GetComponent<Projectile>()) {
            
            if((other.gameObject.GetComponent<Projectile>().teamNum != teamNum)||(!other.gameObject.GetComponent<Projectile>().ignoreColl)) {
                if (showDebug)
                {
                    print(other.gameObject.GetComponent<Projectile>().damage);
                }
                health -= other.gameObject.GetComponent<Projectile>().damage;
                other.gameObject.GetComponent<Projectile>().CollisionResponse();
                //Destroy(other.gameObject);
                healthPercent = health / maxHealth;
            }


        }
	}


    public virtual void OnShipControlDisable(bool disable) {
        this.enabled = disable;
    }


    [ClientRpc]
    public void RpcSetLeadController(string leadName, int playerNum, int shipNum, int maxNum)
    {
        //print("rpc leadcontroller set called " + leadName);
        leadController = GameObject.Find(leadName).GetComponent<JPShip>();
        if(leadName != gameObject.name) {
            lead = false;
        }
        for (int count = 0; count < maxNum; count ++) {
            wingmen[count] = GameObject.Find("Player" + playerNum + "Ship" + shipNum + "Squad" + count);
        }

    }


    public void SetMode (int newMode) {
        if(lead) {
            
            for (int count = 1; count < wingmen.Length; count++)
            {
                wingmen[count].GetComponent<JPShip>().SetMode(newMode);
            }
        }
        specMode = newMode;

    }

    public void TriggerSkill(int skillNum) {
        if(skills[skillNum] != null) {
            skills[skillNum].TriggerSkill();
        }

    }
    public void TriggerSkillLocation(int skillNum, Vector3 pos)
    {
        if (skills[skillNum] != null)
        {
            skills[skillNum].TriggerSkillLocation(pos);
        }

    }
    public void TriggerSkillTarget(int skillNum, GameObject target)
    {
        if (skills[skillNum] != null)
        {
            skills[skillNum].TriggerSkillTarget(target);
        }

    }
    public void JumpToLocation (Vector3 pos, bool rotLock) {
        //print("Jumping");
        warpRotLock = rotLock;
        warping = true;
        warpTarget = pos;
    }
}
