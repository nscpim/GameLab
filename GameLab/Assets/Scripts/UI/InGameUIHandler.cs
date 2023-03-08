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
        for (int i = 0; i < GameManager.instance.amountOfPlayers; i++)
        {
            panels[i].gameObject.SetActive(true);
        }

        GameManager.instance.canSelect = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChooseCharacter(Character character)
    {
        switch (GameManager.instance.order)
        {
            case 0:
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
