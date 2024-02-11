using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{
    public Transform target;
    public Transform pivot;
    public Vector3 offset;
    public bool useOffsetValues;
    public bool invertX;
    public bool invertY;
    public float xRotateSpeed;
    public float yRotateSpeed;
    public float zRotateSpeed;

    [SerializeField] private GameObject pauseObject;

    public float maxViewAngle;
    public float minViewAngle;

    public PlayerControls playerControls;
    private InputAction look;

    private Vector3 lookDirection;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!useOffsetValues)
        {
            offset = target.position - transform.position;
        }


        //Uncomment to hide cursor on startup
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        look = playerControls.Player.Look;
        look.Enable();
    }

    private void OnDisable()
    {
        look.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        PauseGame pauseGame = pauseObject.GetComponent<PauseGame>();
    }
}
