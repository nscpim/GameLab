using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUIHandler : MonoBehaviour
{
    [Header("Panels")]
    public GameObject PlayerSelectionPanel_2;
    public GameObject PlayerSelectionPanel_3;
    public GameObject PlayerSelectionPanel_4;

    [Header("Buttons")]
    public GameObject[] buttons2players;
    public GameObject[] buttons3players;
    public GameObject[] buttons4players;

    private Timer countdownTimer;
    private bool once = false;

    public TextMeshProUGUI countdownText;
    public GameObject pausePanel;

    // Start is called before the first frame updates
    void Start()
    {
        countdownTimer = new Timer();
        GameManager.instance.canSelect = true;
        SetupPanels(true);
        ResetSelection();
        //Dont forget to disable movement code 
        for (int i = 0; i < GameManager.instance.currentPlayers.Count; i++)
        {
            GameManager.instance.currentPlayers[i].canMove = false;
        }
    }

    public void SetupPanels(bool value)
    {
        switch (GameManager.instance.amountOfPlayers)
        {
            case 2:
                PlayerSelectionPanel_2.SetActive(value);
                break;
            case 3:
                PlayerSelectionPanel_3.SetActive(value);
                break;
            case 4:
                PlayerSelectionPanel_4.SetActive(value);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.order == (GameManager.instance.amountOfPlayers + 1) && !once)
        {
            SetupPanels(false);
            GameManager.instance.canSelect = false;
            once = true;
            countdownTimer.SetTimer(GameManager.instance.matchCountdown);
            Debug.Log("MATCH ABOUT TO START");
        }

        if (countdownTimer.isActive && countdownTimer.TimerDone())
        {
            countdownTimer.StopTimer();
            countdownText.transform.parent.gameObject.SetActive(false);
            Debug.Log("GAME STARTING");
            for (int i = 0; i < GameManager.instance.currentPlayers.Count; i++)
            {
                GameManager.instance.currentPlayers[i].canMove = true;
            }

        }
        if (countdownTimer.TimeLeft() >= 0f && countdownTimer.isActive)
        {
            UpdateUI(Mathf.RoundToInt(countdownTimer.TimeLeft()).ToString(), countdownText);
        }

        SetActivePanel(pausePanel, GameManager.paused);

    }

    public void ChooseCharacter(int character)
    {
        //-1 because arrays start at 0
        GameManager.instance.currentPlayers[GameManager.instance.order - 1].SelectedCharacter((Character)character);
        ResetSelection();
    }

    public void ResetSelection()
    {
        //Amount of Players
        switch (GameManager.instance.amountOfPlayers)
        {
            case 2:
                //Order of the players
                switch (GameManager.instance.order)
                {
                    case 1:
                        GameManager.instance.eventSystem.SetSelectedGameObject(buttons2players[0]);
                        break;
                    case 2:
                        GameManager.instance.eventSystem.SetSelectedGameObject(buttons2players[4]);
                        break;
                    default:
                        break;
                }
                break;
            case 3:
                switch (GameManager.instance.order)
                {
                    case 1:
                        GameManager.instance.eventSystem.SetSelectedGameObject(buttons3players[0]);
                        break;
                    case 2:
                        GameManager.instance.eventSystem.SetSelectedGameObject(buttons3players[4]);
                        break;
                    case 3:
                        GameManager.instance.eventSystem.SetSelectedGameObject(buttons3players[8]);
                        break;
                    default:
                        break;
                }
                break;
            case 4:
                switch (GameManager.instance.order)
                {
                    case 1:
                        GameManager.instance.eventSystem.SetSelectedGameObject(buttons4players[0]);
                        break;
                    case 2:
                        GameManager.instance.eventSystem.SetSelectedGameObject(buttons4players[4]);
                        break;
                    case 3:
                        GameManager.instance.eventSystem.SetSelectedGameObject(buttons4players[8]);
                        break;
                    case 4:
                        GameManager.instance.eventSystem.SetSelectedGameObject(buttons4players[12]);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

    }

    public void SetActivePanel(GameObject panel, bool state)
    {
        panel.SetActive(state);
    }

    public void UpdateUI(string message, TextMeshProUGUI text)
    {
        text.text = message;
    }

}
