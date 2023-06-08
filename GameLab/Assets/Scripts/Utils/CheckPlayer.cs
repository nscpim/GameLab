using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    public int playerint;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetPlayerInt(int amount)
    {
        playerint = amount;
    }


    public int GrabPlayerInt()
    {
        return playerint;
    }

}
