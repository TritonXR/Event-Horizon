using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallShip : MonoBehaviour {

	[SerializeField] private float FireRange = 100f;
	public Vector3 target;
	public GameObject targetShip;
	private bool movingBack;
	private int movementMode;
	Vector3 newTargetPos;

	bool savePos;
	bool overrideTarget;
	Transform obstacle;

	public Transform targetTransform;
	private Vector3 storeTarget;


	public List<Vector3> EscapeDirections = new List<Vector3>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (movementMode == 1) {
			if (Vector3.Distance (transform.position, targetShip.transform.position) > FireRange) {
				targetTransform = targetShip.transform;
				ObstacleAvoidance (targetShip.transform.forward, 0);
				transform.position = Vector3.MoveTowards (transform.position, targetTransform.position, .2f);

				var rotation = Quaternion.LookRotation (targetShip.transform.position);
				transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * 0.2f);


			} 
		} else if(movementMode == 2) {

			transform.position = Vector3.MoveTowards (transform.position, target, .2f);
			ObstacleAvoidance (target, 0);
			var rotation = Quaternion.LookRotation (target);
			//targetTransform = target;


			transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * 0.2f);
		}

		/*Vector3 dist = target - transform.position;
		if (dist.magnitude > 5 && movingBack == false) {

			Move (target);
		
		} else {
			Move (target + new Vector3 (-8, 0, 0));
			movingBack = true;

		}

		if (dist.magnitude == 4) {
			Move (target);
		}*/

	}



	RaycastHit[] Rays(Vector3 direction, float offSetX){
	
		Ray ray = new Ray (transform.position + new Vector3 (offSetX, 0, 0), direction);
	
		float distanceToLookAhead = 10f;
		RaycastHit[] hits = Physics.SphereCastAll (ray, 5, distanceToLookAhead);
		for (int i = 0; i < hits.Length; i++) {
			Debug.Log (hits [i].collider.gameObject.name);
		}

		return hits;


	}

	void ObstacleAvoidance(Vector3 direction, float offSetX){

		RaycastHit[] hit = Rays (direction, offSetX);

		for (int i = 0; i < hit.Length - 1; i++) {
			if (hit [i].collider.gameObject != transform.root.gameObject) {
				if (!savePos) {
					if (movementMode == 1) {
						storeTarget = targetTransform.position;
					} else {
						storeTarget = target;
					}
					obstacle = hit [i].transform;
					savePos = true;
				}

				FindEscapeDirections (hit [i].collider);
			}
		}

		if (EscapeDirections.Count > 0) {
		
			if (!overrideTarget) {
			
				newTargetPos = getClosests();
				overrideTarget = true;
			}
		
		}

		float distance = 0f;
		if (movementMode == 1) {
			distance = Vector3.Distance (transform.position, targetTransform.position);
		} else {
			distance = Vector3.Distance (transform.position, target);
		}

		if (distance < 2) {
			if (savePos) {
				if (movementMode == 1) {
					targetTransform.position = storeTarget;
				} else {
					target = storeTarget;
				}
				savePos = false;
			}

			overrideTarget = false;
			EscapeDirections.Clear ();
		} /*else {
			if (movementMode == 1) {
				targetTransform.position = newTargetPos;
			} else {
				target = newTargetPos;
			}
		}*/


	}


	Vector3 getClosests(){
		Vector3 close = EscapeDirections [0];
		float distance = Vector3.Distance (transform.position, EscapeDirections [0]);

		foreach (Vector3 dir in EscapeDirections) {
			float tempDistance = Vector3.Distance (transform.position, dir);

			if (tempDistance < distance) {
				distance = tempDistance;
				close = dir;
			}
		}

		return close;
	}

	void FindEscapeDirections(Collider col){
	
		RaycastHit hitUp;

		if (Physics.Raycast (col.transform.position, col.transform.up, out hitUp, col.bounds.extents.y * 2 + 1)) {
		} else {

			Vector3 dir = col.transform.position + new Vector3 (0, col.bounds.extents.y * 2 + 1,0);
			if(!EscapeDirections.Contains(dir)){
				EscapeDirections.Add(dir);
			}
	
		}

		RaycastHit hitDown;
		if (Physics.Raycast (col.transform.position, -col.transform.up, out hitUp, col.bounds.extents.y * 2 -1)) {
		} else {

			Vector3 dir = col.transform.position + new Vector3 (0, -col.bounds.extents.y * 2 - 1,0);
			if(!EscapeDirections.Contains(dir)){
				EscapeDirections.Add(dir);
			}

		}

		RaycastHit hitRight;
		if (Physics.Raycast (col.transform.position, col.transform.right, out hitUp, col.bounds.extents.x * 2 -1)) {
		} else {

			Vector3 dir = col.transform.position + new Vector3 (col.bounds.extents.x * 2 +1,0,0);
			if(!EscapeDirections.Contains(dir)){
				EscapeDirections.Add(dir);
			}

		}

		RaycastHit hitLeft;
		if (Physics.Raycast (col.transform.position, -col.transform.right, out hitUp, col.bounds.extents.x * 2 -1)) {
		} else {

			Vector3 dir = col.transform.position + new Vector3 (-col.bounds.extents.x * 2 - 1,0,0);
			if(!EscapeDirections.Contains(dir)){
				EscapeDirections.Add(dir);
			}

		}
	
	}

	public void setTargetShip(GameObject ship){

		movementMode = 1;
		targetShip = ship;
	
	}

	public void setTargetPosition(Vector3 vector){


		movementMode = 2;
		target = vector;
	
	}
}
