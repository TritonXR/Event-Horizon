using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRange : MonoBehaviour {

	public bool inRange;
	public GameObject ship;
	public Renderer rend;
	public string find;
	public AreaRange school;
	public GameObject targetShip = null;
    int playerNumber;
	// Use this for initialization
	void Start () {
		school = GetComponentInParent<Turret>().GetComponentInParent<AreaRange>();
        playerNumber = GetComponentInParent<JPNetworkShip>().playerNumber;
	}

	// Update is called once per frame
	void Update () {
		//targetShip = school.targetShip;
	}
	private void OnTriggerStay(Collider collision)
	{
		if (collision.gameObject.Equals(targetShip))
		{
			ship = targetShip;
		}
        if (collision.gameObject.name.Contains("Ship") && ship == null && 
            collision.gameObject.GetComponentInParent<JPNetworkShip>().playerNumber != playerNumber)
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
