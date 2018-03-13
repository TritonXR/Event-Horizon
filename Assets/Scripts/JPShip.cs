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

    public float maxHeight = 10.0f;
    public float minHeight = 0;

    public float distanceTol = 10.0f;
    public int health = 0;

    [SyncVar]
    public int teamNum = 0;

    public GameObject[] wingmen;
    public Vector3[] wingmenOffsets;
    public Vector3 offset;
    public int squadNum = 0;
    public bool lead = true;
    public JPShip leadController;

	// Use this for initialization
	void Start () {
        //defaultMaterial = this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
        if(lead) {
            leadController = this;
        }
        offset = wingmenOffsets[squadNum];

	}
	
	// Update is called once per frame
	void Update () {
		
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
            for (int count = 1; count < wingmen.Length; count++)
            {
                wingmen[count].GetComponent<JPShip>().SetTargetPosition(vector);
            }
        }
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
        }
        else
        {
            this.transform.GetChild(0).GetComponent<Renderer>().material = defaultMaterial;
        }
    }
	private void OnCollisionEnter(Collision collision)
	{
        health++;
	}
	private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.GetComponent<Projectile>()) {
            health += other.gameObject.GetComponent<Projectile>().damage;
            Destroy(other.gameObject);
        }
	}
    public virtual void OnShipControlDisable(bool disable) {
        this.enabled = disable;
    }
    [ClientRpc]
    public void RpcSetLeadController(string leadName, int playerNum, int shipNum, int maxNum)
    {
        print("rpc leadcontroller set called " + leadName);
        leadController = GameObject.Find(leadName).GetComponent<JPShip>();
        if(leadName != gameObject.name) {
            lead = false;
        }
        for (int count = 0; count < maxNum; count ++) {
            wingmen[count] = GameObject.Find("Player" + playerNum + "Ship" + shipNum + "Squad" + count);
        }

    }
}
