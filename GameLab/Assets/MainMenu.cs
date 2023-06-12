using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Canvas[] canvas;
    public Button[] buttons;
    public GameObject options;
    public Slider volumeSlider;

    /// <summary>
    /// When the player presses the start button
    /// </summary>
    public void PlayGame()
    {
        canvas[0].gameObject.SetActive(false);
        canvas[1].gameObject.SetActive(true);
    }
    /// <summary>
    /// When the game starts the first button will be the first selected one
    /// </summary>
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

    /// <summary>
    /// Called when volume gets changed
    /// </summary>
    public void ChangeVolume() 
    {
        GameManager.GetManager<AudioManager>().SetVolume(volumeSlider.value, AudioType.Master);
    }

    /// <summary>
    /// Whent he return button is pressed
    /// </summary>
    public void Return() 
    {
        canvas[0].gameObject.SetActive(true);
        canvas[1].gameObject.SetActive(false);
        options.gameObject.SetActive(false);
    }
}
