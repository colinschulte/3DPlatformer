using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuFunction : MonoBehaviour
{
    public AudioSource buttonPress;
    public void PlayGame()
    {
        buttonPress.Play();
        SceneManager.LoadScene("Overworld");
    }
    public void QuitGame()
    {
        buttonPress.Play();
        Application.Quit();
    }
}
