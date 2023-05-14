using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class ButtonSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject toolTipPanel;
    public TextMeshProUGUI toolTipText;

    public void OnSelect(BaseEventData eventData)
    {
        toolTipPanel.SetActive(true);
        toolTipPanel.transform.position = new Vector3(eventData.selectedObject.transform.position.x, eventData.selectedObject.transform.position.y + 70f, eventData.selectedObject.transform.position.z);
        toolTipText.text = GameManager.instance.characters[eventData.selectedObject.GetComponent<Order>().order].description;  
    }

    public void OnDeselect(BaseEventData eventData)
    {
        toolTipPanel.SetActive(false);
    }
}
