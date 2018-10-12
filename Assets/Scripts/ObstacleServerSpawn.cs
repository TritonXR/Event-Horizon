using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObstacleServerSpawn : NetworkBehaviour {
    public GameObject[] spawnObjects;
    public GameObject[] spawnPrefabs;
	// Use this for initialization
	void Start () {
        if(isServer) {
            for (int count = 0; count < spawnObjects.Length; count++)
            {
                GameObject obj = Instantiate(spawnPrefabs[count], spawnObjects[count].transform.position, spawnObjects[count].transform.rotation);
                obj.transform.localScale = spawnObjects[count].transform.localScale;

                NetworkServer.Spawn(obj);

                spawnObjects[count].SetActive(false);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {

	}
}
