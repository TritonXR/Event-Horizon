using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour {
    public GameObject verticalMarker;
    public GameObject horizontalMarker;
    public int numSpawn = 50;
    public int countMult = 1;
    public bool outlineOnly = false;
	// Use this for initialization
	void Start () {
        if (outlineOnly)
        {
            Instantiate(verticalMarker, new Vector3(250f, -0.2f, 0), Quaternion.Euler(Vector3.zero));
            Instantiate(verticalMarker, new Vector3(-250f, -0.2f, 0), Quaternion.Euler(Vector3.zero));
            Instantiate(horizontalMarker, new Vector3(0, -0.2f, 250f), Quaternion.Euler(new Vector3(0, -90, 0)));
            Instantiate(horizontalMarker, new Vector3(0, -0.2f, -250f), Quaternion.Euler(new Vector3(0, -90, 0)));
            Instantiate(horizontalMarker, new Vector3(0, -0.2f, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
            Instantiate(horizontalMarker, new Vector3(0, -0.2f, 0), Quaternion.Euler(new Vector3(0, -90, 0)));
        }
        else
        {
            for (int count = 0; count < numSpawn; count++)
            {
                GameObject obj1 = (GameObject)Instantiate(verticalMarker, new Vector3(10 * count * countMult, 0f, 0), Quaternion.Euler(Vector3.zero));
                GameObject obj2 = (GameObject)Instantiate(verticalMarker, new Vector3(-10 * count * countMult, 0f, 0), Quaternion.Euler(Vector3.zero));
                GameObject obj3 = (GameObject)Instantiate(horizontalMarker, new Vector3(0, 0f, 10 * count * countMult), Quaternion.Euler(new Vector3(0, -90, 0)));
                GameObject obj4 = (GameObject)Instantiate(horizontalMarker, new Vector3(0, 0f, -10 * count * countMult), Quaternion.Euler(new Vector3(0, -90, 0)));
                obj1.transform.parent = this.transform;
                obj2.transform.parent = this.transform;
                obj3.transform.parent = this.transform;
                obj4.transform.parent = this.transform;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
