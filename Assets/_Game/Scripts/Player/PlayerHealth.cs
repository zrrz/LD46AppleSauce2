using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float currentHealth = 100f;

    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private float minHealthRegenRate = 5f;

    [SerializeField]
    private float maxHealthRegenRate = 25f;

    private float currentHealthRegenRate = 0f;

    [SerializeField]
    private float healthRegenDelay = 4f;
    private float currentRegenDelay = 0f;


    [SerializeField]
    private DamageHandler damageHandler;

    [SerializeField]
    private Image healthBarImage;

    [SerializeField]
    private TMPro.TextMeshProUGUI textMesh;

    void Start()
    {
        damageHandler.OnDamageTaken.AddListener(TakeDamage);

        currentHealth = 1f;// maxHealth;
        currentHealthRegenRate = minHealthRegenRate;
    }

    void Update()
    {
        HandleHealthRegen();
    }

    private void TakeDamage(GameObject damageSource, float damageAmount, DamageHandler.DamageDirectionData damageDirectionData)
    {
        currentHealth = Mathf.Clamp(currentHealth - damageAmount, 0f, maxHealth);
        UpdateHealthBar();
        if(currentHealth < 1f)
        {
            Die();
        }
        else
        {
            currentRegenDelay = healthRegenDelay;
            currentHealthRegenRate = minHealthRegenRate;
        }
    }

    private void HandleHealthRegen()
    {
        if(currentRegenDelay > 0f)
        {
            currentRegenDelay -= Time.deltaTime;
        }
        else
        {
            if(currentHealth < maxHealth)
            {
                if(currentHealthRegenRate < maxHealthRegenRate)
                {
                    currentHealthRegenRate = Mathf.MoveTowards(currentHealthRegenRate, maxHealthRegenRate, Time.deltaTime * 3f);
                }
                currentHealth = Mathf.Clamp(currentHealth + (currentHealthRegenRate * Time.deltaTime), 0f, maxHealth);
                UpdateHealthBar();
            }
        }
    }

    private void UpdateHealthBar()
    {
        //TODO slowly move healthbar
        healthBarImage.fillAmount = currentHealth / maxHealth;
        textMesh.text = $"{(int)currentHealth}/{(int)maxHealth}";
    }

    private void Die()
    {
        //TODO pretty all this flow up
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
}
