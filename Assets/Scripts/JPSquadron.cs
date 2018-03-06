using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPSquadron : JPShip {
    public GameObject[] ships;
    public Vector3[] shipPos;
    public JPFighter[] shipCtrl;

	// Use this for initialization
	void Start () {
        ships = new GameObject[transform.childCount];
        shipPos = new Vector3[transform.childCount];
        shipCtrl = new JPFighter[transform.childCount];
        for (int count = 0; count < transform.childCount; count ++)
        {
            ships[count] = transform.GetChild(count).gameObject;
            shipPos[count] = transform.GetChild(count).localPosition;
            shipCtrl[count] = ships[count].GetComponent<JPFighter>();
            shipCtrl[count].SetOffset(shipPos[count]);
            shipCtrl[count].SetController(this);
        }
        SetTargetShip(target);
	}
	
	// Update is called once per frame
	void Update () {
        //transform.position = ships[0].transform.position - ships[0].transform.localPosition;
	}
    public override void SetTargetShip(GameObject ship)
    {
        base.SetTargetShip(ship);
        for (int count = 0; count < transform.childCount; count++)
        {
            shipCtrl[count].SetTargetShip(ship);
        }
    }
    public override void SetTargetPosition(Vector3 vector)
    {
        base.SetTargetPosition(vector);
        for (int count = 0; count < transform.childCount; count++)
        {
            shipCtrl[count].SetTargetPosition(vector + shipPos[count]);
        }
    }
    public override void SetSelected(bool selected)
    {
        for (int count = 0; count < transform.childCount; count++) {
            shipCtrl[count].SetSelected(selected);
        }
       
    }
}
