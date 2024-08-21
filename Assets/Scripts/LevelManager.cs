using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UIElements;
using TMPro;
using Unity.VisualScripting;

public class LevelManager : MonoBehaviour
{
    public GameManager gameManager;
    public Scene activeScene;

    public int currentCoins;
    public int currentBricks;
    public Text coinText;
    public Text cheeseText;
    public Text brickText;

    [SerializeField] private GameObject coinList;
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
    [SerializeField] private GameObject coins;
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject options, controls;
    [SerializeField] private GameObject pauseMenu, hud;
    [SerializeField] private UITimer UITimer;
    [SerializeField] private GameObject SpeakingMenu;
    [SerializeField] private GameObject SpeakerName;
    [SerializeField] private GameObject Dialogue;
    [SerializeField] private UnityEngine.UI.Slider volumeSlider;
    [SerializeField] private Volume musicVolume;
    [SerializeField] private CameraSensitivty cameraOptions;
    [SerializeField] public CinemachineFreeLook freeLook;
    [SerializeField] private UnityEngine.UI.Slider cameraSlider;
    [SerializeField] private UIMovement uiMovement;
    private bool xInvertOption;
    private bool yInvertOption;
    private bool FirstUpdate;

    // Start is called before the first frame update
    void Start()
    {
        FirstUpdate = true;
        options.SetActive(true);
        freeLook = FindObjectOfType<CinemachineFreeLook>();
        gameManager = FindObjectOfType<GameManager>();
        gameManager.freeLook = freeLook;
        xInvertOption = gameManager.xInvert;
        yInvertOption = gameManager.yInvert;
        startWaitCounter = waitTime;
        endWaitCounter = waitTime * 1.5f;
        coinCheese.SetActive(false);
        cheeseCam.SetActive(false);
        brickCheese.SetActive(false);
        brickCam.SetActive(false);
        //if (pauseMenu)
        //{
            pauseMenu.SetActive(false);
        //}
        //musicVolume = volumeSlider.GetComponent<Volume>();
        //cameraOptions = cameraSlider.GetComponent<CameraSensitivty>();
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
            if (FindObjectsOfType<GameManager>().Length > 1)
            {
                return;
            }
            gameManager = FindObjectOfType<GameManager>();
            gameManager.freeLook = freeLook;
            if (gameManager.cameraSliderValue != 0)
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
            brickCam.SetActive(true);
            MainCam.SetActive(false);

            if (startWaitCounter < 0)
            {
                brickCheese.SetActive(true);
            }
            else
            {
                startWaitCounter -= Time.deltaTime;
            }
            if (brickCheese.activeInHierarchy)
            {
                if (endWaitCounter < 0)
                {
                    brickCam.SetActive(false);
                    MainCam.SetActive(true);
                    startWaitCounter = waitTime;
                    endWaitCounter = waitTime * 1.5f;
                    allBricksCollected = true;
                }
                else
                {
                    endWaitCounter -= Time.deltaTime;
                }
            }
        }
    }

    public void AddCoins(int coinsToAdd)
    {
        uiMovement.CoinToggle(true);
        
        currentCoins += coinsToAdd;
        coinText.text = "Crackers: " + currentCoins + "/" + maxCoins;
    }

    public void CheeseGet(string id)
    {
        uiMovement.CheeseToggle(true);
        gameManager.AddCheese(id);
        cheeseText.text = "Cheeses: " + gameManager.NumCheesesCollected;
        if (gameManager.NumCheesesCollected >= 10)
        {
            Congratulations();
        }
    }
    
    public void BrickGet()
    {
        uiMovement.BrickToggle(true);
        currentBricks += 1;
        brickText.text = "Bricks: " + currentBricks + "/5";
    }

    public void Congratulations() 
    {
        UITimer.playing = false;
        SpeakerName.GetComponent<TextMeshProUGUI>().text = "Congratulations!";
        Dialogue.GetComponent<TextMeshProUGUI>().text = "You found all 10 cheeses! Thank you for playing!";
        SpeakingMenu.SetActive(true);
    }

    public Transform FindClosestCracker(Transform playerTransform)
    {
        Transform closestCracker = null;
        float currentDistance = 0;
        float closestDistance = 0;


        foreach (Transform coinTransform in coinList.GetComponentInChildren<Transform>())
        {
            currentDistance = Vector3.Distance(playerTransform.position, coinTransform.position);
            if(currentDistance < closestDistance || closestCracker == null)
            {
                closestDistance = currentDistance;
                closestCracker = coinTransform;
            }
        }
        return closestCracker;
    }
}
