using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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


    [Header("Abilities")]
    public GameObject[] Abilities2Players;
    public GameObject[] Abilities3Players;
    public GameObject[] Abilities4Players;

    [Header("VariableChangeUI")]
    public Transform variablesPanel;
    public TMP_InputField moveSpeedField, abilityCooldown, jumpSpeed, gravity, maxSpeed, acceleration, startingSpeed;

    private Timer countdownTimer;

    private bool once = false;

    public TextMeshProUGUI countdownText;
    public GameObject pausePanel;
    public GameObject toolTipPanel;

    public float matchTimer;
    private Timer matchTimerUpdater;

    public bool matchStarted = false;
    private bool activeState = false;

    public TextMeshProUGUI matchTimerText;

    public bool[] soundBools;

    // Start is called before the first frame updates
    void Start()
    {
        countdownTimer = new Timer();
        matchTimerUpdater = new Timer();
        GameManager.instance.SetCanMove(false);
        GameManager.instance.canSelect = true;
        matchTimer = 0f;
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

    public void OnTextFieldChange()
    {
        for (int i = 0; i < GameManager.instance.currentPlayers.Count; i++)
        {
            GameManager.instance.currentPlayers[i].character.speed = float.Parse(moveSpeedField.text);
            GameManager.instance.currentPlayers[i].character.abilityCooldown = float.Parse(abilityCooldown.text);
            GameManager.instance.currentPlayers[i].character.jumpSpeed = float.Parse(jumpSpeed.text);
            GameManager.instance.currentPlayers[i].character.acceleration = float.Parse(acceleration.text);
            GameManager.instance.currentPlayers[i].character.gravity = float.Parse(gravity.text);
            GameManager.instance.currentPlayers[i].character.maxSpeed = float.Parse(maxSpeed.text);
            //GameManager.instance.currentPlayers[i].character.startingSpeed = float.Parse(startingSpeed.text);
        }
    }


    // Update is called once per frame
    void Update()
    {

        GameManager.instance.currentMatchCountdown = countdownTimer.TimeLeft();

        if (Input.GetKeyDown(KeyCode.L))
        {
            activeState = !activeState;
            variablesPanel.gameObject.SetActive(activeState);
        }

        if (GameManager.instance.order == (GameManager.instance.amountOfPlayers + 1) && !once)
        {
            SetupPanels(false);
            GameManager.instance.canSelect = false;
            toolTipPanel.SetActive(false);
            once = true;
            GameManager.GetManager<AudioManager>().StopPlaying();
            countdownTimer.SetTimer(GameManager.instance.matchCountdown);
            // Debug.Log("MATCH ABOUT TO START");
        }

        if (countdownTimer.isActive && countdownTimer.TimerDone())
        {
            countdownTimer.StopTimer();
            countdownText.transform.parent.gameObject.SetActive(false);
            matchTimerUpdater.SetTimer(1f);
            matchStarted = true;
            foreach (ThirdPersonMovement i in GameManager.instance.currentPlayers)
            {
                i.ChangeCameras();
            }
            GameManager.instance.SetCanMove(true);
            GameManager.GetManager<AudioManager>().PlayMusic("ingame");

           /* switch (GameManager.instance.amountOfPlayers)
            {
                case 2:
                    Abilities2Players[0].gameObject.SetActive(true);
                    Abilities2Players[1].gameObject.SetActive(false);
                    Abilities2Players[2].gameObject.SetActive(false);
                    break;
                case 3:
                    Abilities2Players[0].gameObject.SetActive(false);
                    Abilities2Players[1].gameObject.SetActive(true);
                    Abilities2Players[2].gameObject.SetActive(false);
                    break;
                case 4:
                    Abilities2Players[0].gameObject.SetActive(false);
                    Abilities2Players[1].gameObject.SetActive(false);
                    Abilities2Players[2].gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
           */

            // Debug.Log("GAME STARTING");
            for (int i = 0; i < GameManager.instance.currentPlayers.Count; i++)
            {
                GameManager.instance.currentPlayers[i].canMove = true;
            }

        }
        if (countdownTimer.TimeLeft() >= 0f && countdownTimer.isActive)
        {
            UpdateUI(Mathf.RoundToInt(countdownTimer.TimeLeft()).ToString(), countdownText);
        }
        if (countdownTimer.isActive)
        {
            switch (Mathf.RoundToInt(countdownTimer.TimeLeft()))
            {
                case 3:
                    if (!soundBools[0])
                    {
                        soundBools[0] = true;
                        Debug.Log("Played Audio 3");
                        GameManager.GetManager<AudioManager>().PlaySound("three", false, Vector3.zero, false, null);
                    }
                    break;
                case 2:
                    if (!soundBools[1])
                    {
                        soundBools[1] = true;
                        Debug.Log("Played Audio 2");
                        GameManager.GetManager<AudioManager>().PlaySound("two", false, Vector3.zero, false, null);
                    }
                    break;
                case 1:
                    if (!soundBools[2])
                    {
                        soundBools[2] = true;
                        Debug.Log("Played Audio 1");
                        GameManager.GetManager<AudioManager>().PlaySound("one", false, Vector3.zero, false, null);
                    }
                    break;
                case 0:
                    if (!soundBools[3])
                    {
                        soundBools[3] = true;
                        Debug.Log("Played Audio 0");
                        GameManager.GetManager<AudioManager>().PlaySound("sprint", false, Vector3.zero, false, null);
                    }
                    break;
                default:
                    break;
            }
        }
        SetActivePanel(pausePanel, GameManager.paused);

        if (matchStarted)
        {
            matchTimer += Time.deltaTime;
        }

        if (matchTimerUpdater.isActive && matchTimerUpdater.TimerDone())
        {
            matchTimerUpdater.StopTimer();
            float totalSeconds = matchTimer;
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
            GameManager.GetManager<UIManager>().UpdateUI(matchTimerText, time.ToString("mm':'ss"));
            matchTimerUpdater.SetTimer(1f);
        }

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
