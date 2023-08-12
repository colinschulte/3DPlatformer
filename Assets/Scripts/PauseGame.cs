using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public bool gamePaused = false;
    public AudioSource levelMusic;
    public GameObject pauseMenu, optionsMenu;
    public GameObject mainCamera;
    public AudioSource pauseOpen;
    public AudioSource pauseClose;
    public AudioSource buttonPress;

    PlayerControls playerControls;

    private InputAction pause;
    private Scene activeScene;

    public GameObject firstButton, optionsFirstButton, optionsExitButton;

    private void Awake()
    {
        playerControls = new PlayerControls();
        activeScene = SceneManager.GetActiveScene();
    }

    private void OnEnable()
    {
        pause = playerControls.Player.Pause;
        pause.Enable();
    }

    private void OnDisable()
    {
        pause.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if(activeScene.name != "MainMenu")
        {
            if (pause.triggered)
            {
                if(gamePaused == false)
                {
                    Time.timeScale = 0;
                    gamePaused = true;
                    Cursor.visible = true;
                    levelMusic.Pause();
                    pauseOpen.Play();
                    pauseMenu.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(firstButton);
                }
                else
                {
                    Time.timeScale = 1;
                    gamePaused = false;
                    //Cursor.visible = false;
                    pauseClose.Play();
                    levelMusic.UnPause();
                    pauseMenu.SetActive(false);
                    optionsMenu.SetActive(false);
                }
            }
        }
    }

    public void Resume()
    {
        buttonPress.Play();
        Time.timeScale = 1;
        gamePaused = false;
        Cursor.visible = false;
        levelMusic.UnPause();
        pauseMenu.SetActive(false);
    }
    public void Options()
    {
        buttonPress.Play();
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }
    public void CloseOptions()
    {
        buttonPress.Play();
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsExitButton);
    }
    public void Restart()
    {
        buttonPress.Play();
        Time.timeScale = 1;
        gamePaused = false;
        Cursor.visible = false;
        levelMusic.UnPause();
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(2);
    }
    public void Quit()
    {
        buttonPress.Play();
        Time.timeScale = 1;
        gamePaused = false;
        Cursor.visible = false;
        levelMusic.UnPause();
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(1);
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
