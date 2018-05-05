using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JPInputController : NetworkBehaviour {
	JPNetworkPlayer networkPlayer;
	GameObject selectedShip;
	GameObject targetShip;
	Vector3 targetPosition;
    public LayerMask touchMask;
	Plane groundPlane = new Plane (new Vector3 (0, 0, 0), new Vector3 (-100, 0, -100), new Vector3 (100, 0, -100));
	GameObject marker;
	public int playerNumber = 0;
    public int targetMode = 0;
    JPUIController localUI;
    Slider healthSlider;
	// Use this for initialization
	void Start () {
        if (!isLocalPlayer)
        {
            return;
        }
        //healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
		networkPlayer = this.GetComponent<JPNetworkPlayer> ();
		marker = GameObject.Find ("Marker");
		playerNumber = networkPlayer.playerNumber;
        JPNetworkHostManager localHost = GameObject.Find("NetworkManager").GetComponent<JPNetworkHostManager>();
        JPUIController.OnModeCancel += setModeCancel;
        JPUIController.OnModeMove += setModeMove;
        JPUIController.OnModeTarget += setModeTarget;
        localUI = GetComponent<JPUIController>();
        marker.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            //print("Not local player!");
            return;
        }
        //print("LOCAL UI CONTROLLER " + localUI.targetMode);
		if (Input.GetMouseButtonDown (0)) {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked on the UI");
            }
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			Debug.DrawRay (ray.origin, ray.direction * 100000, Color.yellow, 0.0f, false);
            if (Physics.Raycast (ray, out hit, Mathf.Infinity, touchMask)) {
                Debug.Log(hit.collider.name);
				if (hit.collider.gameObject.transform.name.Contains ("Ship")) {
                    if (hit.collider.gameObject.name.Contains ("Player" + networkPlayer.playerNumber)) { 
                        if (selectedShip != null) {
                            selectedShip.GetComponent<JPShip>().leadController.SetSelected(false);
                        }
                        if(hit.collider.gameObject.name.Contains("Fighter")) {
                            //Debug.Log("Got Squad lead " + hit.collider.transform.parent.name);
                            selectedShip = hit.collider.transform.parent.gameObject;
                            selectedShip.GetComponent<JPShip>().leadController.SetSelected(true);
                            selectShip(selectedShip);
                        } else {
                            selectedShip = hit.collider.gameObject;
                            selectedShip.GetComponent<JPShip>().leadController.SetSelected(true);
                            selectShip(selectedShip);
                        }
                        marker.SetActive(true);
                        //healthSlider.gameObject.SetActive(true);
                        //healthSlider.value = selectedShip.GetComponent<JPShip>().healthPercent;
					}
				}
            } else {
                if (selectedShip != null)
                {
                    selectedShip.GetComponent<JPShip>().leadController.SetSelected(false);
                }
                selectedShip = null;
                targetShip = null;
                marker.SetActive(false);
                //healthSlider.gameObject.SetActive(false);
            }
		} else if(Input.GetMouseButtonUp (0)) {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked Up on the UI");
            }
            else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 100000, Color.yellow, 0.0f, false);
                float rayDistance;

                // Ship Targeted
                if (targetMode == 1)
                {
                    if ((Physics.Raycast(ray, out hit)) && (hit.collider.gameObject.name.Contains("Ship")))
                    {
                        if (selectedShip != null)
                        {
                            setTargetShip(hit.collider.gameObject);
                            marker.transform.position = hit.collider.gameObject.transform.position;
                            //marker.transform.rotation = selectedShip.GetComponent<JPShip>().targetRotation;
                        }
                    }
                }
                else if (targetMode == 0)
                {
                    // Position Targeted
                    if (groundPlane.Raycast(ray, out rayDistance))
                    {
                        if (selectedShip != null)
                        {
                            setTargetPosition(ray.GetPoint(rayDistance));
                            marker.transform.position = ray.GetPoint(rayDistance);
                            //marker.transform.rotation = selectedShip.GetComponent<JPShip>().targetRotation;
                        }
                    }
                }
            }
            if (selectedShip != null)
            {
                //selectedShip.GetComponent<JPShip>().leadController.SetSelected(false);
            }
            //selectedShip = null;
            //targetShip = null;
            //marker.SetActive(false);
            //healthSlider.gameObject.SetActive(false);



		} else if(Input.GetMouseButton (0)) {
				
			if(selectedShip) {
				//RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				float rayDistance;
				groundPlane.Raycast (ray, out rayDistance);
				marker.transform.position = ray.GetPoint (rayDistance);
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
        pos.y = 14.0f;
		networkPlayer.CmdSetPosition (pos);
		print ("Targeted position " + pos);
	}


    public void setModeMove () {
        //print("Mode Move");

        if (!isLocalPlayer)
        {
            return;
        }
        targetMode = 0;
        print("Mode Move 0");

    }
    public void setModeTarget()
    {
        //print("Mode Target ");

        if (!isLocalPlayer)
        {
            return;
        }
        targetMode = 1; 
        print("Mode Target 1");

    }
    public void setModeCancel () {
        

        if (!isLocalPlayer)
        {
            print("Not local player!");
            return;
        }
        print("Local player! Cancel");
        if (selectedShip != null)
        {
            selectedShip.GetComponent<JPShip>().SetSelected(false);
        }
        selectedShip = null;
        targetShip = null;
        print("Mode Cancel " + targetMode);
        targetMode = 0;
        marker.SetActive(false);

    }


}