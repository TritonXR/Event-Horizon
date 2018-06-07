using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AttackRunSkill : BaseSkill {
    public float torpedoSpeed;
    public float torpedoDamage;
    public GameObject torpedoPrefab;
    public GameObject positionMarker;
	// Use this for initialization
	void Start () {
        ship = GetComponent<JPShip>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public override void TriggerSkill()
    {
        base.TriggerSkill();
        print("SpeedBost pressed");


        for (int count = 0; count < ship.wingmen.Length; count++)
        {
            ship.wingmen[count].GetComponent<AttackRunSkill>().Fire();

        }

       

    }
    public void Fire()
    {
        GameObject torpedo = (GameObject)Instantiate(torpedoPrefab, positionMarker.transform.position, transform.rotation);
        torpedo.GetComponent<Rigidbody>().AddForce(torpedo.transform.forward * torpedoSpeed);
        NetworkServer.Spawn(torpedo);
    }


}
