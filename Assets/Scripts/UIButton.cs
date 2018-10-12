using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour {
    public Sprite[] images;
    public int mode = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetInvisible()
    {
        GetComponent<Button>().interactable = false;

        GetComponent<Image>().enabled = false;

    }
    public void SetActive() {
        GetComponent<Button>().interactable = true;

        GetComponent<Image>().enabled = true;
    }

    public void toggleImage(int val) {
        GetComponent<Image>().sprite = images[val];
        mode = val;
    }

    public int getMode() {
        return mode;
    }
}
