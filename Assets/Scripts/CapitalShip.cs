using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitalShip : MonoBehaviour {

	[SerializeField] public GameObject target;
	[SerializeField] private float FireRange = 5f;
	public int idleMode = 0;
	public float moveSpeed = 0.01f;
	public float turnSpeed = 0.2f;

	public Vector3 targetVector;
	public int movementMode = 0; //0 = stop 1 = target ship 2 = vector

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

		if (movementMode == 1) {
			if (Vector3.Distance (transform.position, target.transform.position) > FireRange) {
				var rotation = Quaternion.LookRotation (target.transform.position);
				if ((rotation.eulerAngles.y - 10.0f<transform.rotation.eulerAngles.y)&&(transform.rotation.eulerAngles.y<rotation.eulerAngles.y + 10.0f)) {
					//if (transform.position == target.transform.position) {
						
					//} else {
						transform.position = Vector3.MoveTowards (transform.position, target.transform.position, moveSpeed);
					//}
				} else {
					transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * turnSpeed);
				}

			}
		} else if(movementMode == 2) {
			var rotation = Quaternion.LookRotation (targetVector);
			if ((rotation.eulerAngles.y - 10.0f<transform.rotation.eulerAngles.y)&&(transform.rotation.eulerAngles.y<rotation.eulerAngles.y + 10.0f)) {
				//if (transform.position == target.transform.position) {

				//} else {
					transform.position = Vector3.MoveTowards (transform.position, targetVector, moveSpeed);
				//}
			} else {
				transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * turnSpeed);
			}

		}

	}

	public void setTargetShip(GameObject ship){
		this.target = ship;
		movementMode = 1;
		print ("Recieved target "+this.target.name);
	}

	public void setTargetPosition(Vector3 vector){
		this.targetVector = vector;
		movementMode = 2;
	}


}
