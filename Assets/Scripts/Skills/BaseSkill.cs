using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour {
	public Sprite imageIcon;
	public int cooldownTime;
	public bool locationRequired;
	public bool gameObjectRequired;

	public float timeActivated;
	public bool ready;
	// Use this for initialization
	void Start () {
		
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
        
    }
    public virtual void TriggerSkillLocation(Vector3 pos) {
        
    }
    public virtual void TriggerSkillTarget(GameObject target) {
        
    }
}
