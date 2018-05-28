using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JPShip : NetworkBehaviour {

    public GameObject target;
    public float fireDist = 5f;
    public float rangeDist = 20.0f;

    public int idleMode = 0;

    public float moveSpeed = 0.01f;
    public float turnSpeed = 0.2f;

    public Vector3 targetVector;
    public Quaternion targetRotation;
    public int movementMode = 0; //0 = stop 1 = target ship 2 = vector

    public Material defaultMaterial;
    public Material selectedMaterial;
    public Material altDefaultMaterial;
    public bool materialSwitch = true;

    public float maxHeight = 10.0f;
    public float minHeight = 0;

    public float distanceTol = 10.0f;
    [SyncVar]
    public int maxHealth = 1000;
    [SyncVar]
    public int health = 0;
    [SyncVar]
    public float healthPercent = 1;

    [SyncVar]
    public int teamNum = 0;

    [SyncVar]
    public int specMode = 0;

    public GameObject[] wingmen;
    public Vector3[] wingmenOffsets;
    public Vector3 offset;
    public int squadNum = 0;
    public bool lead = true;
    public JPShip leadController;

    public bool destroyed = false;
    public bool fighter = true;

    public GameObject hpShow;

    public BaseSkill[] skills = new BaseSkill[3];
    public bool warping = false;
    public Vector3 warpTarget;

	// Use this for initialization
	void Start () {
        //defaultMaterial = this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
        if(lead) {
            leadController = this;
        }
        offset = wingmenOffsets[squadNum];
        health = maxHealth;
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
                float step = 500f * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, warpTarget, step);
            }
            return;
        }
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

        print("Recieved target " + this.target.name);
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
        this.targetRotation = Quaternion.LookRotation(vector);
        movementMode = 2;
    }
    public virtual void SetPos (Vector3 vector) {
        this.targetVector = vector;
        this.targetRotation = Quaternion.LookRotation(vector);
        movementMode = 2;
    }
    public virtual void SetSelected (bool selected) {
        if (lead)
        {
            print("Lead Selected " + wingmen.Length);
            for (int count = 1; count < wingmen.Length; count++)
            {

                print("select " + count);
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
        }
    }
	private void OnCollisionEnter(Collision collision)
	{
        //health++;
	}
	private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.GetComponent<Projectile>()) {
            int damageMult = 1;
            if((other.gameObject.GetComponent<Projectile>().fighter) && (fighter)) {
                damageMult = 25;
            }
            //health -= other.gameObject.GetComponent<Projectile>().damage;
            Destroy(other.gameObject);
            healthPercent = health / maxHealth;
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
    public void JumpToLocation (Vector3 pos) {
        print("Jumping");
        warping = true;
        warpTarget = pos;
    }
}
