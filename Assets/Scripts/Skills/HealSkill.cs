using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealSkill : BaseSkill
{
    public GameObject healRadius;
    public float healAmount;
    public float healTime = 5f;
    // Use this for initialization
    void Start()
    {
        ship = GetComponent<JPShip>();
        healRadius.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void TriggerSkill()
    {
        base.TriggerSkill();
        print("Heal pressed");
        //ApplyHeal();
        StartCoroutine(ApplyHealBuff());
    }
    void ApplyHeal()
    {
        SkillshotFire skills = GetComponent<SkillshotFire>();
        skills.HealArea(); // NOTE: this heals ALL in the area
    }

    IEnumerator ApplyHealBuff()
    {
        for (int count = 0; count < ship.wingmen.Length; count++)
        {
            ship.wingmen[count].GetComponent<HealSkill>().healRadius.SetActive(true);
            ship.wingmen[count].GetComponent<HealSkill>().RpcActivateHeal(true);
            ship.wingmen[count].GetComponent<HealSkill>().healRadius.GetComponent<BuffDebuff>().teamNum = ship.teamNum;

        }

        yield return new WaitForSeconds(healTime);
        for (int count = 0; count < ship.wingmen.Length; count++)
        {
            ship.wingmen[count].GetComponent<HealSkill>().healRadius.SetActive(false);
            ship.wingmen[count].GetComponent<HealSkill>().RpcActivateHeal(false);
        }
    }
    [ClientRpc]
    void RpcActivateHeal(bool value)
    {
        healRadius.GetComponent<Renderer>().material = ship.defaultMaterial;
        healRadius.SetActive(value);
        healRadius.GetComponent<BuffDebuff>().teamNum = ship.teamNum;
    }
}
