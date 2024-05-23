using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOn : MonoBehaviour
{
    Player player;
    Enemy enemy;
    Button button;
    KeyPress keyPress;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WeakPoint"))
        {
            player.moveDirection.y += player.jumpForce;
            player.enemyStomped = true;
            enemy = other.GetComponentInParent<Enemy>();
            if (!enemy.defeated)
            {
                enemy.Death();
            }
        }
        else if (other.gameObject.CompareTag("Button") && player.isGroundPounding)
        {
            button = other.GetComponent<Button>();
            player.canMove = false;
            button.Press();
        }
        else if (other.gameObject.CompareTag("KeyPress") && player.isGroundPounding)
        {
            keyPress = other.GetComponent<KeyPress>();
            keyPress.Press();
        }
    }
}
