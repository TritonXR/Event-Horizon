using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    public int health = 100;
    public int amt = 20;
    public Ship ship;
	// Use this for initialization
	void Start () {
        ship = GetComponentInParent<Ship>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Bullet"))
        {
            health -= amt;
            ship.health -= amt;
        }
        if(health <= 0)
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
