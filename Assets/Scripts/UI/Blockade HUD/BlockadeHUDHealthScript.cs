using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockadeHUDHealthScript : MonoBehaviour
{
    public Image healthBarOutline;
    public Image healthBarBox;
    public Image healthBar;
    float health;
    float maxHealth;
    float lerpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        lerpSpeed = 6f * Time.deltaTime;
        healthBar.color = Color.blue;
        GetHealthValues();
        HealthBarFiller();

        healthBarOutline.enabled = false;
        healthBarBox.enabled = false;
        healthBar.enabled = false;
    }

    void GetHealthValues()
    {
        health = transform.parent.gameObject.GetComponent<HealthScript>().CurrentHealth;
        maxHealth = transform.parent.gameObject.GetComponent<HealthScript>().MaxHealth;
    }

    void HealthBarFiller()
    {
        if (health != 0)
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, lerpSpeed);
        }
        else
        {
            healthBar.fillAmount = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetHealthValues();

        Debug.Log("Current Health of Blockade: " + health + "\nMax health of blockade: " + maxHealth);

        HealthBarFiller();

        if (transform.parent.gameObject.GetComponent<HealthScript>().IsDead == false)
        {
            if (health < maxHealth)
            {
                healthBarOutline.enabled = true;
                healthBarBox.enabled = true;
                healthBar.enabled = true;
            }
        }
        else
        {
            healthBarOutline.enabled = false;
            healthBarBox.enabled = false;
            healthBar.enabled = false;
        }
    }
}
