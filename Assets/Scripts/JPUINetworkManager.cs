using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JPUINetworkManager : MonoBehaviour {
    NetworkManager manager;
    TextMesh ipShow;
    public InputField shipList;
	// Use this for initialization
	void Start () {
        manager = GetComponent<NetworkManager>();
        //ipShow.text = ip
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Host() {
        //PlayerPrefs.SetString("SpawnList", shipList.text);
        //Debug.Log("Set SpawnList " + PlayerPrefs.GetString("SpawnList"));
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
    public void SpawnListChange(string list)
    {
        Debug.Log("Changed ShipList from: " + PlayerPrefs.GetString("SpawnList"));
        PlayerPrefs.SetString("SpawnList", list);
        Debug.Log("Changed IP to: " + PlayerPrefs.GetString("SpawnList"));
    }
}
