using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public CinemachineFreeLook freeLook;

    [SerializeField] public Dictionary<string, bool> CheesesCollected;
    public int NumCheesesCollected = 0;

    public float timer;
    public float musicVolume;

    public float cameraXMax;
    public float cameraYMax;
    public float cameraXCurrent;
    public float cameraYCurrent;
    public float cameraSliderValue;
    public bool xInvert = false;
    public bool yInvert = true;

    public int lastScene = 1;

    // Start is called before the first frame update
    void Start()
    {
        CheesesCollected = new Dictionary<string, bool>();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        freeLook = FindObjectOfType<CinemachineFreeLook>();
        cameraXMax = 200;
        cameraYMax = 2;
    }


    public void AddCheese(string id)
    {
        if (!CheesesCollected.ContainsKey(id))
        {
            CheesesCollected.Add(id, false);
            NumCheesesCollected += 1;
        }
    }
        
}
