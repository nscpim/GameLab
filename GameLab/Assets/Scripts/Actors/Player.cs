using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    private ControlScheme scheme;
    private Camera mainCamera;
    //Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        SetViewPortRect(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        scheme.Update();
    }

    public void SetViewPortRect(string _name) 
    {
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
                Debug.LogError("Player not correctly initalized!");
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
        Debug.Log("Test has been called");
    }

}
