using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    public int value;

    [SerializeField] private string id;
    [ContextMenu("generate id")]
    private void GenerateID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public GameObject pickupEffect;
    public AudioSource collectSound;
    public LevelManager levelManager;
    public Material collectedMaterial;
    public bool isCollectable = true;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void Update()
    {
        if (levelManager.gameManager.CheesesCollected != null && levelManager.gameManager.CheesesCollected.ContainsKey(id) && isCollectable)
        {
            Renderer meshRenderer = gameObject.GetComponent<Renderer>();
            var materials = meshRenderer.materials;
            for (var i = 0; i < materials.Length; i++)
            {
                materials[i] = collectedMaterial;
            }
            meshRenderer.materials = materials;
            isCollectable = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (gameObject.CompareTag("Coin"))
            {
                levelManager.AddCoins(value);
                player.crackerTimer = player.UIWait;
            }
            else if(gameObject.CompareTag("Cheese"))
            {
                levelManager.CheeseGet(id);
                player.cheeseTimer = player.UIWait;
            }
            else if (gameObject.CompareTag("Brick"))
            {
                levelManager.BrickGet();
                player.brickTimer = player.UIWait;
            }
            GameObject effect = Instantiate(pickupEffect, transform.position, transform.rotation);
            var main = effect.gameObject.GetComponent<ParticleSystem>().main;
            main.startColor = this.GetComponent<Renderer>().material.color;
            collectSound.Play();
            Destroy(gameObject);
            Destroy(effect, 1.2f);
        }
    }
}
