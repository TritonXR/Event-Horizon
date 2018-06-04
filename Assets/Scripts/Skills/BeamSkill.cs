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
    public override void TriggerSkill()
    {
        base.TriggerSkill();
        print("Beam pressed");
        FireBeam();
    }
    void FireBeam()
    {
        SkillshotFire skills = GetComponent<SkillshotFire>();
        skills.FireBeam(); // NOTE: this fires off whatever gameobject this skill is on
    }
}