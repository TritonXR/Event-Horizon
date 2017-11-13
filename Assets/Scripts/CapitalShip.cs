using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitalShip : MonoBehaviour {

	[SerializeField] public GameObject target;
	[SerializeField] private float FireRange = 5f;

	public Vector3 targetVector;

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("DefaultTarget");
	}
	
	// Update is called once per frame
	void Update () {

		/*if (target == null &&  targetVector != null) {
			transform.position = Vector3.MoveTowards(transform.position, targetVector, .04f);
		} else if(target != null) {
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), 1f * Time.deltaTime);
		}*/

		if (target != null) {
			if (Vector3.Distance (transform.position, target.transform.position) > FireRange) {
				transform.position = Vector3.MoveTowards (transform.position, target.transform.position, .01f);
				var rotation = Quaternion.LookRotation (target.transform.position);
				transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * 0.2f);
			}
		} else if(targetVector != null) {
			
			transform.position = Vector3.MoveTowards (transform.position, targetVector, .02f);
		}

	}

	public void setTargetShip(GameObject ship){
		this.target = ship;
		print ("Recieved target "+this.target.name);
	}

	public void setTargetPosition(Vector3 vector){
		this.targetVector = vector;
	}


}
