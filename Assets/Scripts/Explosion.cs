using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
	public float timeAlive = 2f;
	public float expandSpeed = 1.1f;
	// Use this for initialization
	void Start() {
		Destroy(gameObject, timeAlive);
	}
	
    // Update is called once per frame
	void Update () {
		transform.localScale = transform.localScale * expandSpeed;
	}
}
