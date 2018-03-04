using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JPNetworkPlayer : NetworkBehaviour {
	GameObject selectedShip;
	GameObject targetShip;
	Vector3 targetPosition;
    [SyncVar]
	public int playerNumber = 0;
	public GameObject[] shipList;
	public GameObject[] spawnedShipList;

	JPNetworkHostManager gameManagerHost;
	// Use this for initialization
	void Start () {
        if(isLocalPlayer) {
            //print("Player id: " + Network.player.ToString());
            this.name = "LocalPlayer";
        }
		if(isServer) {
			gameManagerHost = GameObject.Find ("NetworkManager").GetComponent<JPNetworkHostManager> ();
			playerNumber = gameManagerHost.getPlayerCount();
			gameManagerHost.incrementPlayerCount (playerNumber);
			RpcSetPlayerNumber (playerNumber);
			spawnedShipList = new GameObject[shipList.Length];
			for(int count = 0; count < shipList.Length; count ++) {
                GameObject obj = (GameObject)Instantiate(shipList[count], new Vector3(Random.Range(0, 0), 0.1f, Random.Range(0, 0)), transform.rotation);
                if (obj.GetComponent<JPNetworkShip>().forcePlayerNumber)
                {
                    obj.name = "Player" + "1" + "Ship" + count;
                }
                else
                {
                    obj.name = "Player" + playerNumber + "Ship" + count;
                }

                //NetworkServer.Spawn(obj);
                NetworkServer.SpawnWithClientAuthority(obj, connectionToClient);

                if (obj.GetComponent<JPNetworkShip>().forcePlayerNumber)
                {
                    obj.GetComponent<JPNetworkShip>().RpcSetName("Player" + "1" + "Ship" + count);
                }
                else
                {
                    obj.GetComponent<JPNetworkShip>().RpcSetName("Player" + playerNumber + "Ship" + count);
                }
                obj.GetComponent<JPNetworkShip>().gamePlayerNumber = playerNumber;
                spawnedShipList[count] = obj;
                if(obj.GetComponent<JPNetworkShip>().localPlayer) {
                    GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
                }
			}

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
			selectedShip.GetComponent<SmallShip> ().SetTargetShip (targetShip);
		} else {
			selectedShip.GetComponent<CapitalShip> ().SetTargetShip (targetShip);
		}
		//print("Insert code to set target here");
	}
	[Command]
	public void CmdSetPosition (Vector3 pos) {
		targetPosition = pos;
		if (selectedShip.GetComponent<CapitalShip> () == null) {
			selectedShip.GetComponent<SmallShip> ().SetTargetPosition (pos);
		} else {
			selectedShip.GetComponent<CapitalShip> ().SetTargetPosition (pos);
		}
		//print("Insert code to set position here");
	}
	[ClientRpc]
	void RpcSetPlayerNumber (int playerNum) {
		playerNumber = playerNum;
	}
    [Command]
    void CmdSpawnControlShip () {
        print("Spawning control ship on server" + NetworkServer.active); 
        if (shipList[0].name.Contains("Single"))
        {

            GameObject obj = (GameObject)Instantiate(shipList[0], new Vector3(Random.Range(0, 0), 0.1f, Random.Range(0, 0)), transform.rotation);
            if (obj.GetComponent<JPNetworkShip>().forcePlayerNumber)
            {
                obj.name = "Player" + "1" + "Ship" + 0;
            }
            else
            {
                obj.name = "Player" + playerNumber + "Ship" + 0;
            }

            NetworkServer.SpawnWithClientAuthority(obj, connectionToClient);


            spawnedShipList[0] = obj;
        }
    }
}
