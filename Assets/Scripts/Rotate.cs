using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float waitTime;
    private float waitCounter;
    private Quaternion NextRotation;
    [SerializeField] private Quaternion[] Rotations;
    [SerializeField] private int rotationCounter;


    // Start is called before the first frame update
    void Start()
    {
        NextRotation = Rotations[rotationCounter];
        //waitCounter = waitTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, NextRotation, speed);

        if (transform.rotation == NextRotation)
        {
            if (waitCounter <= 0)
            {
                if (rotationCounter < Rotations.Length - 1)
                {
                    rotationCounter++;
                }
                else
                {
                    rotationCounter = 0;
                }

                NextRotation = Rotations[rotationCounter];

                waitCounter = waitTime;
            }
            else
            {
                waitCounter -= Time.fixedDeltaTime;
            }
        }
    }
}
