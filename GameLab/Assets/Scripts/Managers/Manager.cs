using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager
{

    public virtual void Awake() 
    {
    
    }

    // Start is called before the first frame update
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {

    }

    public virtual void Pause(bool pause) 
    {
        if (pause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
