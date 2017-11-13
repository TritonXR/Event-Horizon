using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRange : MonoBehaviour {

	public bool inRange;
	public GameObject ship;
	public Renderer rend;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}
	private void OnTriggerEnter(Collider collision)
	{

		if (collision.gameObject.tag == "g" && ship == null)
		{
			ship = collision.gameObject;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if(other.gameObject == ship)
		{
			ship = null;
		}
	}
}
