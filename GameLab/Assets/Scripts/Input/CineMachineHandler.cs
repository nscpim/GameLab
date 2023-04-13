using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineMachineHandler : MonoBehaviour
{
    public CinemachineFreeLook[] cameras;

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
                break;
            case 4:
                break;
            default:
                break;
        }

    }


}
