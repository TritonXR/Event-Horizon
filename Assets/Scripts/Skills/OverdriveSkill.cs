using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverdriveSkill : BaseSkill {
    int originalSpeed;
    public float buffAmt;
    public float buffTime = 5f;
	// Use this for initialization
	void Start () {
        ship = GetComponent<JPShip>();
        originalSpeed = ship.GetComponent<JPShip>().fireRate;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public override void TriggerSkill()
    {
        base.TriggerSkill();
        print("SpeedBost pressed");
        StartCoroutine(ApplyBuff());
    }
    IEnumerator ApplyBuff()
    {
        for (int count = 0; count < ship.wingmen.Length; count++)
        {
            
            ship.wingmen[count].GetComponent<JPShip>().fireRate =  ((int)(originalSpeed * buffAmt));
        }

        yield return new WaitForSeconds(buffTime);
        for (int count = 0; count < ship.wingmen.Length; count++)
        {
            ship.wingmen[count].GetComponent<JPShip>().fireRate = originalSpeed;

        }
    }
}
