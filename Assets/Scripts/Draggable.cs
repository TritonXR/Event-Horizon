using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour {

	bool selected = false;
	// Use this for initialization

	/*public void OnBeginDrag (PointerEventData eventData)
	{
		Debug.Log("Began to drag");
	}

	public void OnDrag (PointerEventData eventData)
	{
		
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		Collider[] hitColliders = Physics.OverlapSphere(eventData.position, 1f);

		if (hitColliders.Length != 0) {
			this.GetComponent<CapitalShip>().target = hitColliders[0].gameObject; 
		} else {
			this.GetComponent<CapitalShip> ().targetVector = new Vector3 (eventData.position.x,eventData.position.y, 0);
		}


 	}*/

	void Start(){

	}

	void Update(){

	

		foreach (Touch touch in Input.touches) {
			RaycastHit hit;

			if (touch.phase == TouchPhase.Began) {

				var ray = Camera.main.ScreenPointToRay (touch.position);
				if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
					Debug.Log (hit.transform.gameObject);
					if (hit.transform.gameObject == this.gameObject) {
						Debug.Log ("Selected");
						selected = true;
					}
				}
			}
			


			else if (touch.phase == TouchPhase.Ended) {
				Debug.Log ("Hello");
				// Construct a ray from the current touch coordinates
				if (selected) {
					var ray = Camera.main.ScreenPointToRay (touch.position);
					if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
						// Create a particle if hit
						Debug.Log (hit.transform.gameObject);
						this.GetComponent<CapitalShip> ().target = hit.transform.gameObject;
					} else {
						this.GetComponent<CapitalShip> ().target = null;
						this.GetComponent<CapitalShip> ().targetVector = Camera.main.ScreenToWorldPoint (new Vector3(touch.position.y, 0, touch.position.x));
						//this.GetComponent<CapitalShip> ().targetVector = new Vector3(touch.position.x, 0, touch.position.y);
					}
				}

				selected = false;
			}


		}

	}

}
