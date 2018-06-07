
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;

public class UpwardRotation : MonoBehaviour {
	public GameObject colliderCenter;
	public Quaternion rotation;
	void onTriggerEnter(Collider colliderCenter){
		rotation = Quaternion.Euler(0, 10, 0);
	}

}
