using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuffSkill : BaseSkill {
    float originalSpeed;
    float originalTurnSpeed;

    public float buffTime = 3f;
    public float buffAmt = 2f;
	// Use this for initialization
	void Start () {
        ship = GetComponent<JPShip>();
        originalSpeed = ship.moveSpeed;
        originalTurnSpeed = ship.turnSpeed;
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
        for (int count = 0; count < ship.wingmen.Length; count ++) {
            ship.wingmen[count].GetComponent<JPShip>().moveSpeed = originalSpeed * buffAmt;
            ship.wingmen[count].GetComponent<JPShip>().turnSpeed = originalTurnSpeed * buffAmt;
        }

        yield return new WaitForSeconds(buffTime);
        for (int count = 0; count < ship.wingmen.Length; count++)
        {
            ship.wingmen[count].GetComponent<JPShip>().moveSpeed = originalSpeed;
            ship.wingmen[count].GetComponent<JPShip>().turnSpeed = originalTurnSpeed;
        }
    }
}
