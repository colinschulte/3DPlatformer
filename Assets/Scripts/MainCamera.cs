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
        GameObject pauseObject = GameObject.Find("PauseObject");
        PauseGame pauseGame = pauseObject.GetComponent<PauseGame>();
        //if(pauseGame.gamePaused != true)
        //{
        //    lookDirection = look.ReadValue<Vector2>();

        //    float horizontal = lookDirection.x * xRotateSpeed;
        //    if (invertX)
        //    {
        //        pivot.Rotate(0, -horizontal, 0);
        //    }
        //    else
        //    {
        //        pivot.Rotate(0, horizontal, 0);
        //    }

        //    float vertical = lookDirection.y * yRotateSpeed;
        //    if (invertY)
        //    {
        //        pivot.Rotate(-vertical, 0, 0);
        //    }
        //    else
        //    {
        //        pivot.Rotate(vertical, 0, 0);
        //    }

        //    //limit up/down camera rotation
        //    if (pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180f)
        //    {
        //        pivot.rotation = Quaternion.Euler(maxViewAngle, 0, 0);
        //    }
            
        //    if (pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < 360f + minViewAngle)
        //    {
        //        pivot.rotation = Quaternion.Euler(360f + minViewAngle, 0, 0);
        //    }



        //    float zoom = playerControls.UI.ScrollWheel.ReadValue<Vector2>().y * zRotateSpeed;


        //    float desiredYAngle = pivot.eulerAngles.y;
        //    float desiredXAngle = pivot.eulerAngles.x;

        //    Quaternion rotation = Quaternion.Euler(-desiredXAngle, desiredYAngle, 0);

        //    if(offset.z - zoom < 1)
        //    {
        //        offset = new Vector3(offset.x, offset.y, 1);
        //    }
        //    else
        //    {
        //        offset = new Vector3(offset.x, offset.y, offset.z - zoom);
        //    }

        //    transform.position = target.position - (rotation * offset);

        //    if(transform.position.y < target.position.y)
        //    {
        //        transform.position = new Vector3(transform.position.x, target.position.y - 0.25f, transform.position.z);
        //    }

        //    transform.LookAt(target.transform);
        //}
    }
}
