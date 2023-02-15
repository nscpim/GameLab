using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Manager
{
    private ControlScheme[] schemes;


    public InputManager()
    {
        schemes = new ControlScheme[]
        {
            new ControlScheme()
            {
            _input = new InputBinding[]
               { 
                 new InputBinding() { axis = "Horizontal"},

            
               }
            
            }

        };
    
    
    }


    // Start is called before the first frame update
    public override void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {

    }
}
