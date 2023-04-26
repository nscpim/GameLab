using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Canvas[] canvas;
    public Button[] buttons;
    public void PlayGame()
    {
        canvas[0].gameObject.SetActive(false);
        canvas[1].gameObject.SetActive(true);
    }

    public void Start()
    {
        GameManager.instance.eventSystem.SetSelectedGameObject(buttons[0].gameObject);
    }

    public void PracticePlay()
    {
        SceneManager.LoadScene("Practice");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
    }


    public void Return() 
    {
        canvas[0].gameObject.SetActive(true);
        canvas[1].gameObject.SetActive(false);
    }
}
