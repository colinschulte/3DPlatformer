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
    [SerializeField] private float growSpeed;
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
                shortVine.transform.localScale = Vector3.MoveTowards(shortVine.transform.localScale, longVine.transform.localScale, growSpeed);
                shortVine.transform.localPosition = new Vector3(shortVine.transform.localPosition.x, -(shortVine.transform.localScale.y / 2) + 1, shortVine.transform.localPosition.z);
            }
            else
            {
                startWaitCounter -= Time.deltaTime;
            }
            if(shortVine.transform.localScale == longVine.transform.localScale)
            {
                shortVine.SetActive(false);
                longVine.SetActive(true);
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
