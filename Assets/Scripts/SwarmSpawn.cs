using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SwarmSpawn : NetworkBehaviour {
    public GameObject[] spawnLocations;
    public int waveNumber;
    public GameObject swarmShip;
    public GameObject target;

    public int spawnTime = 1000;
    int spawnTimer = 0;
	// Use this for initialization
	void Start () {
        SpawnWave();
	}
	
	// Update is called once per frame
	void Update () {
        if(isServer) {
            if(spawnTimer > spawnTime) {
                SpawnWave();
                spawnTimer = 0;
            }
            spawnTimer++;
        }
	}
    void SpawnWave() {
        int spawnPoint = Random.Range(0, spawnLocations.Length);
        for (int count = 0; count < waveNumber; count ++) {
            GameObject obj = (GameObject)Instantiate(swarmShip, new Vector3(count * 20f, 12.0f, 100f) + spawnLocations[spawnPoint].transform.position , spawnLocations[spawnPoint].transform.rotation);
            obj.GetComponent<JPShip>().JumpToLocation(new Vector3(count * 20f, 12.0f, 0) + spawnLocations[spawnPoint].transform.position, true);
            obj.name = "Player" + "1" + "Ship" + count + "Squad0";

            JPShip lead;
            lead = obj.GetComponent<JPShip>();
            lead.leadController = lead;
            lead.lead = true;
            lead.wingmen[0] = obj;

            lead.SetTargetShip(target);

            NetworkServer.Spawn(obj);

        }
    }
}
