using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoSkill : BaseSkill {

    public float torpedoSpeed;
    public float torpedoDamage;
    // Use this for initialization
    void Start()
    {
        ship = GetComponent<JPShip>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void TriggerSkillLocation(Vector3 pos)
    {
        base.TriggerSkillLocation(pos);
        print("Torpedo pressed");
        FireTorpedo(pos);
    }
    void FireTorpedo(Vector3 pos)
    {
        SkillshotFire skills = GetComponent<SkillshotFire>();
        Vector3 direction = (pos - gameObject.transform.position).normalized;
        skills.FireTorpedo(gameObject.transform.position, direction); // NOTE: this fires off whatever gameobject this skill is on
    }
}
