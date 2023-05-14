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
                 new InputBinding() { axis = "Ability1"},
                 new InputBinding() { axis =  "Jump1"},
                 new InputBinding() { axis = "SecondAbility1"},
                 new InputBinding() { keyCode = KeyCode.Joystick1Button7, strokeType = KeyStrokeType.down},


               }
            },
            new ControlScheme()
            {
               _input = new InputBinding[]
               {
                 new InputBinding() { axis = "Ability2"},
                 new InputBinding() { axis =  "Jump2"},
                 new InputBinding() { axis = "SecondAbility2"},
                 new InputBinding() { keyCode = KeyCode.Joystick2Button7, strokeType = KeyStrokeType.down},

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

                 new InputBinding() { axis = "Ability1"},
                 new InputBinding() { axis =  "Jump1"},
                 new InputBinding() { axis = "SecondAbility1"},
                 new InputBinding() { keyCode = KeyCode.Joystick1Button7, strokeType = KeyStrokeType.down},
               }

            },
            new ControlScheme()
            {
               _input = new InputBinding[]
               {

                   new InputBinding() { axis = "Ability2"},
                   new InputBinding() { axis =  "Jump2"},
                   new InputBinding() { axis = "SecondAbility2"},
                   new InputBinding() { keyCode = KeyCode.Joystick2Button7, strokeType = KeyStrokeType.down},

               }

            },
            new ControlScheme()
            {
            _input = new InputBinding[]
               {

                   new InputBinding() { axis = "Ability3"},
                   new InputBinding() { axis =  "Jump3"},
                   new InputBinding() { axis = "SecondAbility3"},
                   new InputBinding() { keyCode = KeyCode.Joystick3Button7, strokeType = KeyStrokeType.down},


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

                   new InputBinding() { axis = "Ability1"},
                   new InputBinding() { axis =  "Jump1"},
                   new InputBinding() { axis = "SecondAbility1"},
                   new InputBinding() { keyCode = KeyCode.Joystick1Button7, strokeType = KeyStrokeType.down},
               }

            },
            new ControlScheme()
            {
               _input = new InputBinding[]
               {

                   new InputBinding() { axis = "Ability2"},
                   new InputBinding() { axis =  "Jump2"},
                   new InputBinding() { axis = "SecondAbility2"},
                   new InputBinding() { keyCode = KeyCode.Joystick2Button7, strokeType = KeyStrokeType.down},

               }

            },
            new ControlScheme()
            {
            _input = new InputBinding[]
               {

                   new InputBinding() { axis = "Ability3"},
                   new InputBinding() { axis =  "Jump3"},
                   new InputBinding() { axis = "SecondAbility3"},
                   new InputBinding() { keyCode = KeyCode.Joystick3Button7, strokeType = KeyStrokeType.down},

               }

            },
            new ControlScheme()
            {
            _input = new InputBinding[]
               {

                 new InputBinding() { axis = "Ability4"},
                 new InputBinding() { axis =  "Jump4"},
                 new InputBinding() { axis = "SecondAbility4"},
                 new InputBinding() { keyCode = KeyCode.Joystick4Button7, strokeType = KeyStrokeType.down},

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

            ThirdPersonMovement player = Object.Instantiate(GameManager.instance.player, new Vector3(GameManager.instance.spawnPoints[i].transform.position.x, GameManager.instance.spawnPoints[i].transform.position.y, GameManager.instance.spawnPoints[i].transform.position.z), GameManager.instance.player.transform.rotation);
            player.name = GameManager.instance.player.name = "Player: " + (i + 1);
            player.playerInt = i + 1;
            GameManager.instance.AddToGameManager(player);
            if (player.name != "Player: 1")
            {
                Object.Destroy(player.GetComponentInChildren<AudioListener>());
            }
            player.GetComponent<ThirdPersonMovement>().SetControlScheme(schemes[i]);
            Debug.Log("Test");
        }
    }
}
