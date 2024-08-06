using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMovement : MonoBehaviour
{
    public RectTransform timer;
    public RectTransform coin;
    public RectTransform health;
    public RectTransform brick;
    public RectTransform cheese;
    public Vector3 coinDestination;
    public Vector3 cheeseDestination;
    public Vector3 healthDestination;
    public Vector3 brickDestination;

    // Start is called before the first frame update
    void Start()
    {

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
    }

    public void TimerToggle(bool isVisible)
    {
        if (isVisible)
        {
            timer.anchoredPosition = new Vector2(timer.anchoredPosition.x, -30);
        }
        else
        {
            timer.anchoredPosition = new Vector2(timer.anchoredPosition.x, 400);
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
        }
        else
        {
            CoinToggle(false);
            CheeseToggle(false);
            HealthToggle(false);
            BrickToggle(false);
        }
    }
    public void CoinToggle(bool isOn)
    {
        if (isOn)
        {
            coinDestination = new Vector3(150, coin.transform.position.y, 0);
        }
        else
        {
            coinDestination = new Vector3(-300, coin.transform.position.y, 0);
        }
    }
    public void CheeseToggle(bool isOn)
    {
        if (isOn)
        {
            cheeseDestination = new Vector3(885, cheese.transform.position.y, 0);
        }
        else
        {
            cheeseDestination = new Vector3(1300, cheese.transform.position.y, 0);
        }
    }
    public void HealthToggle(bool isOn)
    {
        if (isOn)
        {
            healthDestination = new Vector3(885, health.transform.position.y, 0);

        }
        else
        {
            healthDestination = new Vector3(1300, health.transform.position.y, 0);
        }
    }
    public void BrickToggle(bool isOn)
    {
        if (isOn)
        {
            brickDestination = new Vector3(150, brick.transform.position.y, 0);
        }
        else
        {
           brickDestination = new Vector3(-250, brick.transform.position.y, 0);
        }
    }
}
