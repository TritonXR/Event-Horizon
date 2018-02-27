using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPShip : MonoBehaviour {

    [SerializeField] public GameObject target;
    [SerializeField] public float FireRange = 5f;
    public int idleMode = 0;
    public float moveSpeed = 0.01f;
    public float turnSpeed = 0.2f;
    public Vector3 targetVector;
    public Quaternion targetRotation;
    public int movementMode = 0; //0 = stop 1 = target ship 2 = vector
    public Material defaultMaterial;
    public Material selectedMaterial;

	// Use this for initialization
	void Start () {
        defaultMaterial = this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
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
    public void SetSelected (bool selected) {
        if(selected) {
            this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = selectedMaterial;
        } else {
            this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = defaultMaterial;
        }
    }
}
