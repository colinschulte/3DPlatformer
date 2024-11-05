using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class UIMovement : MonoBehaviour
{
    //public Player player;
    public Canvas UI;
    public RectTransform hud;
    public RectTransform timer;
    public RectTransform coin;
    public RectTransform health;
    public RectTransform brick;
    public RectTransform cheese;
    public Vector3 coinDestination;
    public Vector3 cheeseDestination;
    public Vector3 healthDestination;
    public Vector3 brickDestination;
    public Vector3 timerDestination;

    // Start is called before the first frame update
    void Start()
    {
        timerDestination = timer.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(coin.transform.position != coinDestination)
        {
            coin.transform.position = Vector3.MoveTowards(coin.transform.position, coinDestination, 60);
        }
        if (cheese.transform.position != cheeseDestination)
        {
            cheese.transform.position = Vector3.MoveTowards(cheese.transform.position, cheeseDestination, 60);
        }
        if (health.transform.position != healthDestination)
        {
            health.transform.position = Vector3.MoveTowards(health.transform.position, healthDestination, 60);
        }
        if (brick.transform.position != brickDestination)
        {
            brick.transform.position = Vector3.MoveTowards(brick.transform.position, brickDestination, 60);
        }
        if (timer.transform.position != timerDestination)
        {
            timer.transform.position = Vector3.MoveTowards(timer.transform.position, timerDestination, 60);
        }
    }

    public void TimerToggle(bool isVisible)
    {
        if (isVisible)
        {
            timerDestination = new Vector3(UnityEngine.Screen.width/2, 30, 0);
        }
        else
        {
            timerDestination = new Vector3(UnityEngine.Screen.width/2, -50, 0);
        }
    }

    public void UIToggle(bool isOn)
    {
        if (isOn)
        {
            CoinToggle(true);
            CheeseToggle(true);
            HealthToggle(true);
            BrickToggle(true);
            //TimerToggle(true);
        }
        else
        {
            CoinToggle(false);
            CheeseToggle(false);
            HealthToggle(false);
            BrickToggle(false);
            //TimerToggle(false);
        }
    }
    public void CoinToggle(bool isOn)
    {
        if (isOn)
        {
            coinDestination = new Vector3(coin.rect.width, UnityEngine.Screen.height - coin.rect.height, 0);
        }
        else
        {
            coinDestination = new Vector3(-coin.rect.width, UnityEngine.Screen.height - coin.rect.height, 0);
        }
    }
    public void CheeseToggle(bool isOn)
    {
        if (isOn)
        {
            cheeseDestination = new Vector3(UnityEngine.Screen.width - (cheese.rect.width), cheese.rect.height, 0);
        }
        else
        {
            cheeseDestination = new Vector3(UnityEngine.Screen.width + (cheese.rect.width), cheese.rect.height, 0);
        }
    }
    public void HealthToggle(bool isOn)
    {
        if (isOn)
        {
            healthDestination = new Vector3(UnityEngine.Screen.width - (health.rect.width), UnityEngine.Screen.height - health.rect.height, 0);

        }
        else
        {
            healthDestination = new Vector3(UnityEngine.Screen.width + (health.rect.width), UnityEngine.Screen.height - health.rect.height, 0);
        }
    }
    public void BrickToggle(bool isOn)
    {
        if (isOn)
        {
            brickDestination = new Vector3(brick.rect.width, brick.rect.height, 0);
        }
        else
        {
           brickDestination = new Vector3(-brick.rect.width, brick.rect.height, 0);
        }
    }
}
