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

    public bool ignoreColl = false;

    public bool overrideDestruction = false;

    public bool explosive = false;

    public GameObject destructionObject;

    public int teamNum;

    public bool permanent = false;

    public ParticleSystem[] destructionParticles;
	// Use this for initialization
	void Start () {
        if (!permanent)
        {
            Destroy(gameObject, timeAlive);
        }
        //fireParticles();
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
        //Explode(); // will explode on *anything* right now
        if(explosive) {
            CollisionResponse();
        }
    }

    public void Explode()
    {
        // create an explosion sphere/radius, where everything that comes
        // in contact with it will take damage
        Destroy(gameObject);
    }
    public void CollisionResponse () {
        if (destructionParticles.Length > 0)
        {
            fireParticles();
        }
        if (destructionObject) {
            Instantiate(destructionObject, transform.position, transform.rotation);
        }

        if (!overrideDestruction)
        {
            
            Destroy(gameObject);
        }
    }

    void fireParticles() {
        for (int count = 0; count < destructionParticles.Length; count++)
        {
            destructionParticles[count].transform.parent = null;
            destructionParticles[count].Play();
            Destroy(destructionParticles[count].gameObject, destructionParticles[count].main.duration);
        }
    }
}