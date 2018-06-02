using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPNetworkHostManager : MonoBehaviour {
	public int playerNumber;
    public int teamThreshold = 10;
    public int[] teamHealthMax = new int[3];
    public int[] teamHealth = new int[3];
    public bool[] teamActive = new bool[3];

    public delegate void gameOverMsg(int defeatedTeam);
    public static event gameOverMsg OnGameOver;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
        //print(playerNumber);
        for (int count = 1; count < teamHealth.Length; count ++) {
            if((teamActive[count]) && (teamHealth[count] <= teamThreshold)) {
                print("Team " + count + "Eliminated!");
                OnGameOver(count);
            }
        }
		
	}
	public int getPlayerCount () {
		return playerNumber;
	}
	public void incrementPlayerCount (int playerNum) {
		//if(playerNum == playerNumber) {
		//print ("Increment");
			playerNumber++;
		//}
	}



   
}
