using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StationControl : NetworkBehaviour {
    [SyncVar]
    public int health = 0;

    public int maxHealth = 10000;
	// Use this for initialization
	void Start () {
        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        //print("hit");
        if (other.gameObject.GetComponent<Projectile>())
        {
            
            health -= other.gameObject.GetComponent<Projectile>().damage;
            Destroy(other.gameObject);

        }
    }
}
