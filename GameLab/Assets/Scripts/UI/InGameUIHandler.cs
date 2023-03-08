using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIHandler : MonoBehaviour
{
    public GameObject[] panels;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.canSelect = true;
    }

    public void SetupPanels()
    {
        switch (GameManager.instance.amountOfPlayers)
        {
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

    // Update is called once per frame
    void Update()
    {

    }

    public void ChooseCharacter(int character)
    {
        switch (GameManager.instance.order)
        {
            case 0:
                GameManager.instance.currentPlayers[GameManager.instance.order].SelectedCharacter((Character)character);
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
        //Reference to the player who pressed it
        GameManager.instance.order++;

    }

}
