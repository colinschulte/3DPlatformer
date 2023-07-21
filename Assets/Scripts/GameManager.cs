using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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
