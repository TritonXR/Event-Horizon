using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMarker : MonoBehaviour {
    public bool ignoreRotation = false;
    public float heightOffset = 0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 curPos = transform.parent.position;
        //curPos = transform.TransformPoint(curPos);
        curPos.y = -0.1f + heightOffset;
        transform.position = curPos;

        if (!ignoreRotation)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            float parentY = transform.parent.rotation.eulerAngles.y;
            Vector3 curRot = transform.rotation.eulerAngles;
            curRot.y = parentY;
            transform.rotation = Quaternion.Euler(curRot);
        }

    }
}
