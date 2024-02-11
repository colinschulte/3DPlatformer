using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{
    public AudioSource audioSource;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SetVolume(float sliderValue)
    {
        audioSource.volume = sliderValue;
        gameManager = FindObjectOfType<GameManager>();
        gameManager.musicVolume = sliderValue;

    }
}
