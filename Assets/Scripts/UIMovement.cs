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

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TimerToggle(bool isVisible)
    {
        if(isVisible)
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
            coin.anchoredPosition = new Vector2(-115, coin.anchoredPosition.y);
            health.anchoredPosition = new Vector2(115, health.anchoredPosition.y);
            cheese.anchoredPosition = new Vector2(-115, cheese.anchoredPosition.y);
            brick.anchoredPosition = new Vector2(115, brick.anchoredPosition.y);
        }
        else
        {
            coin.anchoredPosition = new Vector2(265, coin.anchoredPosition.y);
            health.anchoredPosition = new Vector2(-115, health.anchoredPosition.y);
            cheese.anchoredPosition = new Vector2(215, cheese.anchoredPosition.y);
            brick.anchoredPosition = new Vector2(-115, brick.anchoredPosition.y);
        }
    }
}
