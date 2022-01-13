using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int value;
    //public GameObject pickupEffect;
    public AudioSource collectSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<Health>().healPlayer(value);
            //GameObject effect = Instantiate(pickupEffect, transform.position, transform.rotation);
            collectSound.Play();
            Destroy(gameObject);
            //Destroy(effect, 1.2f);
        }
    }
}
