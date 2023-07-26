using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool buttonPressed = false;
    public OpenDoor openDoor;

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
        Vector3 newPosition = transform.position;
        newPosition.y -= 0.24f;
        transform.position = newPosition;

        openDoor.Open();
        buttonPressed = true;
    }
}

