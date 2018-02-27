using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JPInputController : NetworkBehaviour {
	JPNetworkPlayer networkPlayer;
	GameObject selectedShip;
	GameObject targetShip;
	Vector3 targetPosition;
    public LayerMask touchMask;
	Plane groundPlane = new Plane (new Vector3 (0, 0, 0), new Vector3 (-100, 0, -100), new Vector3 (100, 0, -100));
	GameObject marker;
	public int playerNumber = 0;
	// Use this for initialization
	void Start () {
		if(isLocalPlayer) {
			networkPlayer = this.GetComponent<JPNetworkPlayer> ();
			marker = GameObject.Find ("Marker");
			playerNumber = networkPlayer.playerNumber;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(isLocalPlayer) {
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				Debug.DrawRay (ray.origin, ray.direction * 100000, Color.yellow, 0.0f, false);
                if (Physics.Raycast (ray, out hit, Mathf.Infinity, touchMask)) {
                    Debug.Log(hit.collider.name);
					if (hit.collider.gameObject.transform.name.Contains ("Ship")) {
						if (hit.collider.gameObject.name.Contains ("Player" + networkPlayer.playerNumber)) {
                            if (selectedShip != null) {
                                selectedShip.GetComponent<JPShip>().SetSelected(false);
                            }
							selectedShip = hit.collider.gameObject;
                            selectedShip.GetComponent<JPShip>().SetSelected(true);
							selectShip (selectedShip);
						}
					}

				}
			} else if(Input.GetMouseButtonUp (0)) {
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				Debug.DrawRay (ray.origin, ray.direction * 100000, Color.yellow, 0.0f, false);
				float rayDistance;

				// Ship Targeted
				if ((Physics.Raycast (ray, out hit)) && (hit.collider.gameObject.name.Contains("Ship"))) {
					if (selectedShip != null) {
						setTargetShip (hit.collider.gameObject);
                        marker.transform.position = hit.collider.gameObject.transform.position;
                        marker.transform.rotation = selectedShip.GetComponent<JPShip>().targetRotation;
					}
				}
				// Position Targeted
				else if (groundPlane.Raycast (ray, out rayDistance)) {
					if (selectedShip != null) {
						setTargetPosition(ray.GetPoint (rayDistance));
                        marker.transform.position = ray.GetPoint(rayDistance);
                        marker.transform.rotation = selectedShip.GetComponent<JPShip>().targetRotation;
					}
				}
                if (selectedShip != null)
                {
                    selectedShip.GetComponent<JPShip>().SetSelected(false);
                }
				selectedShip = null;
				targetShip = null;

			} else if(Input.GetMouseButton (0)) {
				
				if(selectedShip) {
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					float rayDistance;
					groundPlane.Raycast (ray, out rayDistance);
					marker.transform.position = ray.GetPoint (rayDistance);
				}
			}
		}
	}
	void selectShip (GameObject ship) {
		networkPlayer.CmdSelectShip (ship.name);
		print ("Selected Ship " + ship.name);
	}
	void setTargetShip (GameObject ship) {
		networkPlayer.CmdSetTargetShip (ship.name);
		print ("Targeted GameObject " + ship.name);
	}
	void setTargetPosition (Vector3 pos) {
		networkPlayer.CmdSetPosition (pos);
		print ("Targeted position " + pos);
	}

}
