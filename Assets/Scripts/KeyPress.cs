using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPress : MonoBehaviour
{
    [SerializeField] MazeInput mazeInput;
    [SerializeField] string direction;

    public void Press()
    {
        mazeInput.Move(direction);
    }
}
