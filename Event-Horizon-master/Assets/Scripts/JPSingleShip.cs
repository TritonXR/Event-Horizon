using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JPSingleShip : JPShip {

	// Use this for initialization
	void Start () {
        if ((GetComponent<JPNetworkShip>().teamNumber != 0) && (materialSwitch))
        {
            if (GetComponent<JPNetworkShip>().teamNumber == 2)
            {
                defaultMaterial = altDefaultMaterial;
                this.transform.GetChild(0).GetComponent<Renderer>().material = altDefaultMaterial;
            }
            print("Change material " + GetComponent<JPNetworkShip>().teamNumber);
            materialSwitch = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public override void OnShipControlDisable(bool disable)
	{
        //base.OnShipControlDisable(disable);
	}


}
