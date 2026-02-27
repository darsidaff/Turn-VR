using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using Valve.VR.InteractionSystem;
using TMPro;


public class KeyBoard : MonoBehaviour
{
    public GameObject TLette;      // Префаб буквы
    public GameObject PName;       // Компонент для отображения имени игрока 
    public GameObject RightHand;   // Правый контроллер 
    public GameObject PLinePref;   // Префаб рендер-луча
    private GameObject PLine;      // Рендер-луч   
    private string PlayerName;     // Имя игрока
    private bool OnPress;
    private int k;

    public GameObject player;

    private string Alphabet = "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ";

    private Interactable interactable;
    public SteamVR_Behaviour_Pose Pos = null; // Хранит правый контроллер - поле назначается из редактора Unity
    private SteamVR_Action_Boolean buttonGrabPinch = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    private SteamVR_Action_Boolean buttonGrabGrip = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");


    void Start()
    {
        interactable = GetComponent<Interactable>();

        PLine = Instantiate(PLinePref, RightHand.transform.position, RightHand.transform.rotation) as GameObject;

        Transform KeyBoardPosition = gameObject.transform;
        GameObject Item;
        PlayerName = "";
        OnPress = false;
        k = 0;


        for (int i = 0; i < 12; i++)
        {
            Item = Instantiate(TLette, KeyBoardPosition) as GameObject;
            Item.GetComponentInChildren<TextMesh>().text = (Alphabet[k++]).ToString();
            Item.transform.localPosition = new Vector3(0, 2.1f, 0 - i * 0.6f);
        }

        for (int i = 12, j = 0; i < 23; i++, j++)
        {
            Item = Instantiate(TLette, KeyBoardPosition) as GameObject;
            Item.GetComponentInChildren<TextMesh>().text = (Alphabet[k++]).ToString();
            Item.transform.localPosition = new Vector3(0, 1.5f, -0.3f - j * 0.6f);
        }

        for (int i = 23, j = 0; i < 32; i++, j++)
        {
            Item = Instantiate(TLette, KeyBoardPosition) as GameObject;
            Item.GetComponentInChildren<TextMesh>().text = (Alphabet[k++]).ToString();
            Item.transform.localPosition = new Vector3(0, 0.9f, -0.9f - j * 0.6f);
        }

        Item = Instantiate(TLette, KeyBoardPosition) as GameObject;
        Item.GetComponentInChildren<TextMesh>().text = ('\u2190').ToString();
        Item.transform.localPosition = new Vector3(0.02f, 0.3f, -6.601f);
        Item = Instantiate(TLette, KeyBoardPosition) as GameObject;
        Item.GetComponentInChildren<TextMesh>().text = ('\u21B2').ToString();
        Item.transform.localPosition = new Vector3(0, 0.3f, -5.992f);
    }


    private void Update()
    {
        PLine.transform.position = RightHand.transform.position;
        PLine.transform.rotation = RightHand.transform.rotation;

        RaycastHit hit;
        Vector3 Temp = RightHand.transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(RightHand.transform.position, Temp, out hit))
        {
            if (hit.transform.tag == "Lette" && buttonGrabPinch.GetStateDown(Pos.inputSource) && OnPress == false)
            {
                GameObject TempParent = hit.transform.parent.gameObject;
                    
                if (TempParent.GetComponentInChildren<TextMesh>().text == ('\u2190').ToString())
                {
                    PlayerName = PlayerName.Substring(0, PlayerName.Length - 1);
                    PName.GetComponent<TMP_Text>().text = PlayerName;
                }
                else if (TempParent.GetComponentInChildren<TextMesh>().text == ('\u21B2').ToString())
                {
                    PlayerName = PName.GetComponent<TMP_Text>().text;
                    ReadWriteFile.PlayerInfo.playerName = PlayerName;
                    PlayersList.ShowBoard = true;
                    PLine.SetActive(false);
                    gameObject.SetActive(false);
                    player.GetComponent<moveVR>().enabled = true;
                }
                else if (PlayerName.Length < 11) 
                {
                    PlayerName = PlayerName + TempParent.GetComponentInChildren<TextMesh>().text;
                    PName.GetComponent<TMP_Text>().text = PlayerName;
                }
                OnPress = true;
            }
        }

        if (buttonGrabPinch.GetStateUp(Pos.inputSource)) { OnPress = false; }
    }

}
