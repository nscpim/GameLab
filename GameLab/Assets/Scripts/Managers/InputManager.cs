using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Manager
{
    private ControlScheme[] schemes;
    public bool setControlScheme;

    public InputManager()
    {

    }

    public void SetPlayerSchemes() 
    {
        switch (GameManager.instance.GetAmountOfPlayers())
        {
            case 2:
                schemes = new ControlScheme[]
       {
            new ControlScheme()
            {
            _input = new InputBinding[]
               {
                 new InputBinding() { axis = "Horizontal" },
                 new InputBinding() {  },
               }

            },
            new ControlScheme()
            {
               _input = new InputBinding[]
               {
                 new InputBinding() { axis = "Vertical" },

               }

            },
       };
                break;
            case 3:
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
        };
                break;
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
        setControlScheme = true;
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
