using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticFieldSkill : BaseSkill
{

    public float healAmount;
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
        print("MagneticField pressed");
        ApplyMagneticField();
    }
    void ApplyMagneticField()
    {
        SkillshotFire skills = GetComponent<SkillshotFire>();
        skills.MagneticField(); // NOTE: this slows ALL in the area
    }
}
