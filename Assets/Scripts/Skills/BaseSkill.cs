using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BaseSkill : NetworkBehaviour {
    public string skillName;
	public Sprite imageIcon;
	public int cooldownTime;
	public bool locationRequired;
	public bool gameObjectRequired;

	public float timeActivated;
	public bool ready;

    public JPShip ship;
	// Use this for initialization
	void Start () {
        ship = GetComponent<JPShip>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void ConfigureSkill (UISkill button) {
        button.SetCurrentSkill(this);
		button.ChangeIcon (imageIcon);
		button.ChangeCoolDown (cooldownTime);
		button.SetTimer (timeActivated);
        button.ConfigureButton();
	}
    public virtual void TriggerSkill() {
        print("Base Trigger");
    }
    public virtual void TriggerSkillLocation(Vector3 pos) {
        
    }
    public virtual void TriggerSkillTarget(GameObject target) {
        
    }
    public virtual void TriggerSkillPreview(bool active) {
        if(active)
        print("Base Preview");
    }

}
