using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool buttonPressed = false;
    public List<OpenDoor> openDoors;
    public CheeseLaunch launch;

    public void Press()
    {
        if (!buttonPressed) { 
            Vector3 newPosition = transform.position;
            newPosition.y -= 0.24f;
            transform.position = newPosition;

            foreach(OpenDoor openDoor in openDoors)
            {
                openDoor.Open();
            }

            if(launch != null)
            {
                launch.Punch();
            }

            buttonPressed = true;
        }
    }
}

