using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPNetworkHostManager : MonoBehaviour {
	public int playerNumber;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}
	public int getPlayerCount () {
		return playerNumber;
	}
	public void incrementPlayerCount (int playerNum) {
		if(playerNum == playerNumber) {
			playerNum++;
		}
	}
}
