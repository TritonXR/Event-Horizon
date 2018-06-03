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
        StartCoroutine(FireTorpedo());
    }
    IEnumerator FireTorpedo()
    {
        return null;
    }
}