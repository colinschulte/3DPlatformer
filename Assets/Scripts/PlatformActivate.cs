using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformActivate : MonoBehaviour
{
    [SerializeField] private PlatformMovement platform;
    [SerializeField] private float waitTime = 1;
    [SerializeField] private float startWaitCounter;
    [SerializeField] private float endWaitCounter;
    [SerializeField] private GameObject cineCam;
    [SerializeField] private GameObject MainCam;
    private bool isActivated = false;


    // Start is called before the first frame update
    void Start()
    {
        cineCam.SetActive(false);
        MainCam = FindObjectOfType<CinemachineBrain>().GameObject();
        startWaitCounter = waitTime;
        endWaitCounter = waitTime * 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            cineCam.SetActive(true);
            MainCam.SetActive(false);

            if (startWaitCounter < 0)
            {
                platform.enabled = true;
            }
            else
            {
                startWaitCounter -= Time.deltaTime;
            }
            if (platform.enabled)
            {
                if (endWaitCounter < 0)
                {
                    cineCam.SetActive(false);
                    MainCam.SetActive(true);
                    isActivated = false;
                    startWaitCounter = waitTime;
                    endWaitCounter = waitTime * 1.5f;
                }
                else
                {
                    endWaitCounter -= Time.deltaTime;
                }
            }
        }
    }

    public void Activate()
    {
        isActivated = true;
    }
}
