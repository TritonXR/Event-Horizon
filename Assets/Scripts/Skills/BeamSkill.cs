using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamSkill : BaseSkill
{
    public float beamDamage;
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
        print("Beam pressed");
        FireBeam(pos);
    }
    void FireBeam(Vector3 pos)
    {
        SkillshotFire skills = GetComponent<SkillshotFire>();
        Vector3 direction = (pos - gameObject.transform.position).normalized;
        skills.FireBeam(gameObject.transform.position, direction); // NOTE: this fires off whatever gameobject this skill is on
    }
}