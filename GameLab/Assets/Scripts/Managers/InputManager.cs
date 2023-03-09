using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Manager
{
    private ControlScheme[] schemes;

    public InputManager()
    {

    }

    public void SetPlayerSchemes() 
    {
        switch (GameManager.instance.GetAmountOfPlayers())
        {
            //2 Players Array
            case 2:
                schemes = new ControlScheme[]
       {
            new ControlScheme()
            {
            _input = new InputBinding[]
               {
                 new InputBinding() { axis = "Horizontal1" },
                 new InputBinding() { keyCode = KeyCode.Q, strokeType = KeyStrokeType.down}
               }
            },
            new ControlScheme()
            {
               _input = new InputBinding[]
               {
                 new InputBinding() { axis = "Horizontal2" },
                 new InputBinding() { keyCode = KeyCode.Q, strokeType = KeyStrokeType.down}

               }
            },
       };
                break;
                //3 Players array
            case 3:
                schemes = new ControlScheme[]
{
            new ControlScheme()
            {
            _input = new InputBinding[]
               {
                 new InputBinding() { axis = "Horizontal" },
                 new InputBinding() { keyCode = KeyCode.Q, strokeType = KeyStrokeType.down}
               }

            },
            new ControlScheme()
            {
               _input = new InputBinding[]
               {
                 new InputBinding() { axis = "Vertical" },
                 new InputBinding() { keyCode = KeyCode.Q, strokeType = KeyStrokeType.down}

               }

            },
            new ControlScheme()
            {
            _input = new InputBinding[]
               {
                 new InputBinding() { keyCode = KeyCode.Space, strokeType = KeyStrokeType.down },
               }

            },
        };
                break;
                //4 Players Array
            case 4:
                schemes = new ControlScheme[]
       {
            new ControlScheme()
            {
            _input = new InputBinding[]
               {
                 new InputBinding() { axis = "Horizontal" },
               }

            },
            new ControlScheme()
            {
               _input = new InputBinding[]
               {
                 new InputBinding() { axis = "Vertical" },

               }

            },
            new ControlScheme()
            {
            _input = new InputBinding[]
               {
                 new InputBinding() { keyCode = KeyCode.Space, strokeType = KeyStrokeType.down },
               }

            },
            new ControlScheme()
            {
            _input = new InputBinding[]
               {
                 new InputBinding() { keyCode = KeyCode.W, strokeType = KeyStrokeType.down },
               }

            },

       };
                break;
            default:
                break;
        }
    }


    // Start is called before the first frame update
    public override void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    { 
        if (!GameManager.GetInGame())
        {
            return;
        }
        base.Update();

        for (int i = 0; i < schemes.Length; i++)
        {
            if (schemes[i].isActive)
            {
                continue;
            }
            schemes[i].isActive = true;
            Player player = Object.Instantiate(GameManager.instance.player, new Vector3(GameManager.instance.player.transform.position.x + (i * 10), GameManager.instance.player.transform.position.y, GameManager.instance.player.transform.position.z), GameManager.instance.transform.rotation);
           
            player.name = GameManager.instance.player.name = "Player: " + (i + 1);
            player.playerInt = i + 1;
            GameManager.instance.AddToGameManager(player);
            if (player.name != "Player: 1")
            {
                Object.Destroy(player.GetComponentInChildren<AudioListener>());
            }
            player.GetComponent<Player>().SetControlScheme(schemes[i]);
           
            if (GameManager.GetInGame())
            {
                GameManager.Pause(false);
            }
        }
    }
}
