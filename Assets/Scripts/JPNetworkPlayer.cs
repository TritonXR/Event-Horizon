using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
    public string spawnControl = "1";
    string shipSpawnList = "1,1,2,2,2";
    public int[] shipSpawnCommandList;

    [SyncVar]
    public int fleetHealth = 0;
    bool fleetActive = false;

	JPNetworkHostManager gameManagerHost;
	// Use this for initialization
	void Start () {
        

        if(isLocalPlayer) {
            //PlayerPrefs.SetString("SpawnList", "1,1,2,2,3");
            //print("Player id: " + Network.player.ToString());
            this.name = "LocalPlayer";
            JPUIController.OnSelectTeamOne += SetTeamOne;
            JPUIController.OnSelectTeamTwo += SetTeamTwo;
            JPNetworkHostManager.OnGameOver += ProcessGameOver;
            GameObject.Find("ServerIP").GetComponent<Text>().text = Network.player.ipAddress;//NetworkManager.singleton.networkAddress;
            Debug.Log("Sending ship string: " + PlayerPrefs.GetString("SpawnList"));
            CmdSetShipSpawnString(spawnControl);
            //CmdSetShipSpawnString(PlayerPrefs.GetString("SpawnList"));
        }
		if(isServer) {
            
		}
	}
	
	// Update is called once per frame
	void Update () {
        //if(fleetActive) {
            //if(fleetHealth <= 0) {
                //RpcSetGameOver(false);
            //}
        //}
	}
    [Command]
    void CmdspawnShips(int team) {
        playerTeam = team;

        Transform spawnLoc = GameObject.Find("SpawnTeam" + team).transform;
        gameManagerHost = GameObject.Find("NetworkManager").GetComponent<JPNetworkHostManager>();
        playerNumber = gameManagerHost.getPlayerCount();
        gameManagerHost.incrementPlayerCount(playerNumber);
        RpcSetPlayerNumber(playerNumber);
        spawnedShipList = new GameObject[shipSpawnCommandList.Length];
        Vector3 initPos = spawnLoc.transform.position;
        if(spawnLoc.transform.position.z > 0) {
            initPos.z += 100f;
        } else {
            initPos.z -= 100f;
        }
        for (int count = 0; count < shipSpawnCommandList.Length; count++)
        {

            //Spawn lead ships
            GameObject obj = (GameObject)Instantiate(shipList[shipSpawnCommandList[count]], new Vector3(count * 100f, 12.0f, Random.Range(0, 0)) + initPos, spawnLoc.transform.rotation);
            obj.GetComponent<JPShip>().JumpToLocation(new Vector3(count * 100f, 12.0f, Random.Range(0, 0)) + spawnLoc.transform.position, true);
            if (obj.GetComponent<JPNetworkShip>().forcePlayerNumber)
            {
                obj.name = "Player" + "1" + "Ship" + count;
            }
            else
            {
                obj.name = "Player" + playerNumber + "Ship" + count + "Squad0";
            }
            GameObject[] squadShips = new GameObject[obj.GetComponent<JPShip>().wingmen.Length];

            JPShip lead;
            lead = obj.GetComponent<JPShip>();
            obj.GetComponent<JPShip>().playerController = this;

            fleetHealth += obj.GetComponent<JPShip>().shipValue;
            lead.leadController = lead;
            squadShips[0] = obj;



            //Spawn wingmen
            for (int countSquad = 1; countSquad < squadShips.Length; countSquad++) {
                GameObject obj2 = (GameObject)Instantiate(shipList[shipSpawnCommandList[count]], new Vector3(count * 100f, 12.0f, Random.Range(0, 0)) + initPos + lead.wingmenOffsets[countSquad], spawnLoc.transform.rotation);
                obj2.GetComponent<JPShip>().JumpToLocation(new Vector3(count * 100f, 12.0f, Random.Range(0, 0)) + spawnLoc.transform.position + lead.wingmenOffsets[countSquad], true);

                obj2.name = "Player" + playerNumber + "Ship" + count + "Squad" + countSquad;
                obj2.GetComponent<JPShip>().leadController = lead;
                obj2.GetComponent<JPShip>().lead = false;
                obj2.GetComponent<JPShip>().playerController = this;
                fleetHealth += obj.GetComponent<JPShip>().shipValue;
                squadShips[countSquad] = obj2;
                obj2.GetComponent<JPFighter>().squadNum = countSquad;
            }
            lead.wingmen = squadShips;


            for (int countSpawn = 0; countSpawn < squadShips.Length; countSpawn++)
            {
                //NetworkServer.Spawn(obj);
                NetworkServer.SpawnWithClientAuthority(squadShips[countSpawn], connectionToClient);

                if (squadShips[countSpawn].GetComponent<JPNetworkShip>().forcePlayerNumber)
                {
                    squadShips[countSpawn].GetComponent<JPNetworkShip>().RpcSetName("Player" + "1" + "Ship" + count);
                }
                else
                {
                    squadShips[countSpawn].GetComponent<JPNetworkShip>().RpcSetName("Player" + playerNumber + "Ship" + count + "Squad" + countSpawn);
                }
                squadShips[countSpawn].GetComponent<JPNetworkShip>().gamePlayerNumber = playerNumber;
                spawnedShipList[count] = obj;
                if (squadShips[countSpawn].GetComponent<JPNetworkShip>().localPlayer)
                {
                    GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
                }
                squadShips[countSpawn].GetComponent<JPNetworkShip>().teamNumber = team;

            }
            for (int countWingmen = 0; countWingmen < squadShips.Length; countWingmen ++) {
                squadShips[countWingmen].GetComponent<JPShip>().RpcSetLeadController(squadShips[0].name, playerNumber, count, squadShips.Length);
            }

            fleetActive = true;
            gameManagerHost.teamHealth[playerTeam] += fleetHealth;
            gameManagerHost.teamActive[playerTeam] = true;

      }

    }
    public int ParseCommandString(int min) {
        string str = shipSpawnList.Substring(min);
        int pos = str.IndexOf(',');


        return int.Parse(str.Substring(0, pos));
    }
    public void InitializeShipSpawnList () {
        shipSpawnCommandList = new int[CountShipSpawn()];
        int minSearch = 0;
        int arrayCounter = 0;
        for (int count = 0; count < shipSpawnList.Length; count++)
        {
            if (shipSpawnList.Substring(count, 1).Equals(","))
            {
                print(shipSpawnList.Substring(minSearch, (count - minSearch)));
                shipSpawnCommandList[arrayCounter] = int.Parse(shipSpawnList.Substring(minSearch, (count - minSearch)));
                arrayCounter++;
                minSearch = count + 1;
            }
        }
        //print(shipSpawnList.Substring(minSearch, (shipSpawnList.Length - minSearch)));
        shipSpawnCommandList[arrayCounter] = int.Parse(shipSpawnList.Substring(minSearch, (shipSpawnList.Length - minSearch)));
    }
    public int CountShipSpawn()
    {
        int commaCount = 0;
        for (int count = 0; count < shipSpawnList.Length; count ++) {
            if(shipSpawnList.Substring(count, 1).Equals(",")) {
                commaCount++;
            }
        }
        return commaCount + 1;
    }


    void SetTeamOne()
    {
        CmdspawnShips(1);
        GameObject.Find("SelectTeamOne").SetActive(false);
        GameObject.Find("SelectTeamTwo").SetActive(false);
    }
    void SetTeamTwo()
    {
        CmdspawnShips(2);
        GameObject.Find("SelectTeamOne").SetActive(false);
        GameObject.Find("SelectTeamTwo").SetActive(false);
    }


    public void DecrementFleetHealth (int amt) {
        fleetHealth -= amt;
        gameManagerHost.teamHealth[playerTeam] -= amt;
    }


    [Command]
    public void CmdSetShipSpawnString(string shipString)
    {
        Debug.Log("Server recieved ship string: " + shipString);
        shipSpawnList = shipString;
        InitializeShipSpawnList();
    }
	[Command]
	public void CmdSelectShip (string shipName) {
		selectedShip = GameObject.Find (shipName);
		//print("Ship name recieved by server" + shipName);
	}
	[Command]
	public void CmdSetTargetShip (string shipName) {
		targetShip = GameObject.Find (shipName);
        selectedShip.GetComponent<JPShip>().leadController.SetTargetShip(targetShip);
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
        pos.y = 14f;
        selectedShip.GetComponent<JPShip>().leadController.SetTargetPosition(pos);
		/*if (selectedShip.GetComponent<CapitalShip> () == null) {
			selectedShip.GetComponent<SmallShip> ().SetTargetPosition (pos);
		} else {
			selectedShip.GetComponent<CapitalShip> ().SetTargetPosition (pos);
		}*/
		//print("Insert code to set position here");
	}

    [Command]
    public void CmdFireVRShip (string shipName)
    {
        GameObject.Find(shipName).GetComponent<JPPilot>().ShootVRLaser();
    }


    [Command]
    public void CmdSetSkill (int skillNum) {
        selectedShip.GetComponent<JPShip>().TriggerSkill(skillNum);
    }
    [Command]
    public void CmdSetSkillLocation(int skillNum, Vector3 pos)
    {
        selectedShip.GetComponent<JPShip>().TriggerSkillLocation(skillNum, pos);
    }
    [Command]
    public void CmdSetSkillTarget(int skillNum, GameObject target)
    {
        selectedShip.GetComponent<JPShip>().TriggerSkillTarget(skillNum, target);
    }



    [Command]
    public void CmdSetMode(int mode)
    {
        selectedShip.GetComponent<JPShip>().leadController.SetMode(mode);
        print("Setting mode " + mode);
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

    void ProcessGameOver (int num) {
        RpcSetGameOver(num);
    }

    [ClientRpc]
    void RpcSetGameOver(int defeatedNum)
    {
        GameObject.Find("UICanvas").GetComponent<JPUIController>().SetGameOver(defeatedNum != playerTeam);

    }

    [Command]
    public void CmdRetreat()
    {
        for (int count = 0; count < spawnedShipList.Length; count ++) {
            JPShip temp = spawnedShipList[count].GetComponent<JPShip>();
            for (int countInner = 0; countInner < temp.wingmen.Length; countInner ++) {
                temp.wingmen[countInner].GetComponent<JPShip>().JumpToLocation(new Vector3(1000f, 0f, 10000f), false);
                if(!temp.wingmen[countInner].GetComponent<JPShip>().destroyed) {
                    DecrementFleetHealth(temp.wingmen[countInner].GetComponent<JPShip>().shipValue);
                }
            }

        }
    }


}
