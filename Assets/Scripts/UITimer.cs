using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    public Text TimerText;
    private GameManager gameManager;
    private bool firstUpdate;
    public bool playing;
    private float Timer;
    void Start()
    {
        firstUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (firstUpdate)
        {
            if (FindObjectsOfType<GameManager>().Length > 1)
            {
                return;
            }
            gameManager = FindObjectOfType<GameManager>();
            Timer = gameManager.timer;

            firstUpdate = false;
        }
        else if (playing == true)
        {

            Timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(Timer / 60f);
            int seconds = Mathf.FloorToInt(Timer % 60f);
            int milliseconds = Mathf.FloorToInt((Timer * 100f) % 100f);
            TimerText.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
            gameManager.timer = Timer;
        }
    }
}

