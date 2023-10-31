using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlock : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private int cheesesNeeded;
    [SerializeField] private GameObject levelText;

    void Update()
    {
        if (levelManager != null)
        {
            if (cheesesNeeded <= levelManager.gameManager.NumCheesesCollected)
            {
                Destroy(gameObject);
                levelText.SetActive(true);
            }
        }
    }
}
