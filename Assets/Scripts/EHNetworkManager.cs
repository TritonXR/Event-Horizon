using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EHNetworkManager : NetworkManager {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnClientError(NetworkConnection conn, int errorCode)
    {
        print("ClientError");
        print(conn);
        print(errorCode);
    }
}
