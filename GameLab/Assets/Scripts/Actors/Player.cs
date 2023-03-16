using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Actor
{
    private ControlScheme scheme;
    private Camera mainCamera;
    [HideInInspector] public int playerInt;
    private float gravity = 20f;
    [SerializeField] private ScriptableCharacter character;
    private Vector3 moveDirection = Vector3.zero;
    private float timeStamp;
    public bool canSelectCharacter;
    private Timer abilityCooldown;
    public bool canMove;

    //Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        mainCamera = GetComponentInChildren<Camera>();
        SetViewPortRect(gameObject.name, GameManager.instance.GetAmountOfPlayers());
        abilityCooldown = new Timer();

    }

    // Update is called once per frame
    void Update()
    {
        scheme.Update();
        moveDirection.y -= gravity * Time.deltaTime;
        Timer();
    }

    public void UpdateMesh()
    {
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        filter.mesh = character.characterMesh;
    }
    public void Timer() 
    {
        if (abilityCooldown.isActive && abilityCooldown.TimerDone())
        {
            abilityCooldown.StopTimer();
        }
    }


    public void SetViewPortRect(string _name, int amount)
    {
        //X , Y, Width and Height
        //Widh and height are always the same in 2 or 4 players
        //X position of screen on the X axis, starting from the bottom left corner
        //Y Position of screen on the Y axis, starting from the bottom
        switch (amount)
        {
            case 2:
                switch (_name)
                {
                    case "Player: 1":
                        mainCamera.rect = new Rect(0, 0.5f, 1f, 0.5f);
                        break;
                    case "Player: 2":
                        mainCamera.rect = new Rect(0f, 0f, 1f, 0.5f);
                        break;
                    default:
                        Debug.LogError("Players not correctly initalized!");
                        break;
                }
                break;
            case 3:
                switch (_name)
                {
                    case "Player: 1":
                        mainCamera.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                        break;
                    case "Player: 2":
                        mainCamera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                        break;
                    case "Player: 3":
                        mainCamera.rect = new Rect(0f, 0f, 1f, 0.5f);
                        break;
                    default:
                        Debug.LogError("Players not correctly initalized!");
                        break;
                }
                break;
            case 4:
                switch (_name)
                {
                    case "Player: 1":
                        mainCamera.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                        break;
                    case "Player: 2":
                        mainCamera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                        break;
                    case "Player: 3":
                        mainCamera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                        break;
                    case "Player: 4":
                        mainCamera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
                        break;
                    default:
                        Debug.LogError("Players not correctly initalized!");
                        break;
                }
                break;
            default:
                break;
        }





    }
    public void SetControlScheme(ControlScheme scheme)
    {
        if (scheme != null)
        {
            ClearControlScheme();
        }
        scheme.AssignInput(this);
        this.scheme = scheme;

    }
    public void ClearControlScheme()
    {
        scheme = null;
    }


    public void Test()
    {
      
    }
    public void ExecuteAbility()
    {
        if (!abilityCooldown.isActive)
        {
            switch (character.characterEnum)
            {
                case Character.Test:
                    Debug.Log("Executed Ability, WOW!");
                    break;
                case Character.Test1:
                    Debug.Log("Executed Ability, WOW!");
                    break;
                case Character.Test2:
                    Debug.Log("Executed Ability, WOW!");
                    break;
                case Character.Test3:
                    Debug.Log("Executed Ability, WOW!");
                    break;
                default:
                    break;
            }
            abilityCooldown.SetTimer(character.abilityCooldown);
        }

        else
        {
            Debug.Log(gameObject.name + " Your ability is on cooldown:" + abilityCooldown.TimeLeft());
        }
    }

    public void SelectedCharacter(Character selection)
    {
        character = GameManager.instance.characters[(int)selection];
        //UpdateMesh();
        //+1 because arrays start at 0, but we need the actual number of players here.
        if (GameManager.instance.order <= GameManager.instance.amountOfPlayers)
        {
            GameManager.instance.SelectedPlayer();
        }
    }

}
