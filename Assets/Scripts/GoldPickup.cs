using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    public int value;

    public GameObject pickupEffect;
    public AudioSource collectSound;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            FindObjectOfType<GameManager>().AddCoins(value);
            GameObject effect = Instantiate(pickupEffect, transform.position, transform.rotation);
            collectSound.Play();
            Destroy(gameObject);
            Destroy(effect, 1.2f);
        }
    }
}
