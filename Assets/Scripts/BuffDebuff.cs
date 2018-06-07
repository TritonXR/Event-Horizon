using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BuffDebuff : NetworkBehaviour {
    public float buffTime;
    public float healAmount;
    public float speedAmount;
    public int teamNum;
	// Use this for initialization
	void Start () {
        //Destroy(this, buffTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if(!isServer) {
            return;
        }
        if (other.gameObject.GetComponent<JPShip>())
        {
            
            JPShip ship = other.gameObject.GetComponent<JPShip>();
            if(ship.teamNum == teamNum) {
                if(ship.health < ship.maxHealth) {
                    ship.health += (int)healAmount;
                }

                ship.moveSpeed += speedAmount;
                //print(other.gameObject.name);
            }

        }
    }
}
