using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public CinemachineFreeLook freeLook;

    public int CheesesCollected;

    public float musicVolume;

    public float CameraXMax;
    public float CameraYMax;
    public float CameraXCurrent;
    public float CameraYCurrent;
    public bool xInvert = false;
    public bool yInvert = true;

    // Start is called before the first frame update
    void Start()
    {
        
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
        CameraXMax = 200;
        CameraYMax = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddCheese()
    {
        CheesesCollected += 1;
    }
        
}
