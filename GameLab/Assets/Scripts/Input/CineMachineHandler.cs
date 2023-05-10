using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CineMachineHandler : MonoBehaviour
{
    public CinemachineFreeLook[] cameras;
    public TextMeshProUGUI[] playerTimeTexts;

    // Start is called before the first frame update
    void Start()
    {
        SetupCineMachines();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetupCineMachines()
    {
        for (int i = 0; i < GameManager.instance.GetAmountOfPlayers(); i++)
        {
            cameras[i].gameObject.SetActive(true);
        }

        switch (GameManager.instance.GetAmountOfPlayers())
        {
            case 2:
                cameras[0].Follow = GameManager.instance.currentPlayers[0].transform;
                cameras[0].LookAt = GameManager.instance.currentPlayers[0].transform;
                cameras[0].m_YAxis.m_InputAxisName = "VerticalCamera1";
                cameras[0].m_XAxis.m_InputAxisName = "HorizontalCamera1";


                cameras[1].Follow = GameManager.instance.currentPlayers[1].transform;
                cameras[1].LookAt = GameManager.instance.currentPlayers[1].transform;
                cameras[1].m_YAxis.m_InputAxisName = "VerticalCamera2";
                cameras[1].m_XAxis.m_InputAxisName = "HorizontalCamera2";
               
                break;
            case 3:
                cameras[0].Follow = GameManager.instance.currentPlayers[0].transform;
                cameras[0].LookAt = GameManager.instance.currentPlayers[0].transform;
                cameras[0].m_YAxis.m_InputAxisName = "VerticalCamera1";
                cameras[0].m_XAxis.m_InputAxisName = "HorizontalCamera1";


                cameras[1].Follow = GameManager.instance.currentPlayers[1].transform;
                cameras[1].LookAt = GameManager.instance.currentPlayers[1].transform;
                cameras[1].m_YAxis.m_InputAxisName = "VerticalCamera2";
                cameras[1].m_XAxis.m_InputAxisName = "HorizontalCamera2";

                cameras[2].Follow = GameManager.instance.currentPlayers[2].transform;
                cameras[2].LookAt = GameManager.instance.currentPlayers[2].transform;
                cameras[2].m_YAxis.m_InputAxisName = "VerticalCamera3";
                cameras[2].m_XAxis.m_InputAxisName = "HorizontalCamera3";
                break;
            case 4:
                cameras[0].Follow = GameManager.instance.currentPlayers[0].transform;
                cameras[0].LookAt = GameManager.instance.currentPlayers[0].transform;
                cameras[0].m_YAxis.m_InputAxisName = "VerticalCamera1";
                cameras[0].m_XAxis.m_InputAxisName = "HorizontalCamera1";

                cameras[1].Follow = GameManager.instance.currentPlayers[1].transform;
                cameras[1].LookAt = GameManager.instance.currentPlayers[1].transform;
                cameras[1].m_YAxis.m_InputAxisName = "VerticalCamera2";
                cameras[1].m_XAxis.m_InputAxisName = "HorizontalCamera2";

                cameras[2].Follow = GameManager.instance.currentPlayers[2].transform;
                cameras[2].LookAt = GameManager.instance.currentPlayers[2].transform;
                cameras[2].m_YAxis.m_InputAxisName = "VerticalCamera3";
                cameras[2].m_XAxis.m_InputAxisName = "HorizontalCamera3";

                cameras[3].Follow = GameManager.instance.currentPlayers[3].transform;
                cameras[3].LookAt = GameManager.instance.currentPlayers[3].transform;
                cameras[3].m_YAxis.m_InputAxisName = "VerticalCamera4";
                cameras[3].m_XAxis.m_InputAxisName = "HorizontalCamera4";
                break;
            default:
                break;
        }

        SetupTexts(GameManager.instance.GetAmountOfPlayers());
    }


    public void SetupTexts(int amount) 
    {
        for (int i = 0; i < amount; i++)
        {
            playerTimeTexts[i].gameObject.SetActive(true);
        }
    }


}
