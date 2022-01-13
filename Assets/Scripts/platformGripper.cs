                                                                       using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformGripper : MonoBehaviour
{
    public GameObject platform;
    public GameObject player;

    void OnTriggerEnter()
    {
        player.transform.parent = platform.transform;
    }
    void OnTriggerExit()
    {
        player.transform.parent = null;
    }
}
