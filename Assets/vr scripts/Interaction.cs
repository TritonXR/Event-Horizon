using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {

    public FixedJoint joint = null;

    public Rigidbody rigbod = null;
    private List<Rigidbody> bodies = new List<Rigidbody>();

    private void Awake()
    {
        joint = GetComponent<FixedJoint>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Interactable"))
        {
			//print ("rrr");
            //bodies.Add(other.gameObject.GetComponent<Rigidbody>());
			if(rigbod == null)
				rigbod = other.gameObject.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            //bodies.Remove(other.gameObject.GetComponent<Rigidbody>());
        }
    }

    public void Pick()
    {
        //rigbod = Nearest();

		if (!rigbod) {
			print ("nonearest");
			return;
		}

        rigbod.transform.position = transform.position;
		if (rigbod.gameObject.GetComponent<stickGrab>())
		{
			rigbod.gameObject.GetComponent<stickGrab>().Picked();
		}
        joint.connectedBody = rigbod;
		rigbod.velocity = Vector3.zero;
		rigbod.angularVelocity = Vector3.zero;
    }

    public void Drop(SteamVR_Controller.Device device)
    {
        if (!rigbod)
            return;
		if (rigbod.gameObject.GetComponent<stickGrab>())
		{
			rigbod.gameObject.GetComponent<stickGrab>().Dropped();
		}
		rigbod.velocity = Vector3.zero;
		rigbod.angularVelocity = Vector3.zero;
		rigbod = null;
        joint.connectedBody = null;
    }

    private Rigidbody Nearest()
    {
        Rigidbody near = null;
        float minDistance = float.MaxValue;
        float distance = 0.0f;

        foreach (Rigidbody contact in bodies)
        {
            distance = (contact.gameObject.transform.position - transform.position).sqrMagnitude;

            if(distance < minDistance)
            {
                minDistance = distance;
                near = contact;
            }
        }
        return near;
    }
}
