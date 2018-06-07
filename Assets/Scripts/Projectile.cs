using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public int damage;
    public bool fighter;
    public float timeAlive;

    public Rigidbody rb;

    public bool autoTurn = false;
    public float turnSpeed = 1f;
    public float moveSpeed = 5f;
    public Vector3 targetPos;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, timeAlive);
    }
	
	// Update is called once per frame
	void Update () {
        if(autoTurn) {
            if(Mathf.Abs(Vector3.Distance(transform.position, targetPos)) < 1) {
                autoTurn = false;
            }
            Vector3 targetDir = targetPos - transform.position;
            float step = turnSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);

            //Get the amount rotating
            Vector3 diffRotation = Quaternion.LookRotation(newDir).eulerAngles - transform.rotation.eulerAngles;
            Vector3 newRot = Quaternion.LookRotation(newDir).eulerAngles;


            //Apply Rotation
            transform.rotation = Quaternion.Euler(newRot.x, newRot.y, newRot.z);
            rb.velocity = transform.forward * moveSpeed;

        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        Explode(); // will explode on *anything* right now
    }

    public void Explode()
    {
        // create an explosion sphere/radius, where everything that comes
        // in contact with it will take damage
        Destroy(gameObject);
    }
}