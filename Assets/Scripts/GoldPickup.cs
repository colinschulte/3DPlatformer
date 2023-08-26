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
            Debug.Log("Cheese Already Collected");
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
            if (gameObject.CompareTag("Coin"))
            {
                levelManager.AddCoins(value);
            }
            else if(gameObject.CompareTag("Cheese"))
            {
                levelManager.CheeseGet(id);
            }
            GameObject effect = Instantiate(pickupEffect, transform.position, transform.rotation);
            collectSound.Play();
            Destroy(gameObject);
            Destroy(effect, 1.2f);
        }
    }
}
