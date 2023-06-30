using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentCoins;
    public Text coinText;

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoins(int coinsToAdd)
    {
        currentCoins += coinsToAdd;
        coinText.text = "Coins: " + currentCoins;
    }
}
