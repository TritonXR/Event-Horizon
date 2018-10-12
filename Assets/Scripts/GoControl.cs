using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoControl : MonoBehaviour
{
    public Vector3 rotationVal;
    public Vector3 deadZone;
    public Vector3 rotationVel;
    public bool vrMode = false;
    public float speed = 2.0f;
    Rigidbody rb;

    float xRot = 0;
    float yRot = 0;
    float zRot = 0;

    public bool xTurn = false;
    public bool yTurn = false;
    public bool zTurn = false;

    public float xReturn;

    Vector3 percentageRot;

    public LaserShoot laser;
    int count = 0;
    int fireRate = 75;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vrMode)
        {
            OVRInput.Controller activeController = OVRInput.GetActiveController();

            rotationVal = OVRInput.GetLocalControllerRotation(activeController).eulerAngles;

            if (rotationVal.x > 180f)
            {
                rotationVal.x -= 360f;
            }
            if (rotationVal.y > 180f)
            {
                rotationVal.y -= 360f;
            }
            if (rotationVal.z > 180f)
            {
                rotationVal.z -= 360f;
            }
            percentageRot = rotationVal / 90f;
            //rotationVal = rot.eulerAngles;
            //transform.rotation = rot;

            if (OVRInput.Get(OVRInput.Button.Any))
            {
                Debug.Log("Shoot");
                count++;
                if (count > fireRate)
                {
                    laser.FireVR();
                    count = 0;
                }
            }
            if ((OVRInput.GetUp(OVRInput.Button.Any))) {
                count = fireRate;
            }
        }
        else
        {
            percentageRot = Vector3.one;
        }

        if (Mathf.Abs(rotationVal.x) > deadZone.x)
        {
            transform.Rotate(Vector3.right * Time.deltaTime * percentageRot.x * Mathf.Abs(percentageRot.x) * rotationVel.x);
            xTurn = true;
        }
        else
        {
            xTurn = false;
        }

        if (Mathf.Abs(rotationVal.z) > deadZone.z)
        {
            transform.Rotate(-Vector3.forward * Time.deltaTime * percentageRot.z * rotationVel.z);
            zTurn = true;
        }
        else
        {
            zTurn = false;
        }

        if (Mathf.Abs(rotationVal.y) > deadZone.y)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * percentageRot.y * rotationVel.y);
            yTurn = true;
        }
        else
        {
            yTurn = false;
        }

        rb.velocity = transform.forward * speed;



    }
}
