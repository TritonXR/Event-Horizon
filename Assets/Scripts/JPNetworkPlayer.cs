﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JPNetworkPlayer : NetworkBehaviour {
	public GameObject selectedShip;
	GameObject targetShip;
	Vector3 targetPosition;
    [SyncVar]
	public int playerNumber = 0;
    [SyncVar]
    public int playerTeam = 0;
	public GameObject[] shipList;
	public GameObject[] spawnedShipList;

	JPNetworkHostManager gameManagerHost;
	// Use this for initialization
	void Start () {
        if(isLocalPlayer) {
            //print("Player id: " + Network.player.ToString());
            this.name = "LocalPlayer";
            JPUIController.OnSelectTeamOne += SetTeamOne;
            JPUIController.OnSelectTeamTwo += SetTeamTwo;

        }
		if(isServer) {
            
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    [Command]
    void CmdspawnShips(int team) {
        playerTeam = team;
        Transform spawnLoc = GameObject.Find("SpawnTeam" + team).transform;
        gameManagerHost = GameObject.Find("NetworkManager").GetComponent<JPNetworkHostManager>();
        playerNumber = gameManagerHost.getPlayerCount();
        gameManagerHost.incrementPlayerCount(playerNumber);
        RpcSetPlayerNumber(playerNumber);
        spawnedShipList = new GameObject[shipList.Length];
        for (int count = 0; count < shipList.Length; count++)
        {
            GameObject obj = (GameObject)Instantiate(shipList[count], new Vector3(count * 20f, 12.0f, Random.Range(0, 0)) + spawnLoc.transform.position, spawnLoc.transform.rotation);
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
            if (obj.GetComponent<JPNetworkShip>().localPlayer)
            {
                GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            }
            obj.GetComponent<JPNetworkShip>().teamNumber = team;

        }

    }
	[Command]
	public void CmdSelectShip (string shipName) {
		selectedShip = GameObject.Find (shipName);
		print("Ship name recieved by server" + shipName);
	}
	[Command]
	public void CmdSetTargetShip (string shipName) {
		targetShip = GameObject.Find (shipName);
        selectedShip.GetComponent<JPShip>().SetTargetShip(targetShip);
		/*if (selectedShip.GetComponent<CapitalShip> () == null) {
			selectedShip.GetComponent<SmallShip> ().SetTargetShip (targetShip);
		} else {
			selectedShip.GetComponent<CapitalShip> ().SetTargetShip (targetShip);
		}*/
		//print("Insert code to set target here");
	}
	[Command]
	public void CmdSetPosition (Vector3 pos) {
		targetPosition = pos;
        selectedShip.GetComponent<JPShip>().SetTargetPosition(pos);
		/*if (selectedShip.GetComponent<CapitalShip> () == null) {
			selectedShip.GetComponent<SmallShip> ().SetTargetPosition (pos);
		} else {
			selectedShip.GetComponent<CapitalShip> ().SetTargetPosition (pos);
		}*/
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

    void SetTeamOne() {
        CmdspawnShips(1);
        GameObject.Find("SelectTeamOne").SetActive(false);
        GameObject.Find("SelectTeamTwo").SetActive(false);
    }
    void SetTeamTwo() {
        CmdspawnShips(2);
        GameObject.Find("SelectTeamOne").SetActive(false);
        GameObject.Find("SelectTeamTwo").SetActive(false);
    }
}
