using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [Header("Manager Variables")]
    public static Manager[] managers;
    public static bool inGame;
    public static bool paused;

    [Header("Players")]
    //Initial player object
    public Player player;
    //Referenced Players
    public List<Player> currentPlayers = new List<Player>();
    public ScriptableCharacter[] characters;

    [Header("UI")]
    [HideInInspector] public int amountOfPlayers;
    public bool canSelect = false;
    public int order = 1;
    public EventSystem eventSystem;

    public static GameManager instance { get; private set; }

    public GameManager()
    {
        instance = this;

        managers = new Manager[]
        {
            new InputManager(),
            new AudioManager(),
        };
    }

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


    public void Awake()
    {
        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Awake();
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Start();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Update();
        }
    }

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

    public static void Pause(bool value)
    {
        paused = value;

        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Pause(value);
        }
    }

    public static bool GetInGame()
    {
        return inGame;
    }

    public void SetAmountOfPlayers(int value)
    {
        amountOfPlayers = value;
    }

    public int GetAmountOfPlayers()
    {
        return amountOfPlayers;
    }

    public void AddToGameManager(Player player)
    {
        currentPlayers.Add(player);
    }

    public void SelectedPlayer()
    {
        //-1 because arrays start at 0 and the order is a representation of the actual player number.
        currentPlayers[order - 1].canSelectCharacter = false;
        order++;
        if (order <= amountOfPlayers)
        {
            currentPlayers[order - 1].canSelectCharacter = true;
        }
    }
}
public enum Levels
{
    MainMenu,
    InGame,

}
public enum Character
{
    Test,
    Test1,
    Test2,
    Test3,
}
