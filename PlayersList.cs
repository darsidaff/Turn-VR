using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using TMPro;

public class PlayersList : MonoBehaviour
{
    public GameObject TPlayerString;      // Префаб строки
    private Transform BoardPosition;
    public static bool ShowBoard = true;

    public GameObject Scroll;

    public GameObject player;

    public static int k = 0;

    public SteamVR_Behaviour_Pose Pos = null; // Хранит правый контроллер - поле назначается из редактора Unity
    private SteamVR_Action_Boolean buttonGrabPinch = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    private SteamVR_Action_Boolean buttonGrabGrip = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");

    public GameObject RightHand;   // Правый контроллер 
    public GameObject PLinePref;   // Префаб рендер-луча
    private GameObject PLine;      // Рендер-луч 
    private bool OnPress;

    public GameObject startButton;
    public GameObject skiPlayer;
    public Rigidbody playerRig;

    public GameObject funic;
    public GameObject funic1;

    public static bool isFunic = false;

    public GameObject telep;

    void Start()
    {
        GameObject Item;

        BoardPosition = gameObject.transform;
        ReadWriteFile.LoadFromFile();

        PLine = Instantiate(PLinePref, RightHand.transform.position, RightHand.transform.rotation) as GameObject;


        for (int i = 0; i < 10; i++)
        {
            Item = Instantiate(TPlayerString, BoardPosition) as GameObject;
            Item.transform.localPosition = new Vector3(-0.3f, 4.13f - i * 0.6f, 0.576f);
            ReadWriteFile.playersList.Add(Item);
        }

        Scroll.transform.localScale = new Vector3(1f, (4.5815f /  ReadWriteFile.nPlayers), 1f);
    }

    private void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if ((i + 1 + k) < 10)
            {
                ReadWriteFile.playersList[i].GetComponentInChildren<TextMesh>().text = (i + 1 + k).ToString() + "." + ReadWriteFile.listPlayers[i + k].playerName + ReadWriteFile.LengthAlignment(ReadWriteFile.listPlayers[i + k].playerName) + ReadWriteFile.listPlayers[i + k].score;
            }

            else ReadWriteFile.playersList[i].GetComponentInChildren<TextMesh>().text = (i + 1 + k).ToString() + "." + ReadWriteFile.listPlayers[i + k].playerName + ReadWriteFile.LengthAlignment(ReadWriteFile.listPlayers[i + k].playerName + " ") + ReadWriteFile.listPlayers[i + k].score;

        }

        if (Input.GetKeyDown(KeyCode.R) && k > 0)
        {
            k--;
        }

        if (Input.GetKeyDown(KeyCode.F) && k < 40)
        {
            k++;
        }

        Scroll.transform.localPosition = new Vector3(-5.89f, 3.736f - (k * 4.5815f / 50f * 1.24f), 0.624f);

        float dist = Vector3.Distance(player.transform.position, transform.position);
        float distStart = Vector3.Distance(player.transform.position, startButton.transform.position);
        float distFunic = Vector3.Distance(player.transform.position, funic.transform.position);
        float distFunic1 = Vector3.Distance(player.transform.position, funic1.transform.position);

        if (dist <= 15 || distStart <=15 || ((distFunic <= 15 || distFunic1 <= 15) && player.GetComponent<moveVR>().enabled == true))
        {

            PLine.transform.position = RightHand.transform.position;
            PLine.transform.rotation = RightHand.transform.rotation;

            PLine.SetActive(true);

            RaycastHit hit;
            Vector3 Temp = RightHand.transform.TransformDirection(Vector3.forward);

            if (Physics.Raycast(RightHand.transform.position, Temp, out hit))
            {
                if (hit.transform.tag == "Up" && buttonGrabPinch.GetStateDown(Pos.inputSource) && OnPress == false && PlayersList.k > 0) PlayersList.k--;
                if (hit.transform.tag == "Down" && buttonGrabPinch.GetStateDown(Pos.inputSource) && OnPress == false && PlayersList.k < 40) PlayersList.k++;

                if (hit.transform.tag == "Start" && buttonGrabPinch.GetStateDown(Pos.inputSource) && OnPress == false)
                {
                    player.GetComponent<moveVR>().enabled = false;
                    player.transform.position = new Vector3(1173, 609f, 1681.027f);
                    player.transform.SetParent(skiPlayer.transform);
                    playerRig.isKinematic = true;
                    playerRig.useGravity = false;

                }

                if (hit.transform.tag == "Funic" && buttonGrabPinch.GetStateDown(Pos.inputSource) && OnPress == false)
                {
                    player.GetComponent<moveVR>().enabled = false;
                    player.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
                    player.transform.SetParent(hit.transform.parent.transform);
                    playerRig.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
                    isFunic = true;
                }
            }

            if (buttonGrabPinch.GetStateUp(Pos.inputSource)) { OnPress = false; }
        }
        else
        {
            PLine.SetActive(false);
        }

        if (player.GetComponent<moveVR>().enabled == false) telep.SetActive(false);
        else telep.SetActive(true);

    }

}
