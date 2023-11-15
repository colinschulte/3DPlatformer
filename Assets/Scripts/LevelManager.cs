using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    public GameManager gameManager;
    public Scene activeScene;

    public int currentCoins;
    public int currentBricks;
    public Text coinText;
    public Text cheeseText;
    public Text brickText;
    
    [SerializeField] private GameObject coinCheese;
    [SerializeField] private GameObject brickCheese;
    public bool allCoinsCollected = false;
    public bool allBricksCollected = false;

    [SerializeField] private int maxCoins;
    private GameObject coins;
    private GameObject UI;
    [SerializeField] private GameObject options, controls;
    private GameObject pauseMenu, hud;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Volume musicVolume;
    private CameraSensitivty cameraOptions;
    public CinemachineFreeLook freeLook;
    private Slider cameraSlider;
    private bool xInvertOption;
    private bool yInvertOption;
    private bool FirstUpdate;

    // Start is called before the first frame update
    void Start()
    {
        FirstUpdate = true;
        coins = GameObject.Find("Coins");
        UI = GameObject.Find("UI");
        options = GameObject.Find("OptionsMenu");
        options.SetActive(true);
        controls = GameObject.Find("ControlsMenu");
        pauseMenu = GameObject.Find("PauseMenu");
        hud = GameObject.Find("HUD");
        freeLook = FindObjectOfType<CinemachineFreeLook>();
        gameManager = FindObjectOfType<GameManager>();
        gameManager.freeLook = freeLook;
        volumeSlider = GameObject.Find("MusicVolume").GetComponent<Slider>();
        cameraSlider = GameObject.Find("CameraSensitivity").GetComponent<Slider>();
        xInvertOption = gameManager.xInvert;
        yInvertOption = gameManager.yInvert;
        coinCheese.SetActive(false);
        if (pauseMenu)
        {
            pauseMenu.SetActive(false);
        }
        musicVolume = volumeSlider.GetComponent<Volume>();
        cameraOptions = cameraSlider.GetComponent<CameraSensitivty>();
        if (coins)
        {
            maxCoins = coins.transform.childCount;
        }
        activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == "MainMenu" && UI)
        {
            hud.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            hud.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
        }
        cheeseText.text = "Cheese: " + gameManager.NumCheesesCollected;
        if (gameManager.musicVolume != 0)
        {
            volumeSlider.value = gameManager.musicVolume;
        }
        musicVolume.SetVolume(volumeSlider.value);
        cameraOptions.freeLook = freeLook;
        cameraOptions.gameManager = gameManager;
        cameraOptions.SetSensitivity(cameraSlider.value);
        freeLook.m_XAxis.m_InvertInput = xInvertOption;
        freeLook.m_YAxis.m_InvertInput = yInvertOption;
        options.SetActive(false);
        controls.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (FirstUpdate)
        {
            if(FindObjectsOfType<GameManager>().Length > 1)
            {
                return;
            }
            gameManager = FindObjectOfType<GameManager>();
            gameManager.freeLook = freeLook;
            if(gameManager.cameraSliderValue != 0)
            {
                cameraSlider.value = gameManager.cameraSliderValue;
                cameraOptions.SetSensitivity(cameraSlider.value);
            }
            if (gameManager.NumCheesesCollected == 1)
            {
                cheeseText.text = "Cheese: " + gameManager.NumCheesesCollected;
            }
            else
            {
                cheeseText.text = "Cheeses: " + gameManager.NumCheesesCollected;
            }
            coinText.text = "Crackers: " + currentCoins + "/" + maxCoins;
            if (gameManager.musicVolume != 0)
            {
                volumeSlider.value = gameManager.musicVolume;
            }
            


            FirstUpdate = false;
        }
        if (currentCoins >= maxCoins && !allCoinsCollected)
        {
            coinCheese.SetActive(true);
            coinText.text = "Crackers: " + currentCoins + "/" + maxCoins;
            allCoinsCollected = true;
        }

        if (currentBricks >= 5 && !allBricksCollected)
        {
            brickCheese.SetActive(true);
            brickText.text = "Bricks: " + currentBricks + "/5";
            allBricksCollected = true;
        }

        //if (freeLook != null && (freeLook.m_XAxis.m_MaxSpeed == 0f || freeLook.m_XAxis.m_MaxSpeed == 0f))
        //{
        //    gameManager = FindObjectOfType<GameManager>();
        //    gameManager.freeLook = freeLook;
        //    cameraOptions.SetSensitivity(cameraSlider.value);
        //}
        //if (gameManager.NumCheesesCollected == 0)
        //{
        //    gameManager = FindObjectOfType<GameManager>();
        //    cheeseText.text = "Cheese: " + gameManager.NumCheesesCollected;
        //}
        //if (gameManager.musicVolume != 0)
        //{
        //    //volumeSlider = GameObject.Find("MusicVolume").GetComponent<Slider>();
        //    volumeSlider.value = gameManager.musicVolume;
        //}
    }

    public void AddCoins(int coinsToAdd)
    {
        currentCoins += coinsToAdd;
        coinText.text = "Crackers: " + currentCoins + "/" + maxCoins;
    }

    public void CheeseGet(string id)
    {
        gameManager.AddCheese(id);
        cheeseText.text = "Cheeses: " + gameManager.NumCheesesCollected;
    }
    
    public void BrickGet()
    {
        currentBricks += 1;
        brickText.text = "Bricks: " + currentBricks + "/5";
    }
}
