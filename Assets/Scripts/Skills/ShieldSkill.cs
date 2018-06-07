using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShieldSkill : BaseSkill {
    public GameObject shield;
    public float shieldTime = 5f;
	// Use this for initialization
	void Start () {
        ship = GetComponent<JPShip>();
        shield.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public override void TriggerSkill()
    {
        base.TriggerSkill();
        print("Shield pressed");
        StartCoroutine(ApplyShield());
    }
    IEnumerator ApplyShield()
    {
        for (int count = 0; count < ship.wingmen.Length; count++)
        {
            ship.wingmen[count].GetComponent<ShieldSkill>().shield.SetActive(true);
            ship.wingmen[count].GetComponent<ShieldSkill>().RpcActivateShield(true);
        }

        yield return new WaitForSeconds(shieldTime);
        for (int count = 0; count < ship.wingmen.Length; count++)
        {
            ship.wingmen[count].GetComponent<ShieldSkill>().shield.SetActive(false);
            ship.wingmen[count].GetComponent<ShieldSkill>().RpcActivateShield(false);
        }
    }
    [ClientRpc]
    void RpcActivateShield(bool value) {
        shield.GetComponent<Renderer>().material = ship.defaultMaterial;
        shield.SetActive(value);
    }
}
