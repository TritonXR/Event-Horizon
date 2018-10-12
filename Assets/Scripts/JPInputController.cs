using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JPInputController : NetworkBehaviour {
    public Vector3 minPos;
    public Vector3 maxPos;
    JPNetworkPlayer networkPlayer;
    GameObject selectedShip;
    GameObject targetShip;
    Vector3 targetPosition;
    public LayerMask touchMask;
    Plane groundPlane = new Plane (new Vector3 (0, 0, 0), new Vector3 (-100, 0, -100), new Vector3 (100, 0, -100));
    GameObject marker;
    public int playerNumber = 0;
    public int targetMode = 0;
    JPUIController localUI;
    Slider healthSlider;

    GameObject targetedShip;

    Vector3 selectStartPos;
    bool selectStart = true;

    public bool rotationSet = false;

    float minPullDistance = 50f;
    int tapCount = 0;

    BaseSkill currentSkill;
    UISkill currentButton;
    bool skillActive = false;
    bool skillTimingSpacing = false;
    int skillIndex;

    UISkill[] uiSkillButtons = new UISkill[3];
    UIButton cancelButton;
    //UIButton targetButton;
    UIButton moveButton;
    // Use this for initialization
    void Start () {
        if (!isLocalPlayer)
        {
            return;
        }
        //healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        networkPlayer = this.GetComponent<JPNetworkPlayer> ();
        marker = GameObject.Find ("Marker");
        playerNumber = networkPlayer.playerNumber;
        JPNetworkHostManager localHost = GameObject.Find("NetworkManager").GetComponent<JPNetworkHostManager>();
        JPUIController.OnModeCancel += setModeCancel;
        JPUIController.OnModeMove += setModeMove;
        JPUIController.OnModeTarget += setModeTarget;
        JPUIController.OnDefendMode += SetDefendMode;
        JPUIController.OnAttackMode += SetAttackMode;
        JPUIController.OnSpeedMode += SetSpeedMode;
        JPUIController.OnRetreat += Retreat;
        JPUIController.OnQuit += QuitGame;
        localUI = GetComponent<JPUIController>();
        marker.SetActive(false);

        GameObject.Find("UICanvas").transform.Find("Skill1").gameObject.GetComponent<UISkill>().localPlayerController = this;
        uiSkillButtons[0] = GameObject.Find("UICanvas").transform.Find("Skill1").gameObject.GetComponent<UISkill>();
        GameObject.Find("UICanvas").transform.Find("Skill2").gameObject.GetComponent<UISkill>().localPlayerController = this;
        uiSkillButtons[1] = GameObject.Find("UICanvas").transform.Find("Skill2").gameObject.GetComponent<UISkill>();
        GameObject.Find("UICanvas").transform.Find("Skill3").gameObject.GetComponent<UISkill>().localPlayerController = this;
        uiSkillButtons[2] = GameObject.Find("UICanvas").transform.Find("Skill3").gameObject.GetComponent<UISkill>();

        cancelButton = GameObject.Find("UICanvas").transform.Find("CancelButton").gameObject.GetComponent<UIButton>();
        cancelButton.SetInvisible();
        //targetButton = GameObject.Find("UICanvas").transform.Find("TargetButton").gameObject.GetComponent<UIButton>();
        moveButton = GameObject.Find("UICanvas").transform.Find("MoveButton").gameObject.GetComponent<UIButton>();
        moveButton.SetInvisible();
    }
    
    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer)
        {
            //print("Not local player!");
            return;
        }
        //print("LOCAL UI CONTROLLER " + localUI.targetMode);
        if (Input.GetMouseButtonDown (0)) {
            bool touchedUI = false;
            foreach (Touch touch in Input.touches)
            {
                int id = touch.fingerId;
                if (EventSystem.current.IsPointerOverGameObject(id))
                {
                    touchedUI = true;
                }
            }
            if(touchedUI) {
                Debug.Log("touched down on the UI");
                tapCount = 0;
            }
            else if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked down on the UI");
                tapCount = 0;
            }
            else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 100000, Color.yellow, 0.0f, false);
                selectStart = false;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchMask))
                {
                    Debug.Log(hit.collider.name);
                    Collider coll = hit.collider;
                    if(hit.collider.name == "GroundMarker") {
                        print("GroundMarker");
                        coll = hit.collider.gameObject.transform.parent.GetComponent<Collider>();
                        print(coll.name);
                    }
                    if (coll.gameObject.transform.name.Contains("Ship"))
                    {
                        if (((tapCount > 0) || (selectedShip == null)) && (coll.gameObject.name.Contains("Player" + networkPlayer.playerNumber)))
                        {
                            if (tapCount > 0)
                            {

                                DeselectShip();
                            }

                            if (selectedShip != null)
                            {
                                selectedShip.GetComponent<JPShip>().leadController.SetSelected(false);
                            }
                            if (coll.gameObject.name.Contains("Fighter"))
                            {
                                //Debug.Log("Got Squad lead " + coll.transform.parent.name);
                                selectedShip = coll.transform.parent.gameObject;
                                selectedShip.GetComponent<JPShip>().leadController.SetSelected(true);
                                selectShip(selectedShip);
                            }
                            else
                            {
                                selectedShip = coll.gameObject;
                                selectedShip.GetComponent<JPShip>().leadController.SetSelected(true);
                                selectShip(selectedShip);
                            }
                            marker.SetActive(true);
                            selectStart = true;
                            selectStartPos = selectedShip.transform.position;

                            marker.GetComponent<MarkerController>().setRotationMode(selectedShip.GetComponent<JPShip>().rotationControl);
                            marker.transform.rotation = selectedShip.GetComponent<JPShip>().targetRotation;

                            tapCount = 0;
                            //targetMode = 0;
                            //healthSlider.gameObject.SetActive(true);
                            //healthSlider.value = selectedShip.GetComponent<JPShip>().healthPercent;
                        } else {
                            tapCount++;
                        }
                    } else if(coll.gameObject.transform.name.Contains("Marker")) {
                        if((selectedShip.GetComponent<JPShip>().rotationControl) && (targetMode == 0)) {
                            rotationSet = true;
                            marker.GetComponent<MarkerController>().setRotationMode(true);
                        }

                    }
                }
                else
                {
                    if (tapCount > 0)
                    {

                        DeselectShip();
                    }
                    tapCount++;
                }
            }
        } else if(Input.GetMouseButtonUp (0)) {
            bool touchedUI = false;
            if (selectedShip != null)
            {
                for (int count = 0; count < selectedShip.GetComponent<JPShip>().skills.Length; count ++) {
                    if (selectedShip.GetComponent<JPShip>().skills[count])
                    {
                        selectedShip.GetComponent<JPShip>().skills[count].TriggerSkillPreview(false);
                    }
                }
            }
                foreach (Touch touch in Input.touches)
            {
                int id = touch.fingerId;
                if (EventSystem.current.IsPointerOverGameObject(id))
                {
                    touchedUI = true;
                }
            }
            if (touchedUI)
            {
                //Debug.Log("touched down on the UI");
                tapCount = 0;
            }
            else if (EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log("Clicked Up on the UI");
                tapCount = 0;
            }
            else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 100000, Color.yellow, 0.0f, false);
                float rayDistance;

                if(skillActive) {
                    if(!currentSkill) {
                        skillActive = false;

                    } else if(currentSkill.gameObjectRequired) {
                        if (Physics.Raycast(ray, out hit))
                        {
                            Collider coll = hit.collider;
                            if (hit.collider.name == "GroundMarker")
                            {
                                print("GroundMarker");
                                coll = hit.collider.gameObject.transform.parent.GetComponent<Collider>();
                                print(coll.name);
                            }
                            if (coll.gameObject.name.Contains("Ship"))
                            {
                                //hit.collider.gameObject;

                                SetSkillTarget(coll.gameObject);
                                tapCount = 2;
                            }
                              
                        }
                    } else if(currentSkill.locationRequired) {
                        // Position Targeted
                        if (groundPlane.Raycast(ray, out rayDistance))
                        {

                            //Location
                            SetSkillLocation(ray.GetPoint(rayDistance));
                                ray.GetPoint(rayDistance);
                            tapCount = 2;
                        }
                    } else {
                        skillActive = false;

                    }
                } else if(rotationSet) {

                    groundPlane.Raycast(ray, out rayDistance);
                    Vector3 pos = ray.GetPoint(rayDistance);

                    Quaternion rotateTo = Quaternion.LookRotation(pos);
                    rotateTo.x = 0;
                    rotateTo.z = 0;
                    marker.transform.rotation = rotateTo;

                    if (selectedShip.GetComponent<JPShip>().rotationControl)
                    {
                        //selectedShip.GetComponent<JPShip>().targetRotation = rotateTo;
                        setTargetRotation(rotateTo);
                    }
                } else {
                    //print("Targeting Sent");
                    // Ship Targeted
                    if (targetMode == 1)
                    {
                        if (Physics.Raycast(ray, out hit))
                        {
                            Collider coll = hit.collider;
                            if (hit.collider.name == "GroundMarker")
                            {
                                //print("GroundMarker");
                                coll = hit.collider.gameObject.transform.parent.GetComponent<Collider>();
                                print(coll.name);
                            }
                            if (coll.gameObject.name.Contains("Ship"))
                            {
                                if (selectedShip != null)
                                {
                                    setTargetShip(coll.gameObject);
                                    marker.transform.position = coll.gameObject.transform.position;
                                    targetedShip = coll.gameObject;
                                    tapCount = 2;
                                    //marker.transform.rotation = selectedShip.GetComponent<JPShip>().targetRotation;
                                }
                            }
                        }
                    }
                    else if (targetMode == 0)
                    {
                        // Position Targeted
                        if (groundPlane.Raycast(ray, out rayDistance))
                        {
                            if ((!selectStart) || (Vector3.Distance(selectStartPos, ray.GetPoint(rayDistance)) > minPullDistance))
                            {
                                if (selectedShip != null)
                                {
                                    Vector3 pos = ray.GetPoint(rayDistance);
                                    if(pos.x > maxPos.x) {
                                        pos.x = maxPos.x;
                                    }
                                    if(pos.x < minPos.x) {
                                        pos.x = minPos.x;
                                    }
                                    if (pos.z > maxPos.z)
                                    {
                                        pos.z = maxPos.z;
                                    }
                                    if (pos.z < minPos.z)
                                    {
                                        pos.z = minPos.z;
                                    }
                                    setTargetPosition(pos);
                                    marker.transform.position = pos;
                                    targetedShip = null;
                                    tapCount = 2;
                                    //marker.transform.rotation = selectedShip.GetComponent<JPShip>().targetRotation;
                                }
                            }
                        }
                    }
                    else
                    {
                        targetedShip = null;
                    }
                }

            }
        } else if(Input.GetMouseButton (0)) {

            bool touchedUI = false;
            foreach (Touch touch in Input.touches)
            {
                int id = touch.fingerId;
                if (EventSystem.current.IsPointerOverGameObject(id))
                {
                    touchedUI = true;
                }
            }
            if (touchedUI)
            {
                //Debug.Log("touched down on the UI");
                tapCount = 0;
            }
            else if (EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log("Clicked Move on the UI");

            }
            else
            {
                if (selectedShip)
                {
                    if(rotationSet) {
                        //print("Rotating");
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        float rayDistance;
                        groundPlane.Raycast(ray, out rayDistance);
                        Vector3 pos = ray.GetPoint(rayDistance);

                        Quaternion rotateTo = Quaternion.LookRotation(pos);
                        rotateTo.x = 0;
                        rotateTo.z = 0;
                        marker.transform.rotation = rotateTo;

                        if(selectedShip.GetComponent<JPShip>().rotationControl) {
                            //selectedShip.GetComponent<JPShip>().targetRotation = rotateTo;
                            setTargetRotation(rotateTo);
                        }


                    } 
                    else if (targetMode == 0)
                    {
                        marker.GetComponent<MarkerController>().setMode(0);
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        float rayDistance;
                        groundPlane.Raycast(ray, out rayDistance);
                        if ((!selectStart) || (Vector3.Distance(selectStartPos, ray.GetPoint(rayDistance)) > minPullDistance))
                        {

                            Vector3 pos = ray.GetPoint(rayDistance);
                            if (pos.x > maxPos.x)
                            {
                                pos.x = maxPos.x;
                            }
                            if (pos.x < minPos.x)
                            {
                                pos.x = minPos.x;
                            }
                            if (pos.z > maxPos.z)
                            {
                                pos.z = maxPos.z;
                            }
                            if (pos.z < minPos.z)
                            {
                                pos.z = minPos.z;
                            }
                            marker.transform.position = pos;
                            //print(selectStartPos + " " + ray.GetPoint(rayDistance) + " = " + Vector3.Distance(selectStartPos, ray.GetPoint(rayDistance)));

                        }
                        else
                        {
                            //print(selectStartPos + " " + ray.GetPoint(rayDistance) + " = " + Vector3.Distance(selectStartPos, ray.GetPoint(rayDistance)));
                        }

                    }
                    else if (targetMode == 1)
                    {
                        marker.GetComponent<MarkerController>().setMode(1);
                        RaycastHit hit;
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        Debug.DrawRay(ray.origin, ray.direction * 100000, Color.yellow, 0.0f, false);
                        float rayDistance;

                        if (Physics.Raycast(ray, out hit))
                        {
                            Collider coll = hit.collider;
                            if (hit.collider.name == "GroundMarker")
                            {
                                print("GroundMarker");
                                coll = hit.collider.gameObject.transform.parent.GetComponent<Collider>();
                                print(coll.name);
                            }
                            if (coll.gameObject.name.Contains("Ship"))
                            {
                                if (selectedShip != null)
                                {
                                    setTargetShip(coll.gameObject);
                                    Vector3 showPos = coll.gameObject.transform.position;
                                    showPos.y = 100f;
                                    marker.transform.position = showPos;
                                    //marker.transform.rotation = selectedShip.GetComponent<JPShip>().targetRotation;
                                }
                            }
                            else
                            {
                                if (targetShip == null)
                                {
                                    groundPlane.Raycast(ray, out rayDistance);
                                    Vector3 pos = ray.GetPoint(rayDistance);
                                    pos.y = 50f;
                                    marker.transform.position = pos;
                                }
                            }
                        }
                        else
                        {
                            if (targetShip == null)
                            {
                                groundPlane.Raycast(ray, out rayDistance);
                                Vector3 pos = ray.GetPoint(rayDistance);
                                pos.y = 50f;
                                marker.transform.position = pos;
                            }
                        }




                    }
                }
            }
        } else {
            if(targetMode == 1) {
                if(targetedShip != null) {
                    Vector3 showPos = targetedShip.transform.position;
                    showPos.y = 100f;
                    marker.transform.position = showPos;
                }
            }
        }    
    }

    void DeselectShip() {
        if (selectedShip != null)
        {
            selectedShip.GetComponent<JPShip>().leadController.SetSelected(false);
        }
        selectedShip = null;
        targetShip = null;
        targetMode = 0;
        tapCount = 0;
        marker.SetActive(false);
        //healthSlider.gameObject.SetActive(false);

        for (int count = 0; count < 3; count++)
        {
            uiSkillButtons[count].SetInvisible();

        }
        cancelButton.SetInvisible();
        //targetButton.SetInvisible();
        moveButton.SetInvisible();

        rotationSet = false;
        marker.GetComponent<MarkerController>().setRotationMode(false);
    }

    void selectShip (GameObject ship) {
        networkPlayer.CmdSelectShip (ship.name);
        for (int count = 0; count < 3; count++)
        {
            if (ship.GetComponent<JPShip>().skills[count] != null)
            {
                ship.GetComponent<JPShip>().skills[count].ConfigureSkill(uiSkillButtons[count]);
            } else {
                uiSkillButtons[count].SetInvisible();
            }
        }
        print ("Selected Ship " + ship.name);
        cancelButton.SetActive();
        //targetButton.SetActive();
        moveButton.SetActive();
        moveButton.toggleImage(0);
    }

    void setTargetShip (GameObject ship) {
        networkPlayer.CmdSetTargetShip (ship.name);
        print ("Targeted GameObject " + ship.name);
        targetShip = ship;
    }
    void setTargetPosition (Vector3 pos) {
        pos.y = 30.0f;
        networkPlayer.CmdSetPosition (pos);
        print ("Targeted position " + pos);
        targetShip = null;
    }
    void setTargetRotation(Quaternion rot)
    {
        networkPlayer.CmdSetRotation(rot);
        print("Targeted rotation " + rot);
        targetShip = null;
    }
    void setTargetMode(int mode)
    {
        
        networkPlayer.CmdSetMode(mode);
        print("Settting mode " + mode);
    }

    public void setModeMove () {
        //print("Mode Move");

        if (!isLocalPlayer)
        {
            return;
        }
        if (moveButton.getMode() == 0)
        {
            targetMode = 1;
            tapCount = 0;
            print("Mode Move 0");
            marker.GetComponent<MarkerController>().setRotationMode(false);

            marker.GetComponent<MarkerController>().setMode(1);

            moveButton.toggleImage(1);
        } else if(moveButton.getMode() == 1) {
            targetMode = 0;
            tapCount = 0;
            print("Mode Target 1");

            marker.GetComponent<MarkerController>().setRotationMode(selectedShip.GetComponent<JPShip>().rotationControl);
            marker.transform.rotation = selectedShip.GetComponent<JPShip>().targetRotation;

            marker.GetComponent<MarkerController>().setMode(0);

            moveButton.toggleImage(0);
        }

    }
    public void setModeTarget()
    {
        //print("Mode Target ");

        if (!isLocalPlayer)
        {
            return;
        }
        targetMode = 1;
        tapCount = 0;
        print("Mode Target 1");

        moveButton.toggleImage(1);

    }
    public void setModeCancel () {
        

        if (!isLocalPlayer)
        {
            print("Not local player!");
            return;
        }
        print("Local player! Cancel");
        if (selectedShip != null)
        {
            selectedShip.GetComponent<JPShip>().SetSelected(false);
        }
        selectedShip = null;
        targetShip = null;
        print("Mode Cancel " + targetMode);
        targetMode = 0;
        marker.SetActive(false);

        currentSkill = null;
        currentButton = null;
        skillActive = false;
        skillTimingSpacing = false;

    }

    public void SetDefendMode () {
        setTargetMode(0);
    }
    public void SetAttackMode()
    {
        setTargetMode(1);
    }
    public void SetSpeedMode () {
        setTargetMode(2);
    }

    public void SkillPressed (int skillNum, UISkill sendingSkill) {
        currentSkill = null;
        skillIndex = skillNum;
        //tapCount = 0;
        BaseSkill tempSkill = selectedShip.GetComponent<JPShip>().skills[skillNum];
        if((!tempSkill.gameObjectRequired) && (!tempSkill.locationRequired)) {
            currentButton = sendingSkill;
            SetSkill();
            skillActive = true;
            skillTimingSpacing = false;
        } else {
            currentSkill = tempSkill;
            currentButton = sendingSkill;
            skillActive = true;
            skillTimingSpacing = true;
        }
        selectedShip.GetComponent<JPShip>().skills[skillNum].TriggerSkillPreview(false);
    }
    public void SkillPreview (int skillNum, UISkill sendingSkill) {
        print("Input preview " + skillNum);
        selectedShip.GetComponent<JPShip>().skills[skillNum].TriggerSkillPreview(true);
    }
    void SetSkill()
    {
        networkPlayer.CmdSetSkill(skillIndex);
        currentButton.BeginCooldown();
        print("Regular Skill Activated " + skillIndex);
        //skillActive = false;
    }
    void SetSkillLocation(Vector3 pos)
    {
        if(skillTimingSpacing) {
            skillTimingSpacing = false;
            return;
        }
        networkPlayer.CmdSetSkillLocation(skillIndex, pos);
        currentButton.BeginCooldown();
        print("Location Skill Activated " + pos);
        skillActive = false;
    }
    void SetSkillTarget(GameObject target)
    {
        if (skillTimingSpacing)
        {
            skillTimingSpacing = false;
            return;
        }
        networkPlayer.CmdSetSkillTarget(skillIndex, target);
        currentButton.BeginCooldown();
        print("GameObject Skill Activated " + target.name);
        skillActive = false;
    }

    void Retreat () {
        networkPlayer.CmdRetreat();
        GameObject.Find("NetworkManager").GetComponent<JPUINetworkManager>().Disconnect();
    }

    void QuitGame()
    {
        if(isServer) {
            GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StopHost();
        }
        GameObject.Find("NetworkManager").GetComponent<JPUINetworkManager>().Disconnect();
    }

    public void VRShoot(string shipName)
    {
        networkPlayer.CmdFireVRShip(shipName);
    }

 
}