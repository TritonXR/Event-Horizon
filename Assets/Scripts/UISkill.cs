using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkill : MonoBehaviour {
    public int skillNumber = 0;
    public Text timerShow;
    public Image icon;
    public int maxCoolDown = 5;
    public bool locked = false;
    public Material activeMat;
    public Material inactiveMat;
    int countVal;
    public float valShow;
    float startTime;
    JPUIController uiController;

    public BaseSkill currentSkill;
    public JPInputController localPlayerController;
    public bool inactive = true;
    // Use this for initialization
	void Start () {
        icon = GetComponent<Image>();
        timerShow = transform.Find("Text").gameObject.GetComponent<Text>();
        activeMat = icon.material;
        uiController = transform.parent.gameObject.GetComponent<JPUIController>();
        SetInvisible();
	}
	
	// Update is called once per frame
	void Update () {
        if(inactive) {
            return;
        }
        if (locked)
        {
            valShow = Time.time - startTime;
            if(maxCoolDown - valShow < 0) {
                locked = false;
            } else {
                timerShow.text = "" + (maxCoolDown - (int)valShow);
            }
            icon.material = inactiveMat;
        } else {
            timerShow.text = "";
            icon.material = activeMat;
            currentSkill.ready = true;
        }

	}
    public void SetCurrentSkill (BaseSkill newSkill) {
        currentSkill = newSkill;
    }
    public void ChangeIcon (Sprite newSprite) {
        icon.sprite = newSprite;
    }
    public void ChangeCoolDown (int newCoolDown) {
        maxCoolDown = newCoolDown;
        if (maxCoolDown - valShow > 0)
        {
            timerShow.text = "" + (maxCoolDown - (int)valShow);
            locked = true;
        } else {
            timerShow.text = "";
            icon.material = activeMat;
            currentSkill.ready = true;
        }
    }
    public void SetTimer (float timeBegun) {
        startTime = timeBegun;
        locked = true;
        currentSkill.timeActivated = startTime;
        currentSkill.ready = false;
    }
    public void ButtonPress () {
        if (!locked)
        {
            localPlayerController.SkillPressed(skillNumber, this);
            if ((!currentSkill.gameObjectRequired) && (!currentSkill.locationRequired))
            {
                print("Pressed");
                BeginCooldown();
            } else if(currentSkill.gameObjectRequired) {
                
            } else if(currentSkill.locationRequired) {
                
            }
        }
    }
    public void ConfigureButton () {
        GetComponent<Button>().interactable = true;
        GetComponent<Image>().enabled = true;
        print("Activating button");
        inactive = false;
    }
    public void BeginCooldown () {
        SetTimer(Time.time);
    }

    public void SetInvisible () {
        GetComponent<Button>().interactable = false;
        timerShow.text = "";
        GetComponent<Image>().enabled = false;
        inactive = true;
    }
}
