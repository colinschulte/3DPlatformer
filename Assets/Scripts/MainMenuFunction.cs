using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuFunction : MonoBehaviour
{
    public AudioSource buttonPress;

    public GameObject mainMenu, optionsMenu;

    public GameObject firstButton, optionsFirstButton, optionsExitButton;

    public LevelManager levelManager;

    public void Start()
    {
        //optionsMenu.SetActive(false);
    }

    public void PlayGame()
    {
        buttonPress.Play();
        levelManager.gameManager.lastScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("Level001");
    }

    public void Options()
    {
        buttonPress.Play();
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    public void QuitGame()
    {
        buttonPress.Play();
        Application.Quit();
    }

    public void CloseOptions()
    {
        buttonPress.Play();
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsExitButton);
    }

    public void Fullscreen(bool isFullscreen)
    {
        //if (Screen.fullScreenMode != FullScreenMode.ExclusiveFullScreen)
        //{
        //    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        //}
        //else
        //{
        //    Screen.fullScreenMode = FullScreenMode.Windowed;
        //}
        Screen.fullScreen = isFullscreen;
    }
}
