using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeInput : MonoBehaviour
{
    //[SerializeField] private Button upButton;
    //[SerializeField] private Button downButton;
    //[SerializeField] private Button leftButton;
    //[SerializeField] private Button rightButton;
    private int maze = 0;
    private int location = 0;
    private List<List<string>> mazeList = new List<List<string>> ();
    [SerializeField] private Image player;

    // Start is called before the first frame update
    void Start()
    {
        List<string> maze1 = new List<string>();
        maze1.Add("R");
        maze1.Add("R");
        maze1.Add("R");
        maze1.Add("R");
        mazeList.Add(maze1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(string direction)
    {
        Debug.Log(direction);
        //if (direction == mazeList[maze][location])
        //{
            Vector2 position = player.rectTransform.anchoredPosition;
            switch (direction)
            {
                case "U":
                    position.y += 130f;
                    break;
                case "D":
                    position.y -= 130f;
                    break;
                case "L":
                    position.x += 130f;
                    break;
                case "R":
                    position.x -= 130f;
                    break;
            }
            player.rectTransform.anchoredPosition = position;
        //}
        //else
        //{
        //    Reset();
        //}
    }

    public void Reset()
    {
        
    }
}
