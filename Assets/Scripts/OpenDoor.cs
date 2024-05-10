using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private float newX;
    [SerializeField] private float newY;
    [SerializeField] private float newZ;
    [SerializeField] private float speed;
    [SerializeField] private float waitTime = 2;
    [SerializeField] private float startWaitCounter;
    [SerializeField] private float endWaitCounter;
    [SerializeField] private GameObject cineCam;
    [SerializeField] private GameObject MainCam;
    private bool isOpening = false;


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
        if (isOpening)
        {
            cineCam.SetActive(true);
            MainCam.SetActive(false);
            //Quaternion newRotation = transform.rotation;
            //newRotation.y = 90f;
            //transform.rotation = newRotation;
            Quaternion newRotation = Quaternion.Euler(newX, newY, newZ);
            
            if (startWaitCounter < 0)
            {
                if (transform.rotation != newRotation)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, speed);
                }
            }
            else
            {
                startWaitCounter -= Time.deltaTime;
            }
            //transform.Rotate(newX, newY, newZ, Space.Self);
            if(transform.rotation == newRotation)
            {
                if (endWaitCounter < 0)
                {
                    cineCam.SetActive(false);
                    MainCam.SetActive(true);
                    isOpening = false;
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

    public void Open()
    {
       isOpening = true;
    }
}
