using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOn : MonoBehaviour
{
    Player player;
    Enemy enemy;
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WeakPoint"))
        {
            player.moveDirection.y += player.jumpForce;
            player.enemyStomped = true;
            enemy = other.GetComponentInParent<Enemy>();
            if(!enemy.defeated)
            {
                enemy.Death();
            }
        }
        if (other.gameObject.CompareTag("Button") && player.isGroundPounding)
        {
            button = other.GetComponent<Button>();
            button.Press();
        }
    }
}
