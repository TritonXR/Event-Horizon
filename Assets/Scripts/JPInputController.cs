using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JPInputController : NetworkBehaviour {
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

    float minPullDistance = 50f;
    int tapCount = 0;

    BaseSkill currentSkill;
    UISkill currentButton;
    bool skillActive = false;
    int skillIndex;
    UISkill[] uiSkillButtons = new UISkill[3];
    UIButton cancelButton;
    UIButton targetButton;
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
        localUI = GetComponent<JPUIController>();
        marker.SetActive(false);

        GameObject.Find("UICanvas").transform.Find("Skill1").gameObject.GetComponent<UISkill>().localPlayerController = this;
        uiSkillButtons[0] = GameObject.Find("UICanvas").transform.Find("Skill1").gameObject.GetComponent<UISkill>();
        GameObject.Find("UICanvas").transform.Find("Skill2").gameObject.GetComponent<UISkill>().localPlayerController = this;
        uiSkillButtons[1] = GameObject.Find("UICanvas").transform.Find("Skill2").gameObject.GetComponent<UISkill>();
        GameObject.Find("UICanvas").transform.Find("Skill3").gameObject.GetComponent<UISkill>().localPlayerController = this;
        uiSkillButtons[2] = GameObject.Find("UICanvas").transform.Find("Skill3").gameObject.GetComponent<UISkill>();

        cancelButton = GameObject.Find("UICanvas").transform.Find("CancelButton").gameObject.GetComponent<UIButton>();
        targetButton = GameObject.Find("UICanvas").transform.Find("TargetButton").gameObject.GetComponent<UIButton>();;
        moveButton = GameObject.Find("UICanvas").transform.Find("MoveButton").gameObject.GetComponent<UIButton>();;
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
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked down on the UI");
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
                    if (hit.collider.gameObject.transform.name.Contains("Ship"))
                    {
                        if (((tapCount > 0) || (selectedShip == null)) && (hit.collider.gameObject.name.Contains("Player" + networkPlayer.playerNumber)))
                        {
                            if (tapCount > 0)
                            {

                                DeselectShip();
                            }

                            if (selectedShip != null)
                            {
                                selectedShip.GetComponent<JPShip>().leadController.SetSelected(false);
                            }
                            if (hit.collider.gameObject.name.Contains("Fighter"))
                            {
                                //Debug.Log("Got Squad lead " + hit.collider.transform.parent.name);
                                selectedShip = hit.collider.transform.parent.gameObject;
                                selectedShip.GetComponent<JPShip>().leadController.SetSelected(true);
                                selectShip(selectedShip);
                            }
                            else
                            {
                                selectedShip = hit.collider.gameObject;
                                selectedShip.GetComponent<JPShip>().leadController.SetSelected(true);
                                selectShip(selectedShip);
                            }
                            marker.SetActive(true);
                            selectStart = true;
                            selectStartPos = selectedShip.transform.position;

                            tapCount = 0;
                            //targetMode = 0;
                            //healthSlider.gameObject.SetActive(true);
                            //healthSlider.value = selectedShip.GetComponent<JPShip>().healthPercent;
                        } else {
                            tapCount++;
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
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked Up on the UI");
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
                        if ((Physics.Raycast(ray, out hit)) && (hit.collider.gameObject.name.Contains("Ship")))
                        {

                            //hit.collider.gameObject;
                            SetSkillTarget(hit.collider.gameObject);
                            tapCount = 2;
                              
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
                } else {
                    //print("Targeting Sent");
                    // Ship Targeted
                    if (targetMode == 1)
                    {
                        if ((Physics.Raycast(ray, out hit)) && (hit.collider.gameObject.name.Contains("Ship")))
                        {
                            if (selectedShip != null)
                            {
                                setTargetShip(hit.collider.gameObject);
                                marker.transform.position = hit.collider.gameObject.transform.position;
                                targetedShip = hit.collider.gameObject;
                                tapCount = 2;
                                //marker.transform.rotation = selectedShip.GetComponent<JPShip>().targetRotation;
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
                                    setTargetPosition(ray.GetPoint(rayDistance));
                                    marker.transform.position = ray.GetPoint(rayDistance);
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

            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked Move on the UI");

            }
            else
            {
                if (selectedShip)
                {
                    //RaycastHit hit;
                    if (targetMode == 0)
                    {
                        marker.GetComponent<MarkerController>().setMode(0);
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        float rayDistance;
                        groundPlane.Raycast(ray, out rayDistance);
                        if ((!selectStart) || (Vector3.Distance(selectStartPos, ray.GetPoint(rayDistance)) > minPullDistance))
                        {

                            marker.transform.position = ray.GetPoint(rayDistance);
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

                        if ((Physics.Raycast(ray, out hit)) && (hit.collider.gameObject.name.Contains("Ship")))
                        {
                            if (selectedShip != null)
                            {
                                setTargetShip(hit.collider.gameObject);
                                Vector3 showPos = hit.collider.gameObject.transform.position;
                                showPos.y = 50f;
                                marker.transform.position = showPos;
                                //marker.transform.rotation = selectedShip.GetComponent<JPShip>().targetRotation;
                            }
                        }
                        else
                        {

                            groundPlane.Raycast(ray, out rayDistance);
                            marker.transform.position = ray.GetPoint(rayDistance);
                        }




                    }
                }
            }
        } else {
            if(targetMode == 1) {
                if(targetedShip != null) {
                    Vector3 showPos = targetedShip.transform.position;
                    showPos.y = 50f;
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
        targetButton.SetInvisible();
        moveButton.SetInvisible();
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
        targetButton.SetActive();
        moveButton.SetActive();
    }

    void setTargetShip (GameObject ship) {
        networkPlayer.CmdSetTargetShip (ship.name);
        print ("Targeted GameObject " + ship.name);
    }
    void setTargetPosition (Vector3 pos) {
        pos.y = 14.0f;
        networkPlayer.CmdSetPosition (pos);
        print ("Targeted position " + pos);
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
        targetMode = 0;
        tapCount = 0;
        print("Mode Move 0");

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
        BaseSkill tempSkill = selectedShip.GetComponent<JPShip>().skills[skillNum];
        if((!tempSkill.gameObjectRequired) && (!tempSkill.locationRequired)) {
            currentButton = sendingSkill;
            SetSkill();
            skillActive = true;
        } else {
            currentSkill = tempSkill;
            currentButton = sendingSkill;
            skillActive = true;
        }
    }

    void SetSkill()
    {
        networkPlayer.CmdSetSkill(skillIndex);
        currentButton.BeginCooldown();
        print("Regular Skill Activated");
    }
    void SetSkillLocation(Vector3 pos)
    {
        networkPlayer.CmdSetSkillLocation(skillIndex, pos);
        currentButton.BeginCooldown();
        print("Location Skill Activated " + pos);
    }
    void SetSkillTarget(GameObject target)
    {
        networkPlayer.CmdSetSkillTarget(skillIndex, target);
        currentButton.BeginCooldown();
        print("GameObject Skill Activated " + target.name);
    }
   

}