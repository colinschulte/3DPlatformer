using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string TargetScene;
    public LevelManager levelManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelManager.gameManager.lastScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(TargetScene);
        }
    }
}
