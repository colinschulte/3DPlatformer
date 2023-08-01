using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private GameManager gameManager;
    public int currentCoins;
    public Text coinText;

    public Text cheeseText;

    [SerializeField] private int maxCoins;
    [SerializeField] private GameObject coinCheese;
    public bool allCoinsCollected = false;

    // Start is called before the first frame update
    void Start()
    {
        coinCheese.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCoins >= maxCoins && !allCoinsCollected)
        {
            coinCheese.SetActive(true);
            allCoinsCollected = true;
        }
    }

    public void AddCoins(int coinsToAdd)
    {
        Debug.Log("running AddCoins");
        currentCoins += coinsToAdd;
        coinText.text = "Coins: " + currentCoins;
    }

    public void CheeseGet()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.AddCheese();
        cheeseText.text = "Cheese: " + gameManager.CheesesCollected;
    }
}
