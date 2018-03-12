using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JPNetworkShip : NetworkBehaviour {
    [SyncVar]
    public int gamePlayerNumber;
    [SyncVar]
    public int clientPlayerNumber;
    [SyncVar]
    public int teamNumber;
	public bool forcePlayerNumber = false;
	public int playerNumber = 0;
    public bool localPlayer = false;
	// Use this for initialization
	void Start () {
        clientPlayerNumber = GameObject.Find("LocalPlayer").GetComponent<JPNetworkPlayer>().playerNumber;

        if((localPlayer) && (gamePlayerNumber == clientPlayerNumber)) {
            GameObject singlePlayerControl = GameObject.Find("SinglePlayerController");

            singlePlayerControl.GetComponent<JPControlShip>().Activate();

            //transform.parent = singlePlayerControl.transform;
            //transform.position = Vector3.zero;
            GetComponent<JPPilot>().activate((singlePlayerControl));
        }
	}
	
	// Update is called once per frame
	void Update () {
        //GetComponent<TextMesh>().text = "ShipID: " + gamePlayerNumber + " ClientID: " + clientPlayerNumber + "\n\n";
	}
	[ClientRpc]
	public void RpcSetName (string name) {
		this.gameObject.name = name;
	}

}
