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


    public GameObject NewSelectionPanel;
    public GameObject[] buttonsNewSelection;

    [Header("Abilities")]
    public GameObject[] AbilityPanels;


    public TextMeshProUGUI[] Ability1CooldownTexts;
    public TextMeshProUGUI[] Ability2CooldownTexts;
    public TextMeshProUGUI[] playerThatsSelecting;


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

    private int placeInArray = 0;
    public TextMeshProUGUI matchTimerText;

    public bool[] soundBools;

    // Start is called before the first frame updates
    void Start()
    {
        countdownTimer = new Timer();
        matchTimerUpdater = new Timer();
        GameManager.instance.SetCanMove(false);
        GameManager.instance.canSelect = true;
        GameManager.GetManager<AudioManager>().PlaySound("playeronechoose", false, Vector3.zero, false, null);
        Invoke("ChooseYourCharacter", 1.5f);
        matchTimer = 0f;
        SetupPanels();
        ResetSelection();
        //Dont forget to disable movement code 
        for (int i = 0; i < GameManager.instance.currentPlayers.Count; i++)
        {
            GameManager.instance.currentPlayers[i].canMove = false;
        }
    }

    public void ChooseYourCharacter() 
    {
        GameManager.GetManager<AudioManager>().PlaySound("choosecharacter", false, Vector3.zero, false, null);
    }

    public void SetupPanels()
    {

        for (int i = 0; i < playerThatsSelecting.Length; i++)
        {
            playerThatsSelecting[i].gameObject.SetActive(true);
            playerThatsSelecting[i].text = String.Format("Player  {0}  is selecting", GameManager.instance.order);
        }

        
        
      
        /*
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
    */
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
        print(GameManager.instance.order);

       

        AbilityCooldown1();

        GameManager.instance.currentMatchCountdown = countdownTimer.TimeLeft();

        if (Input.GetKeyDown(KeyCode.L))
        {
            activeState = !activeState;
            variablesPanel.gameObject.SetActive(activeState);
        }

        if (GameManager.instance.order >= (GameManager.instance.amountOfPlayers + 1) && !once)
        {
            GameManager.instance.canSelect = false;
            NewSelectionPanel.gameObject.SetActive(false);
            for (int i = 0; i < playerThatsSelecting.Length; i++)
            {
                playerThatsSelecting[i].gameObject.SetActive(false);
            }
         
            toolTipPanel.SetActive(false);
            once = true;
            GameManager.GetManager<AudioManager>().StopPlaying();
            countdownTimer.SetTimer(GameManager.instance.matchCountdown);
            // Debug.Log("MATCH ABOUT TO START");
        }

        if (Input.GetButtonDown("UIPositive" + GameManager.instance.order) && GameManager.instance.canSelect)
        {
            placeInArray += 1;
            if (placeInArray >= 3)
            {
                placeInArray = 3;
            }
            for (int i = 0; i < buttonsNewSelection.Length; i++)
            {
                buttonsNewSelection[i].gameObject.SetActive(false);
            }
            buttonsNewSelection[placeInArray].gameObject.SetActive(true);
            GameManager.instance.eventSystem.SetSelectedGameObject(buttonsNewSelection[placeInArray].transform.GetChild(0).gameObject);
        }
        if (Input.GetButtonDown("UINegative" + GameManager.instance.order) && GameManager.instance.canSelect)
        {
            placeInArray -= 1;
            if (placeInArray <= 0)
            {
                placeInArray = 0;
            }
            for (int i = 0; i < buttonsNewSelection.Length; i++)
            {
                buttonsNewSelection[i].gameObject.SetActive(false);
            }
            buttonsNewSelection[placeInArray].gameObject.SetActive(true);
            GameManager.instance.eventSystem.SetSelectedGameObject(buttonsNewSelection[placeInArray].transform.GetChild(0).gameObject);
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

            switch (GameManager.instance.amountOfPlayers)
            {
                case 2:
                    AbilityPanels[0].gameObject.SetActive(true);
                    AbilityPanels[1].gameObject.SetActive(false);
                    AbilityPanels[2].gameObject.SetActive(false);
                    break;
                case 3:
                    AbilityPanels[0].gameObject.SetActive(false);
                    AbilityPanels[1].gameObject.SetActive(true);
                    AbilityPanels[2].gameObject.SetActive(false);
                    break;
                case 4:
                    AbilityPanels[0].gameObject.SetActive(false);
                    AbilityPanels[1].gameObject.SetActive(false);
                    AbilityPanels[2].gameObject.SetActive(true);
                    break;
                default:
                    break;
            }


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

        AbilityCooldown2();

    }


    public void AbilityCooldown2()
    {
        for (int i = 0; i < Ability2CooldownTexts.Length; i++)
        {
            switch (i)
            {
                //2 Players
                case 0:
                    Ability2CooldownTexts[i].text = GameManager.instance.currentPlayers[0].ReturnSecondAbilityCooldown().ToString();
                    if (Ability2CooldownTexts[i].text == "0")
                    {
                        Ability2CooldownTexts[i].text = "Ready";
                    }
                    break;
                case 1:
                    Ability2CooldownTexts[i].text = GameManager.instance.currentPlayers[1].ReturnSecondAbilityCooldown().ToString();
                    if (Ability2CooldownTexts[i].text == "0")
                    {
                        Ability2CooldownTexts[i].text = "Ready";
                    }
                    break;
                //3players
                case 2:
                    Ability2CooldownTexts[i].text = GameManager.instance.currentPlayers[0].ReturnSecondAbilityCooldown().ToString();
                    if (Ability2CooldownTexts[i].text == "0")
                    {
                        Ability2CooldownTexts[i].text = "Ready";
                    }
                    break;
                case 3:
                    Ability2CooldownTexts[i].text = GameManager.instance.currentPlayers[1].ReturnSecondAbilityCooldown().ToString();
                    if (Ability2CooldownTexts[i].text == "0")
                    {
                        Ability2CooldownTexts[i].text = "Ready";
                    }
                    break;
                case 4:

                    if (GameManager.instance.currentPlayers.Count < 3)
                    {
                        return;
                    }
                    else
                    {
                        Ability2CooldownTexts[i].text = GameManager.instance.currentPlayers[2].ReturnSecondAbilityCooldown().ToString();
                        if (Ability2CooldownTexts[i].text == "0")
                        {
                            Ability2CooldownTexts[i].text = "Ready";
                        }
                    }
                    break;
                //4 players
                case 5:
                    Ability2CooldownTexts[i].text = GameManager.instance.currentPlayers[0].ReturnSecondAbilityCooldown().ToString();
                    if (Ability2CooldownTexts[i].text == "0")
                    {
                        Ability2CooldownTexts[i].text = "Ready";
                    }
                    break;
                case 6:
                    Ability2CooldownTexts[i].text = GameManager.instance.currentPlayers[1].ReturnSecondAbilityCooldown().ToString();
                    if (Ability2CooldownTexts[i].text == "0")
                    {
                        Ability2CooldownTexts[i].text = "Ready";
                    }
                    break;
                case 7:
                    if (GameManager.instance.currentPlayers.Count < 3)
                    {
                        return;
                    }
                    else
                    {
                        Ability2CooldownTexts[i].text = GameManager.instance.currentPlayers[2].ReturnSecondAbilityCooldown().ToString();
                        if (Ability2CooldownTexts[i].text == "0")
                        {
                            Ability2CooldownTexts[i].text = "Ready";
                        }
                    }
                    break;
                case 8:
                    if (GameManager.instance.currentPlayers.Count < 4)
                    {
                        return;
                    }
                    else
                    {
                        Ability2CooldownTexts[i].text = GameManager.instance.currentPlayers[3].ReturnSecondAbilityCooldown().ToString();
                        if (Ability2CooldownTexts[i].text == "0")
                        {
                            Ability2CooldownTexts[i].text = "Ready";
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
    public void AbilityCooldown1()
    {

        for (int i = 0; i < Ability1CooldownTexts.Length; i++)
        {
            switch (i)
            {
                //2 Players
                case 0:
                    Ability1CooldownTexts[i].text = GameManager.instance.currentPlayers[0].ReturnAbilityCooldown().ToString();
                    if (Ability1CooldownTexts[i].text == "0")
                    {
                        Ability1CooldownTexts[i].text = "Ready";
                    }

                    break;
                case 1:
                    Ability1CooldownTexts[i].text = GameManager.instance.currentPlayers[1].ReturnAbilityCooldown().ToString();
                    if (Ability1CooldownTexts[i].text == "0")
                    {
                        Ability1CooldownTexts[i].text = "Ready";
                    }
                    break;
                //3players
                case 2:
                    Ability1CooldownTexts[i].text = GameManager.instance.currentPlayers[0].ReturnAbilityCooldown().ToString();
                    if (Ability1CooldownTexts[i].text == "0")
                    {
                        Ability1CooldownTexts[i].text = "Ready";
                    }
                    break;
                case 3:
                    Ability1CooldownTexts[i].text = GameManager.instance.currentPlayers[1].ReturnAbilityCooldown().ToString();
                    if (Ability1CooldownTexts[i].text == "0")
                    {
                        Ability1CooldownTexts[i].text = "Ready";
                    }
                    break;
                case 4:
                    if (GameManager.instance.currentPlayers.Count < 3)
                    {
                        return;
                    }
                    else
                    {
                        Ability1CooldownTexts[i].text = GameManager.instance.currentPlayers[2].ReturnAbilityCooldown().ToString();
                        if (Ability1CooldownTexts[i].text == "0")
                        {
                            Ability1CooldownTexts[i].text = "Ready";
                        }
                    }
                    break;
                //4 players
                case 5:
                    Ability1CooldownTexts[i].text = GameManager.instance.currentPlayers[0].ReturnAbilityCooldown().ToString();
                    if (Ability1CooldownTexts[i].text == "0")
                    {
                        Ability1CooldownTexts[i].text = "Ready";
                    }
                    break;
                case 6:
                    Ability1CooldownTexts[i].text = GameManager.instance.currentPlayers[1].ReturnAbilityCooldown().ToString();
                    if (Ability1CooldownTexts[i].text == "0")
                    {
                        Ability1CooldownTexts[i].text = "Ready";
                    }
                    break;
                case 7:
                    if (GameManager.instance.currentPlayers.Count < 3)
                    {
                        return;
                    }
                    else
                    {
                        Ability1CooldownTexts[i].text = GameManager.instance.currentPlayers[2].ReturnAbilityCooldown().ToString();
                        if (Ability1CooldownTexts[i].text == "0")
                        {
                            Ability1CooldownTexts[i].text = "Ready";
                        }
                    }
                    break;
                case 8:

                    if (GameManager.instance.currentPlayers.Count < 4)
                    {
                        return;
                    }
                    else
                    {
                        Ability1CooldownTexts[i].text = GameManager.instance.currentPlayers[3].ReturnAbilityCooldown().ToString();
                        if (Ability1CooldownTexts[i].text == "0")
                        {
                            Ability1CooldownTexts[i].text = "Ready";
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void ChooseCharacter(int character)
    {
        //-1 because arrays start at 0
        GameManager.instance.currentPlayers[GameManager.instance.order - 1].SelectedCharacter((Character)character);
        ResetSelection();
        for (int i = 0; i < playerThatsSelecting.Length; i++)
        {
            playerThatsSelecting[i].gameObject.SetActive(true);
            playerThatsSelecting[i].text = String.Format("Player  {0}  is selecting", GameManager.instance.order);
        }
    }

    public void ResetSelection()
    {
        for (int i = 0; i < buttonsNewSelection.Length; i++)
        {
            buttonsNewSelection[i].SetActive(false);
        }
        buttonsNewSelection[0].SetActive(true);
        GameManager.instance.eventSystem.SetSelectedGameObject(buttonsNewSelection[0].transform.GetChild(0).gameObject);
        placeInArray = 0;

        /* //Amount of Players
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
        */
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
