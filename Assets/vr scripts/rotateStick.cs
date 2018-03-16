using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateStick : MonoBehaviour {
    public Transform target;
    public Transform rotator;
    public float speed;
	public bool throttle;
	// Use this for initialization
	void Start () {
        speed = 10f;
        rotator = transform;
		target = transform.parent.GetChild(1);
	}
	// Update is called once per frame
	void Update () {
		Vector3 targetDir = target.position - transform.position;
        float step = speed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        //Debug.DrawRay(transform.position, newDir, Color.red);
        rotator.rotation = Quaternion.LookRotation(newDir);
    }
}
