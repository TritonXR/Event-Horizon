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
	// Use this for initialization
	void Start () {
        //defaultMaterial = this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void SetTargetShip(GameObject ship)
    {
        this.target = ship;
        this.targetVector = ship.transform.position;
        this.targetRotation = Quaternion.LookRotation(ship.transform.position);
        movementMode = 1;

        print("Recieved target " + this.target.name);
    }
    public virtual void SetTargetPosition(Vector3 vector)
    {
        this.targetVector = vector;
        this.targetRotation = Quaternion.LookRotation(vector);
        movementMode = 2;
    }
    public virtual void SetSelected (bool selected) {
        
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
}
