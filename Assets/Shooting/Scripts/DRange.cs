using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DRange : NetworkBehaviour {

	public bool inRange;
	public GameObject ship;
	public Renderer rend;
	public string find;
	public AreaRange school;
	public GameObject targetShip = null;
    public bool useRay = false;
    public float rangeDist = 10f;
    int playerNumber;
    int teamNumber;
	// Use this for initialization
	void Start () {
		//school = GetComponentInParent<Turret>().GetComponentInParent<AreaRange>();
        playerNumber = GetComponentInParent<JPNetworkShip>().playerNumber;
        teamNumber = GetComponentInParent<JPNetworkShip>().teamNumber;
	}

	// Update is called once per frame
	void Update () {
        if(useRay) {
            Vector3 fwd = transform.TransformDirection(Vector3.forward) * rangeDist;
            //Debug.DrawRay(transform.position, fwd, Color.red, 0.01f);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, fwd, out hit, rangeDist))
            {
                if (hit.collider.gameObject.Equals(targetShip))
                {
                    ship = targetShip;
                }
                if (hit.collider.gameObject.GetComponentInParent<JPNetworkShip>() && ship == null &&
                    hit.collider.gameObject.GetComponentInParent<JPNetworkShip>().teamNumber != teamNumber)
                {
                    ship = hit.collider.gameObject;
                }
            } else {
                ship = null;
            }
        }
		//targetShip = school.targetShip;
	}
	private void OnTriggerStay(Collider collision)
	{
        //Debug.Log("Entered Range " + collision.gameObject.name);
		if (collision.gameObject.Equals(targetShip))
		{
			ship = targetShip;
		}
        if (collision.gameObject.GetComponent<JPNetworkShip>() && ship == null && 
            collision.gameObject.GetComponent<JPNetworkShip>().teamNumber != teamNumber)
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
