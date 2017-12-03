using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JPNetworkPlayer : NetworkBehaviour {
	GameObject selectedShip;
	GameObject targetShip;
	Vector3 targetPosition;

	public int playerNumber;
	public GameObject[] shipList;
	public GameObject[] spawnedShipList;
	// Use this for initialization
	void Start () {
		if(isServer) {
			/*spawnedShipList = new GameObject[shipList.Length];
			for(int count = 0; count < shipList.Length; count ++) {
				GameObject obj = (GameObject)Instantiate (shipList [count], new Vector3 (Random.Range(-1f,1f),0, Random.Range(-1f,1f)), transform.rotation);
				obj.name = "Player" + playerNumber + "Ship" + count;
				NetworkServer.Spawn (obj);
				obj.GetComponent<JPNetworkShip> ().RpcSetName ("Player" + playerNumber + "Ship" + count);
				spawnedShipList [count] = obj;
			}*/

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[Command]
	public void CmdSelectShip (string shipName) {
		selectedShip = GameObject.Find (shipName);
		print("Ship name recieved by server" + shipName);
	}
	[Command]
	public void CmdSetTargetShip (string shipName) {
		targetShip = GameObject.Find (shipName);
		if (selectedShip.GetComponent<CapitalShip> () == null) {
			selectedShip.GetComponent<SmallShip> ().setTargetShip (targetShip);
		} else {
			selectedShip.GetComponent<CapitalShip> ().setTargetShip (targetShip);
		}
		//print("Insert code to set target here");
	}
	[Command]
	public void CmdSetPosition (Vector3 pos) {
		targetPosition = pos;
		if (selectedShip.GetComponent<CapitalShip> () == null) {
			selectedShip.GetComponent<SmallShip> ().setTargetPosition (pos);
		} else {
			selectedShip.GetComponent<CapitalShip> ().setTargetPosition (pos);
		}
		//print("Insert code to set position here");
	}
}
