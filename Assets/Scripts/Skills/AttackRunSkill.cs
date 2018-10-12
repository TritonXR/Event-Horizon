using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AttackRunSkill : BaseSkill {
    public float torpedoSpeed;
    public float torpedoDamage;
    public GameObject torpedoPrefab;
    public GameObject positionMarker;

    public GameObject previewObject;
	// Use this for initialization
	void Start () {
        ship = GetComponent<JPShip>();
        previewObject.SetActive(false);
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
    public override void TriggerSkillPreview(bool active)
    {
        //print("Skillsdffd Preview " + skillName);
        base.TriggerSkillPreview(active);
        for (int count = 0; count < ship.wingmen.Length; count++)
        {
            ship.wingmen[count].GetComponent<AttackRunSkill>().Preview(active);

        }
        print("Skill Preview " + skillName);
        /*previewObject.SetActive(active);

        if (previewObject)
        {
            previewObject.SetActive(active);
        }*/

    }
    public void Preview(bool active) {
        previewObject.SetActive(active);
    }
    public void Fire()
    {
        GameObject torpedo = (GameObject)Instantiate(torpedoPrefab, positionMarker.transform.position, transform.rotation);
        torpedo.GetComponent<Rigidbody>().AddForce(torpedo.transform.forward * torpedoSpeed);
        NetworkServer.Spawn(torpedo);
    }


}
