using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private float newX;
    [SerializeField] private float newY;
    [SerializeField] private float newZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open()
    {
        //Quaternion newRotation = transform.rotation;
        //newRotation.y = 90f;
        //transform.rotation = newRotation;
        transform.Rotate(newX, newY, newZ, Space.Self);
    }
}
