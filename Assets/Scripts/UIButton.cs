using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour {

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
}
