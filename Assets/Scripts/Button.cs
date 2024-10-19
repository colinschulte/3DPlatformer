using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool buttonPressed = false;
    public List<OpenDoor> openDoors;
    public List<GrowPlant> growPlants;
    [SerializeField] private AudioSource pressSound;
    public CheeseLaunch launch;
    public PlatformActivate platform;

    public void Press()
    {
        if (!buttonPressed) { 
            pressSound.Play();
            Vector3 newPosition = transform.position;
            newPosition.y -= 0.24f;
            transform.position = newPosition;

            foreach(OpenDoor openDoor in openDoors)
            {
                openDoor.Open();
            }

            foreach(GrowPlant growPlant in growPlants)
            {
                growPlant.Grow();
            }

            if(launch != null)
            {
                launch.Punch();
            }

            if(platform != null)
            {
                platform.Activate();
            }

            buttonPressed = true;
        }
    }
}

