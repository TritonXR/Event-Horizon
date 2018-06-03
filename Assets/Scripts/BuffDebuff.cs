using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDebuff : MonoBehaviour {
    public float buffTime;
    public float healAmount;
    public float speedAmount;
	// Use this for initialization
	void Start () {
        Destroy(this, buffTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        JPShip ship = other.gameObject.GetComponent<JPShip>();
        ship.health += (int)healAmount;
        ship.moveSpeed += speedAmount;
    }
}
