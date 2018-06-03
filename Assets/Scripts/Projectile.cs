using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float damage;
    public bool fighter;
    public float timeAlive;

    public Rigidbody rb;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        Destroy(gameObject, timeAlive);
	}

    private void OnCollisionEnter(Collision collision)
    {
        Explode(); // will explode on *anything* right now
    }

    public void Explode()
    {
        // create an explosion sphere/radius, where everything that comes
        // in contact with it will take damage
        Destroy(gameObject);
    }
}