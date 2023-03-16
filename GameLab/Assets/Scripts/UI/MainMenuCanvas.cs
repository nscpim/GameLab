using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    public GameObject[] menuButtons;
    // Start is called before the first frame update
    public void Start()
    {
        SetButtonStartPoint();
    }

    // Update is called once per frame
    public void Update()
    {

    }
    public void AmountOfPlayers(int amount)
    {
        GameManager.instance.SetAmountOfPlayers(amount);
        GameManager.LoadLevel(Levels.InGame);
        GameManager.GetManager<InputManager>().SetPlayerSchemes();
    }


    public void SetButtonStartPoint() 
    {
        GameManager.instance.eventSystem.SetSelectedGameObject(menuButtons[0]);
    }

}
