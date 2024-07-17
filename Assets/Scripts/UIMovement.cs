using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMovement : MonoBehaviour
{
    public RectTransform timer;

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
}
