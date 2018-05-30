using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPControl : MonoBehaviour {
    JPShip parentShip;
    TextMesh text;
    public GameObject healthIndicator;
    public Material undamaged;
    public Material damaged;
    GameObject[] indicators;

    JPShip ship;

    int segmentValue = 50;
    public float maxHeath;
    public float numSegments;
    public float segmentWidth;
	// Use this for initialization
	void Start () {
        ship = transform.parent.gameObject.GetComponent<JPShip>();
        parentShip = transform.parent.gameObject.GetComponent<JPShip>();

        maxHeath = ship.maxHealth;
        numSegments = maxHeath / segmentValue;
        segmentWidth = ((segmentValue * 10f) / maxHeath) * 4f;
        //print(""+segmentValue + " " + " " + maxHeath + " " + ((segmentValue * 10) / maxHeath));

        indicators = new GameObject[(int)numSegments];
        for (int count = 0; count < numSegments; count ++) {
            indicators[count] = (GameObject)Instantiate(healthIndicator, transform.position, transform.rotation);
            indicators[count].transform.parent = transform;
            Vector3 pos = indicators[count].transform.localPosition;
            pos.x = (count * (segmentWidth + (segmentWidth * 0.1f)));
            //Debug.Log("" + (count * (segmentWidth + (segmentWidth * 0.1f))));
            Vector3 scale = indicators[count].transform.localScale;
            scale.y = 4;
            scale.z = 2;
            scale.x = segmentWidth;
            indicators[count].transform.localPosition = pos;
            indicators[count].transform.localScale = scale;

        }
        text = GetComponent<TextMesh>();
    }
	
	// Update is called once per frame
	void Update () {
        int remainingHealthTicks = ship.health/segmentValue;
        for (int count = 0; count < numSegments; count++) {
            if (count > remainingHealthTicks)
            {
                indicators[count].GetComponent<Renderer>().material = damaged;
            } else {
                indicators[count].GetComponent<Renderer>().material = undamaged;
            }
        }
        //text.text = ""+parentShip.health;
	}
}
