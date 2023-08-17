using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    private GameManager gameManager;
    public int currentCoins;
    public Text coinText;
    public Scene activeScene;

    public Text cheeseText;

    [SerializeField] private int maxCoins;
    private GameObject coins;
    private GameObject UI;
    private GameObject options;
    private GameObject pauseMenu;
    private Slider volumeSlider;
    private Volume musicVolume;
    private CameraSensitivty cameraOptions;
    public CinemachineFreeLook freeLook;
    private Slider cameraSlider;
    private bool xInvertOption;
    private bool yInvertOption;
    [SerializeField] private GameObject coinCheese;
    public bool allCoinsCollected = false;

    // Start is called before the first frame update
    void Start()
    {
        coins = GameObject.Find("Coins");
        UI = GameObject.Find("UI");
        options = GameObject.Find("OptionsMenu");
        pauseMenu = GameObject.Find("PauseMenu");
        gameManager = FindObjectOfType<GameManager>();
        volumeSlider = GameObject.Find("MusicVolume").GetComponent<Slider>();
        cameraSlider = GameObject.Find("CameraSensitivity").GetComponent<Slider>();
        xInvertOption = gameManager.xInvert;
        yInvertOption = gameManager.yInvert;
        freeLook = FindObjectOfType<CinemachineFreeLook>();
        coinCheese.SetActive(false);
        options.SetActive(true);
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
            UI.SetActive(false);
        }
        cheeseText.text = "Cheese: " + gameManager.CheesesCollected;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCoins >= maxCoins && !allCoinsCollected)
        {
            coinCheese.SetActive(true);
            allCoinsCollected = true;
        }

        if (freeLook != null && (freeLook.m_XAxis.m_MaxSpeed == 0f || freeLook.m_XAxis.m_MaxSpeed == 0f))
        {
            cameraOptions.SetSensitivity(cameraSlider.value);
        }
    }

    public void AddCoins(int coinsToAdd)
    {
        currentCoins += coinsToAdd;
        coinText.text = "Coins: " + currentCoins;
    }

    public void CheeseGet()
    {
        gameManager.AddCheese();
        cheeseText.text = "Cheese: " + gameManager.CheesesCollected;
    }
}
