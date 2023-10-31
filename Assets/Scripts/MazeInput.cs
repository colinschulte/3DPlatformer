using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MazeInput : MonoBehaviour
{
    [SerializeField] private GameObject rewardCheese;
    [SerializeField] private GameObject Level2;
    private int maze = 0;
    private int location = 0;
    private List<List<string>> mazeList = new();
    [SerializeField] private UnityEngine.UI.Image player;
    private bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        List<string> maze1 = new()
        {
            "R",
            "R",
            "R"
        };
        mazeList.Add(maze1);
        List<string> maze2 = new()
        {
            "U",
            "U",
            "R",
            "D",
            "D",
            "R",
            "U",
            "U",
            "R",
            "D",
            "D"
        };
        mazeList.Add(maze2);
        rewardCheese.SetActive(false);
        Level2.SetActive(false);
    }

    public void Move(string direction)
    {
        if (direction == mazeList[maze][location] && canMove)
        {
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
                    position.x += 180f;
                    break;
                case "R":
                    position.x -= 180f;
                    break;
            }
            player.rectTransform.anchoredPosition = position;
            location ++;
            if(location >= mazeList[maze].Count)
            {
                maze++;
                location = 0;
                
                if(maze >= mazeList.Count)
                {
                    rewardCheese.SetActive(true);
                    canMove = false;
                }
                else
                {
                    if(maze == 1)
                    {
                        Level2.SetActive(true);
                    }
                    Reset();
                }
            }
        }
        else
        {
            Reset();
        }
    }

    public void Reset()
    {
        player.rectTransform.anchoredPosition = new Vector3(280, -140, 0);
        location = 0;
    }
}
