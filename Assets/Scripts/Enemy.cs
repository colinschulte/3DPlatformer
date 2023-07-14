using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player player;
    public CharacterController controller;
    public Vector3 movePosition;
    public bool canMove = true;

    [SerializeField] private float MoveSpeed = 2;
    [SerializeField] private float MaxSpeed = 5;

    [SerializeField] private float MaxDistance = 10;
    [SerializeField] private float MinDistance = 2;
    [SerializeField] private float gravityScale;

    // Start is called before the first frame update
    void Start()
    {
        gravityScale = player.gravityScale;
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (canMove)
        {
            transform.LookAt(player.transform);
            Quaternion g = transform.rotation;
            g.x = 0;
            g.z = 0;
            transform.rotation = g;

            if (Vector3.Distance(transform.position, player.transform.position) <= MaxDistance && Vector3.Distance(transform.position, player.transform.position) >= MinDistance)
            {
                movePosition = transform.forward * MoveSpeed;
            }
            else
            {
                movePosition = Vector3.zero;
            }
        
            movePosition.y += Physics.gravity.y * (gravityScale - 1);


            if (controller.isGrounded)
            {
                movePosition.y = 0;
            }

            controller.Move(movePosition * Time.deltaTime);
        }
    }

    public void Death()
    {
        canMove = false;
        this.transform.localScale += new Vector3(0f, -0.25f, 0f);
        this.transform.localPosition += new Vector3(0f, -0.3f, 0f);
        Destroy(this.gameObject, 5f);

    }
}
