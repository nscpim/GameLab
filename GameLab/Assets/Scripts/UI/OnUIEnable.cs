using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnUIEnable : MonoBehaviour
{
   
    public void OnEnable()
    {
        Debug.Log("UI Setter");
        GameManager.instance.eventSystem.SetSelectedGameObject(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
