using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JPUINetworkManager : MonoBehaviour {
    NetworkManager manager;
    TextMesh ipShow;
	// Use this for initialization
	void Start () {
        manager = GetComponent<NetworkManager>();
        //ipShow.text = ip
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Host() {
        manager.StartHost();
    }
    public void Client() {
        manager.StartClient();
    }
    public void IPChange (string ip) {
        Debug.Log("Changed IP to: " + ip);
        manager.networkAddress = ip;
        Debug.Log("Changed IP to: " + ip);
    }
}
