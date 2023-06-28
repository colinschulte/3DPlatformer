using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    [SerializeField]
    private float speed;

    private Vector3 StartPosition;
    private Vector3 EndPosition;
    private Vector3 NextPosition;
    private Vector3 PlatformPosition;

    [SerializeField]
    private bool positionCounter;


    // Start is called before the first frame update
    void Start()
    {
        StartPosition = transform.localPosition;
        EndPosition = new Vector3 (0, transform.localPosition.y, 0);
        if (positionCounter == true)
        {
            NextPosition = EndPosition;
        } else {
            transform.localPosition = EndPosition;
            NextPosition = StartPosition;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //PlatformPosition.x = Mathf.MoveTowards(PlatformPosition.x, NextPosition.x, speed);
        //PlatformPosition.z = Mathf.MoveTowards(PlatformPosition.z, NextPosition.z, speed);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, NextPosition, speed * Time.deltaTime);

        if(transform.localPosition == NextPosition)
        {
            if (positionCounter == true)
            {
                NextPosition = StartPosition;
                positionCounter = false;
            } else
            {
                NextPosition = EndPosition;
                positionCounter = true;
            }
        }
    }
}
