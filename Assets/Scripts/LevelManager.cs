using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public GameManager gameManager;
    public Scene activeScene;

    public int currentCoins;
    public int currentBricks;
    public Text coinText;
    public Text cheeseText;
    public Text brickText;

    [SerializeField] private float waitTime = 1;
    [SerializeField] private float startWaitCounter;
    [SerializeField] private float endWaitCounter;
    [SerializeField] private GameObject coinCheese;
    [SerializeField] private GameObject cheeseCam;
    [SerializeField] private GameObject brickCheese;
    [SerializeField] private GameObject brickCam;
    [SerializeField] private GameObject MainCam;
    public bool allCoinsCollected = false;
    public bool allBricksCollected = false;

    [SerializeField] private int maxCoins;
    private GameObject coins;
    private GameObject UI;
    [SerializeField] private GameObject options, controls;
    private GameObject pauseMenu, hud;
    [SerializeField] private UnityEngine.UI.Slider volumeSlider;
    [SerializeField] private Volume musicVolume;
    private CameraSensitivty cameraOptions;
    public CinemachineFreeLook freeLook;
    private UnityEngine.UI.Slider cameraSlider;
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
        volumeSlider = GameObject.Find("MusicVolume").GetComponent<UnityEngine.UI.Slider>();
        cameraSlider = GameObject.Find("CameraSensitivity").GetComponent<UnityEngine.UI.Slider>();
        xInvertOption = gameManager.xInvert;
        yInvertOption = gameManager.yInvert;
        coinCheese.SetActive(false);
        cheeseCam.SetActive(false);
        brickCheese.SetActive(false);
        brickCam.SetActive(false);
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
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            hud.SetActive(true);
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
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
            cheeseCam.SetActive(true);
            MainCam.SetActive(false);

            if (startWaitCounter < 0)
            {
                coinCheese.SetActive(true);
            }
            else
            {
                startWaitCounter -= Time.deltaTime;
            }
            //transform.Rotate(newX, newY, newZ, Space.Self);
            if (coinCheese.activeInHierarchy)
            {
                if (endWaitCounter < 0)
                {
                    cheeseCam.SetActive(false);
                    MainCam.SetActive(true);
                    startWaitCounter = waitTime;
                    endWaitCounter = waitTime * 1.5f;
                    allCoinsCollected = true;
                }
                else
                {
                    endWaitCounter -= Time.deltaTime;
                }
            }
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
