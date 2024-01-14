using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrowPlant : MonoBehaviour
{
    [SerializeField] private GameObject shortVine;
    [SerializeField] private GameObject longVine;
    [SerializeField] private float waitTime = 1;
    [SerializeField] private float startWaitCounter;
    [SerializeField] private float endWaitCounter;
    [SerializeField] private GameObject cineCam;
    [SerializeField] private GameObject MainCam;
    private bool isGrowing = false;


    // Start is called before the first frame update
    void Start()
    {
        cineCam.SetActive(false);
        MainCam = FindObjectOfType<CinemachineBrain>().GameObject();
        startWaitCounter = waitTime;
        endWaitCounter = waitTime * 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrowing)
        {
            cineCam.SetActive(true);
            MainCam.SetActive(false);
            
            if (startWaitCounter < 0)
            {
                shortVine.SetActive(false);
                longVine.SetActive(true);
            }
            else
            {
                startWaitCounter -= Time.deltaTime;
            }
            //transform.Rotate(newX, newY, newZ, Space.Self);
            if (longVine.activeInHierarchy)
            {
                if (endWaitCounter < 0)
                {
                    cineCam.SetActive(false);
                    MainCam.SetActive(true);
                    isGrowing = false;
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

    public void Grow()
    {
        isGrowing = true;
    }
}
