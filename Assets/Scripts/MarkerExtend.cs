using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerExtend : MonoBehaviour {
    public float maxExtend;
    public float extendSpeed = 0.01f;
    public float extendVal = 0;
    public float minExtend = 10f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0) * extendVal + new Vector3(0, 0, 1f);

        extendVal += extendSpeed;
        if (extendVal > maxExtend)
        {
            extendVal = minExtend;
        }
    }
}

