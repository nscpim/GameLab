using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static Manager[] managers;

    public static bool inGame;
    public static bool paused;

    [Header("Players")]
    public Player player; 

    public static GameManager instance { get; private set; }

    public GameManager()
    {
        instance = this;

        managers = new Manager[]
        {
            new InputManager(),
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
        inGame = true;
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


}
public enum Levels 
{
    MainMenu,
    InGame,

}