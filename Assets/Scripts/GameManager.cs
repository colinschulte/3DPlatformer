using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentCoins;
    public Text coinText;

    public int CheesesCollected;
    public Text cheeseText;

    [SerializeField] private GameObject coinCheese;
    public bool allCoinsCollected = false;

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        coinCheese.SetActive(false);
    }

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentCoins >= 13 && !allCoinsCollected)
        {
            coinCheese.SetActive(true);
            allCoinsCollected = true;
        }
    }

    public void AddCoins(int coinsToAdd)
    {
        currentCoins += coinsToAdd;
        coinText.text = "Coins: " + currentCoins;
    }

    public void AddCheese()
    {
        CheesesCollected += 1;
        cheeseText.text = "Cheese: " + CheesesCollected;
    }
        
}
