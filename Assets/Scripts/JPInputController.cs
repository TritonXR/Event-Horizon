using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JPInputController : NetworkBehaviour {
	JPNetworkPlayer networkPlayer;
	GameObject selectedShip;
	GameObject targetShip;
	Vector3 targetPosition;
	// Use this for initialization
	void Start () {
		if(isLocalPlayer) {
			networkPlayer = this.GetComponent<JPNetworkPlayer> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(isLocalPlayer) {

		}
	}
	void selectShip (GameObject ship) {
		networkPlayer.CmdSelectShip (ship.name);
	}
	void setTargetShip (GameObject ship) {
		networkPlayer.CmdSetTargetShip (ship.name);
	}
	void setTargetPosition (Vector3 pos) {
		networkPlayer.CmdSetPosition (pos);
	}

}
