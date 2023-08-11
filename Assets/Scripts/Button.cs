using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool buttonPressed = false;
    public List<OpenDoor> openDoors;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            buttonPressed = true;
        }
    }
}

