using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fullscreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FullscreenToggle(bool isFullscreen)
    {
        //if (Screen.fullScreenMode != FullScreenMode.ExclusiveFullScreen)
        //{
        //    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        //}
        //else
        //{
        //    Screen.fullScreenMode = FullScreenMode.Windowed;
        //}
        Screen.fullScreen = isFullscreen;
    }
}
