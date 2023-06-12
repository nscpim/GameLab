using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Manager Variables")]
    //managers array, stores all the managers.
    public static Manager[] managers;
    //Boolean to check if we are ingame or not.
    public static bool inGame;
    //Boolean if the game is paused or not.
    public static bool paused;

    [Header("Players")]
    //Initial player object
    public ThirdPersonMovement player;
    //Referenced Players *pls do not touch*
    public List<ThirdPersonMovement> currentPlayers = new List<ThirdPersonMovement>();
    //All the available character Objects.
    public ScriptableCharacter[] characters;
    //Spawnpoint for the players
    public List<Transform> spawnPoints;



    [Header("UI")]
    //integer with the amount of players.
    [HideInInspector] public int amountOfPlayers;
    //Boolean for if the players can select a character.
    public bool canSelect = false;
    //Order of players starting at 1, using -1 for arrays since they start at 0.
    public int order = 1;
    //reference to the eventSystem.
    public EventSystem eventSystem;
    //Input Module for the event system.
    public StandaloneInputModule inputModule;


    [Header("Level")]
    public int matchCountdown;
    public float currentMatchCountdown;
    
  

    //Instance of this class
    public static GameManager instance { get; private set; }

    /// <summary>
    /// Constructor of this class for initalizing the manager array.
    /// </summary>
    public GameManager()
    {

        instance = this;

        managers = new Manager[]
        {
            new InputManager(),
            new AudioManager(),
            new UIManager(),
        };
    }

    /// <summary>
    /// Method for getting a manager
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetManager<T>() where T : Manager
    {
        for (int i = 0; i < managers.Length; i++)
        {
            if (typeof(T) == managers[i].GetType())
            {
                return (T)managers[i];
            }
        }
        return default(T);
    }


    /// <summary>
    /// Awake for all managers.
    /// </summary>
    public void Awake()
    {
        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Awake();
        }
    }


    /// <summary>
    /// Start of all the managers.
    /// </summary>
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
      

        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Start();
        }
        GetManager<AudioManager>().PlayMusic("menu");
    }

    /// <summary>
    /// Update of all the managers.
    /// </summary>
    public void Update()
    {
        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Update();
        }
        GetInput();
    }


    /// <summary>
    /// Method for loading a level.
    /// </summary>
    /// <param name="level"></param>
    public static void LoadLevel(Levels level)
    {
        SceneManager.LoadScene((int)level);
        if (level == Levels.InGame)
        {
            inGame = true;
            paused = false;
            
        }
        else
        {
            inGame = false;
        }

    }

    /// <summary>
    /// Method for pausing the game.
    /// </summary>
    /// <param name="value"></param>
    public static void Pause()
    {
        paused = !paused;

        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Pause(paused);
        }
    }

    /// <summary>
    /// Getter for the ingame boolean
    /// </summary>
    /// <returns></returns>
    public static bool GetInGame()
    {
        return inGame;
    }

    /// <summary>
    /// Method for setting the amount of players. (This should have 1 reference)
    /// </summary>
    /// <param name="value"></param>
    public void SetAmountOfPlayers(int value)
    {
        amountOfPlayers = value;
    }

    /// <summary>
    /// Getter method for getting the amount of players.
    /// </summary>
    /// <returns></returns>
    public int GetAmountOfPlayers()
    {
        return amountOfPlayers;
    }

    /// <summary>
    /// Add player to the currentplayers list
    /// </summary>
    /// <param name="player"></param>
    public void AddToGameManager(ThirdPersonMovement player)
    {
        currentPlayers.Add(player);
    }

    /// <summary>
    /// Selecting player in pick order for choosing a character
    /// </summary>
    public void SelectedPlayer()
    {
        //-1 because arrays start at 0 and the order is a representation of the actual player number.
        currentPlayers[order - 1].canSelectCharacter = false;
        order++;
        if (order <= amountOfPlayers)
        {
            SetInputModule(order);
            currentPlayers[order - 1].canSelectCharacter = true;
            if (order == 2)
            {
                StartCoroutine(WaitForName("playertwochoose"));
                Invoke("ChooseYourCharacter", 3f);
            }
            if (order == 3)
            {
                StartCoroutine(WaitForName("playerthreechoose"));
                Invoke("ChooseYourCharacter", 3f);
            }
            if (order == 4)
            {
                StartCoroutine(WaitForName("playerfourchoose"));
                Invoke("ChooseYourCharacter", 3f);
            }

        }
    }


    public IEnumerator WaitForName(string clip) 
    {
        yield return new WaitForSeconds(2f);
        GameManager.GetManager<AudioManager>().PlaySound(clip, false, Vector3.zero, false, null);
    }

  
    public void ChooseYourCharacter()
    {
        GameManager.GetManager<AudioManager>().PlaySound("choosecharacter", false, Vector3.zero, false, null);
    }

    /// <summary>
    /// Setting the input module to allow different player's input
    /// </summary>
    /// <param name="order"></param>
    public void SetInputModule(int order)
    {
        inputModule.horizontalAxis = "Horizontal" + order;
        inputModule.verticalAxis = "Vertical" + order;
        inputModule.submitButton = "Submit" + order;
    }

    public void SetCanMove(bool value) 
    {
        for (int i = 0; i < currentPlayers.Count; i++)
        {
            currentPlayers[i].canMove = value;
            currentPlayers[i].GetComponent<ThirdPersonDash>().canDash = value;
        }
    }


    public void GetInput()
    {
        System.Array values = System.Enum.GetValues(typeof(KeyCode));
        foreach (KeyCode code in values)
        {
           // if (Input.GetKeyDown(code)) { print(System.Enum.GetName(typeof(KeyCode), code)); }
        }
    }
}

/// <summary>
/// Enum for the scenes
/// </summary>
public enum Levels
{
    MainMenu,
    InGame,

}
/// <summary>
/// Enum for all the characters
/// </summary>
public enum Character
{
    Test,
    Test1,
    Test2,
    Test3,
}
