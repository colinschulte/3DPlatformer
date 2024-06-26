using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheeseLaunch : MonoBehaviour
{
    public GameObject glove;
    public GameObject iceCheese;
    public GameObject burner;
    [SerializeField] private float waitTime = 1;
    [SerializeField] private float flySpeed = 10;
    [SerializeField] private float startWaitCounter;
    [SerializeField] private float endWaitCounter;
    [SerializeField] private GameObject cineCam;
    [SerializeField] private GameObject MainCam;
    [SerializeField] private bool isPunching = false;


    public void Start()
    {
        cineCam.SetActive(false);
        MainCam = FindObjectOfType<CinemachineBrain>().GameObject();
        startWaitCounter = waitTime;
        endWaitCounter = waitTime * 1.5f;
    }
    public void Update()
    {
        if (isPunching)
        {
            cineCam.SetActive(true);
            MainCam.SetActive(false);
            Vector3 glovePosition = glove.transform.position;
            glovePosition.x += 1.5f;
            glove.transform.position = glovePosition;
            Vector3 cheesePosition = burner.transform.position;
            cheesePosition.y += 2f;

            if (startWaitCounter < 0)
            {
                //shortVine.transform.localScale = Vector3.MoveTowards(shortVine.transform.localScale, longVine.transform.localScale, growSpeed);
                //shortVine.transform.localPosition = new Vector3(shortVine.transform.localPosition.x, -(shortVine.transform.localScale.y / 2) + 1, shortVine.transform.localPosition.z);
                iceCheese.transform.position = Vector3.MoveTowards(iceCheese.transform.position, cheesePosition, flySpeed);
            }
            else
            {
                startWaitCounter -= Time.deltaTime;
            }
            if (iceCheese.transform.position == cheesePosition)
            {
                iceCheese.GetComponent<MeshRenderer>().enabled = false;
                iceCheese.GetComponent<BoxCollider>().enabled = false;
                GameObject cheese = iceCheese.transform.GetChild(0).gameObject;
                cheese.GetComponent<Animator>().enabled = true;
                cheese.GetComponent<SphereCollider>().enabled = true;
                if (endWaitCounter < 0)
                {
                    cineCam.SetActive(false);
                    MainCam.SetActive(true);
                    isPunching = false;
                    startWaitCounter = waitTime;
                    endWaitCounter = waitTime * 1.5f;
                }
                else
                {
                    endWaitCounter -= Time.deltaTime;
                }
            }
            //Vector3 cheesePosition = burner.transform.position;
            //cheesePosition.y += 2f;
            //iceCheese.GetComponent<MeshRenderer>().enabled = false;
            //iceCheese.GetComponent<BoxCollider>().enabled = false;
            //GameObject cheese = iceCheese.transform.GetChild(0).gameObject;
            //cheese.GetComponent<Animator>().enabled = true;
            //cheese.GetComponent<SphereCollider>().enabled = true;
        }
    }
    public void Punch()
    {
        isPunching = true;
    }
}
