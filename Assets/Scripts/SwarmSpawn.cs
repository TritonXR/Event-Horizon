using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SwarmSpawn : NetworkBehaviour {
    public GameObject[] spawnLocations;
    public int waveNumber;
    public GameObject swarmShip;
    public GameObject target;

    public int spawnTime = 1000;

    public StationControl station;
    [SyncVar]
    int waveNum = 0;
    int spawnTimer = 0;
    public Text score;
    public float stationHealth;
	// Use this for initialization
	void Start () {
        SpawnWave();
        //station = target.GetComponent<StationControl>();
	}
	
	// Update is called once per frame
	void Update () {
        if(isServer) {
            if(spawnTimer > spawnTime) {
                //SpawnWave();
                spawnTimer = 0;
                waveNum++;
            }
            spawnTimer++;
        }
        //stationHealth = ((float)station.health / station.maxHealth) * 100;
        //int temp = (int)stationHealth;
        //score.text = "Wave: " + waveNum + " - " + "Station Health: " + temp + "%";
        //if(station.health <= 0) {
            //GameObject.Find("UICanvas").GetComponent<JPUIController>().SetGameOver(false);
        //}
	}
    void SpawnWave() {
        print("Spawning wave");
        //int spawnPoint = Random.Range(0, spawnLocations.Length);
        int spawnPoint = 0;
        waveNumber = 1;
        for (int count = 0; count < waveNumber; count ++) {
            GameObject obj = (GameObject)Instantiate(swarmShip, new Vector3(count * 5f, 20.0f, 100f) + spawnLocations[spawnPoint].transform.position , spawnLocations[spawnPoint].transform.rotation);
            obj.GetComponent<JPShip>().JumpToLocation(new Vector3(count * 5f, 30.0f, 0) + spawnLocations[spawnPoint].transform.position, true);
            obj.name = "Player" + "2" + "Ship" + count + "Squad0";

            JPShip lead;
            lead = obj.GetComponent<JPShip>();
            lead.leadController = lead;
            lead.lead = true;
            lead.wingmen[0] = obj;

            //lead.SetTargetShip(target);

            NetworkServer.Spawn(obj);

        }
    }
}
