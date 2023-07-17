using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    Player player;
    Enemy enemy;

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
            player.moveDirection.y += player.JumpForce;
            player.enemyStomped = true;
            enemy = other.GetComponentInParent<Enemy>();
            enemy.Death();
        }
    }
}
