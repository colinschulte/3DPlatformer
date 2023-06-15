using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public Text healthText;
    public Image blackScreen;
    private bool isFadingDown;
    private bool isFadingUp;
    public float fadeSpeed;

    public Player player;

    public float invincibilityLength;
    private float invincibilityCounter;

    public Renderer playerRenderer;

    public float flashLength = 0.15f;
    private float flashCounter;

    private bool isRespawning;
    private Vector3 respawnPoint;

    public float respawnLength;
    public float waitForFade;

    public GameObject deathEffect;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthText.text = "Health: " + currentHealth;
        respawnPoint = player.transform.position;
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1f);
        isFadingUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;
            if(flashCounter <= 0)
            {
                playerRenderer.enabled = !playerRenderer.enabled;
                flashCounter = flashLength;
            }
            if(invincibilityCounter <= 0)
            {
                playerRenderer.enabled = true;
            }
        }

        if (isFadingDown)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if(blackScreen.color.a == 1f)
            {
                isFadingDown = false;
            }
        }

        if(isFadingUp)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (blackScreen.color.a == 0f)
            {
                isFadingUp = false;
            }
        }
    }

    public void hurtPlayer(int damage, Vector3 direction)
    {
        if(invincibilityCounter <= 0) 
        {
            currentHealth -= damage;
            
            if(currentHealth <= 0)
            {
                Respawn();
            }
            else
            {
                healthText.text = "Health: " + currentHealth;

                player.Knockback(direction);

                invincibilityCounter = invincibilityLength;
                playerRenderer.enabled = false;
                flashCounter = flashLength;
            }
        }
    }

    public void Respawn()
    {
        if (!isRespawning)
        {
            StartCoroutine("RespawnCo");
        }
    }

    public IEnumerator RespawnCo()
    {
        isRespawning = true;
        player.gameObject.SetActive(false);
        GameObject killEffect = Instantiate(deathEffect, player.transform.position, player.transform.rotation);

        yield return new WaitForSeconds(respawnLength);

        isFadingDown = true;

        yield return new WaitForSeconds(waitForFade);

        isFadingDown = false;
        isFadingUp = true;
        Destroy(killEffect);

        isRespawning = false;
        player.gameObject.SetActive(true);

        CharacterController charController = player.GetComponent<CharacterController>();
        charController.enabled = false;
        player.transform.position = respawnPoint;
        charController.enabled = true;

        currentHealth = maxHealth;
        healthText.text = "Health: " + currentHealth;
        invincibilityCounter = invincibilityLength;
        playerRenderer.enabled = false;
        flashCounter = flashLength;
    }

    public void healPlayer(int value)
    {
        if (invincibilityCounter <= 0)
        {
            currentHealth += value;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            healthText.text = "Health: " + currentHealth;
        }
    }
}

