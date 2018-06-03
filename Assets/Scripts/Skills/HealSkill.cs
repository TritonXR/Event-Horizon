using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkill : BaseSkill
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
        print("Heal pressed");
        StartCoroutine(ApplyHeal());
    }
    IEnumerator ApplyHeal()
    {
        return null;
    }
}
