using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIHandler : MonoBehaviour
{
    public Canvas selectionCanvas;
    public GameObject[] panels;


    // Start is called before the first frame update
    void Start()
    {
        panels = new GameObject[GameManager.instance.amountOfPlayers];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseCharacter() 
    {
    
    
    
    }

}
