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
    public GameObject backButton;

    /// <summary>
    /// When the player presses the start button
    /// </summary>
    public void PlayGame()
    {
        canvas[0].gameObject.SetActive(false);
        canvas[1].gameObject.SetActive(true);
        canvas[2].gameObject.SetActive(false);
    }
    /// <summary>
    /// When the game starts the first button will be the first selected one
    /// </summary>
    public void Start()
    {
        GameManager.instance.eventSystem.SetSelectedGameObject(buttons[0].gameObject);
    }
    /// <summary>
    /// Method for when the player presses the controls button
    /// </summary>
    public void Controls() 
    {
        canvas[0].gameObject.SetActive(false);
        canvas[1].gameObject.SetActive(false);
        canvas[2].gameObject.SetActive(true);
        GameManager.instance.eventSystem.SetSelectedGameObject(backButton);
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
        canvas[2].gameObject.SetActive(false);
        options.gameObject.SetActive(false);
    }
}
