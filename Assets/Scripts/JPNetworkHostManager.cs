using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPNetworkHostManager : MonoBehaviour {
	public int playerNumber;
    public delegate void uiSetMode();
    public static event uiSetMode OnModeCancel;
    public static event uiSetMode OnModeMove;
    public static event uiSetMode OnModeTarget;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
        //print(playerNumber);
		
	}
	public int getPlayerCount () {
		return playerNumber;
	}
	public void incrementPlayerCount (int playerNum) {
		//if(playerNum == playerNumber) {
		print ("Increment");
			playerNumber++;
		//}
	}
    public void setModeCancel()
    {
        print("Cancel");
        OnModeCancel();
    }
    public void setModeMove()
    {
        print("Move");
        OnModeMove();
    }
    public void setModeTarget()
    {
        print("Target");
        OnModeTarget();
    }
   
}
