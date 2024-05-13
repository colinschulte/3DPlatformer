using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public Player player;
    [SerializeField] private AudioSource bounceSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //player = other.GetComponent<Player>();
            bounceSound.time = 0.1f;
            bounceSound.Play();
            player.bounceStart = true;
        }
    }
}
