using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    public int damage = 1;
    public Enemy enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && enemy.canHurt)
        {
            Debug.Log("hit");
            Vector3 hitDirection = other.transform.position - transform.position;
            other.transform.position = new Vector3(other.transform.position.x + hitDirection.x, other.transform.position.y, other.transform.position.z + hitDirection.z);
            hitDirection = hitDirection.normalized;

            FindObjectOfType<Health>().HurtPlayer(damage, hitDirection);
        }
    }
}
