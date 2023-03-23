using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Manager
{
    // Start is called before the first frame update
    public override void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {

    }

    public void UpdateUI(TextMeshProUGUI text, string value) 
    {
        text.text = value;
    }

  
}
