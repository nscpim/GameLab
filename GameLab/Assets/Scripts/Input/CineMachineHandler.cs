using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineMachineHandler : MonoBehaviour
{
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
        switch (GameManager.instance.GetAmountOfPlayers())
        {
            case 1:
                break;
            case 2:
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
