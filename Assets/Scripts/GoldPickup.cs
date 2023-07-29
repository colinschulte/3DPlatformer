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
        if(other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Coin"))
            {
                FindObjectOfType<LevelManager>().AddCoins(value);
            }
            else if(gameObject.CompareTag("Cheese"))
            {
                FindObjectOfType<LevelManager>().CheeseGet();
            }
            GameObject effect = Instantiate(pickupEffect, transform.position, transform.rotation);
            collectSound.Play();
            Destroy(gameObject);
            Destroy(effect, 1.2f);
        }
    }
}
