using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour {
    public GameObject verticalMarker;
    public GameObject horizontalMarker;
    public int numSpawn = 50;
	// Use this for initialization
	void Start () {
        for (int count = 0; count < 50; count ++) {
            GameObject obj1 = (GameObject)Instantiate(verticalMarker, new Vector3(10 * count, 0f, 0), Quaternion.Euler(Vector3.zero));
            GameObject obj2 = (GameObject)Instantiate(verticalMarker, new Vector3(-10 * count, 0f, 0), Quaternion.Euler(Vector3.zero));
            GameObject obj3 = (GameObject)Instantiate(horizontalMarker, new Vector3(0, 0f, 10 * count), Quaternion.Euler(new Vector3 (0,-90,0)));
            GameObject obj4 = (GameObject)Instantiate(horizontalMarker, new Vector3(0, 0f, -10 * count), Quaternion.Euler(new Vector3(0, -90, 0)));
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
