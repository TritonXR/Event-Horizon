using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitalShip : JPShip {

	/*[SerializeField] public GameObject target;
	[SerializeField] private float FireRange = 5f;
	public int idleMode = 0;
	public float moveSpeed = 0.01f;
	public float turnSpeed = 0.2f;

	public Vector3 targetVector;
	public int movementMode = 0; //0 = stop 1 = target ship 2 = vector
	*/

	// Use this for initialization
	void Start () {
		//target = GameObject.Find ("DefaultTarget");
        defaultMaterial = this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {

		/*if (target == null &&  targetVector != null) {
			transform.position = Vector3.MoveTowards(transform.position, targetVector, .04f);
		} else if(target != null) {
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), 1f * Time.deltaTime);
		}*/

		if (movementMode == 1) {
            if (Vector3.Distance (transform.position, target.transform.position) > fireDist) {
				var rotation = Quaternion.LookRotation (target.transform.position);
				if ((rotation.eulerAngles.y - 0.5f<transform.rotation.eulerAngles.y)&&(transform.rotation.eulerAngles.y<rotation.eulerAngles.y + 0.5f)) {
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
			if ((rotation.eulerAngles.y - 0.5f<transform.rotation.eulerAngles.y)&&(transform.rotation.eulerAngles.y<rotation.eulerAngles.y + 0.5f)) {
				//if (transform.position == target.transform.position) {

				//} else {
					transform.position = Vector3.MoveTowards (transform.position, targetVector, moveSpeed);
				//}
			} else {
				transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * turnSpeed);
			}

		}

	}

	public override void SetTargetShip(GameObject ship){
		
        base.SetTargetShip(ship);
		print ("Recieved target "+this.target.name);
	}

	public override void SetTargetPosition(Vector3 vector){
        base.SetTargetPosition(vector);
		
	}


}
