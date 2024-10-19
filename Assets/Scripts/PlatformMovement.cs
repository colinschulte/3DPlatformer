using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float waitTime;
    private float waitCounter;
    private Vector3 NextPosition;
    [SerializeField] private Vector3[] PlatformPositions;
    [SerializeField] private int positionCounter;


    // Start is called before the first frame update
    void Start()
    {
        NextPosition = PlatformPositions[positionCounter];
        //waitCounter = waitTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, NextPosition, speed * Time.deltaTime);

        if(transform.localPosition == NextPosition)
        {
            if(waitCounter <= 0)
            {
                if (positionCounter < PlatformPositions.Length - 1)
                {
                    positionCounter++;
                }
                else
                {
                    positionCounter = 0;
                }

                NextPosition = PlatformPositions[positionCounter];

                waitCounter = waitTime;
            }
            else
            {
                waitCounter -= Time.fixedDeltaTime;
            }
        }
    }
}
