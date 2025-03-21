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
    public GameObject pauseMenu, optionsMenu, controlsMenu;
    public GameObject mainCamera;
    public Player player;
    public AudioSource pauseOpen;
    public AudioSource pauseClose;
    public AudioSource buttonPress;
    [SerializeField] private UIMovement uiMovement;

    PlayerControls playerControls;

    private InputAction pause;
    private Scene activeScene;

    public GameObject firstButton, optionsFirstButton, optionsExitButton, controlsFirstButton, controlsExitButton;

    private void Awake()
    {
        playerControls = new PlayerControls();
        activeScene = SceneManager.GetActiveScene();
        player = FindObjectOfType<Player>();
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
        if(pause.triggered)
        {
            if (activeScene.name != "MainMenu")
            {
                if(!gamePaused)
                {
                    Time.timeScale = 0;
                    gamePaused = true;
                    player.canMove = false;
                    player.canTurn = false;
                    uiMovement.UIToggle(true);
                    Cursor.lockState = CursorLockMode.None;
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
                    player.canMove = true;
                    player.canTurn = true;
                    uiMovement.UIToggle(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    pauseClose.Play();
                    levelMusic.UnPause();
                    pauseMenu.SetActive(false);
                    optionsMenu.SetActive(false);
                    controlsMenu.SetActive(false);
                }
            }
        }
    }

    public void Resume()
    {
        buttonPress.Play();
        Time.timeScale = 1;
        gamePaused = false;
        uiMovement.UIToggle(false);
        Cursor.lockState = CursorLockMode.Locked;
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
    public void Controls()
    {
        buttonPress.Play();
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(controlsFirstButton);
    }
    public void CloseControls()
    {
        buttonPress.Play();
        controlsMenu.SetActive(false);
        optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(controlsExitButton);
    }
    public void Restart()
    {
        buttonPress.Play();
        Time.timeScale = 1;
        gamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        levelMusic.UnPause();
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(activeScene.buildIndex);
    }
    public void Quit()
    {
        buttonPress.Play();
        Time.timeScale = 1;
        gamePaused = false;
        Cursor.lockState = CursorLockMode.None;
        levelMusic.UnPause();
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(1);
    }
    
}
