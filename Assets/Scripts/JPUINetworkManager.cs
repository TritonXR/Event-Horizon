using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JPUINetworkManager : MonoBehaviour {
    NetworkManager manager;
    TextMesh ipShow;
    public InputField shipList;
    GameObject connectionError;
	// Use this for initialization
	void Start () {
        manager = GetComponent<NetworkManager>();
        connectionError = GameObject.Find("ConnectErrorMsg");
        connectionError.SetActive(false);
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
        print("Connecting Client");
        NetworkClient client = manager.StartClient();
        client.RegisterHandler(MsgType.Disconnect, DetectError);

    }

    public void Disconnect()
    {
        manager.StopClient();

    }
 
    void DetectError (NetworkMessage netMsg) {
        print("Manager error");
        connectionError.SetActive(true);

        connectionError.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
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
