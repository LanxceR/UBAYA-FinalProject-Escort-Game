using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieHUDHealthScript : MonoBehaviour
{
    public GameObject healthBarObject;

    public Image healthBarOutline;
    public Image healthBarBox;
    public Image healthBar;
    float health;
    float maxHealth;
    float lerpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.color = Color.red;
        lerpSpeed = 6f * Time.deltaTime;

        GetHealthValues();
        HealthBarFiller();

        healthBarOutline.enabled = false;
        healthBarBox.enabled = false;
        healthBar.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        health = transform.parent.gameObject.GetComponent<HealthScript>().CurrentHealth;

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
        //Debug.Log("Current health: " + health + "\nMax Health: " + maxHealth);
    }

    void GetHealthValues()
    {
        health = transform.parent.gameObject.GetComponent<HealthScript>().CurrentHealth;
        maxHealth = transform.parent.gameObject.GetComponent<HealthScript>().MaxHealth;
    }

    void HealthBarFiller()
    {
        if(transform.parent.gameObject.GetComponent<HealthScript>().IsDead == false)
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, lerpSpeed);
        }
        else
        {
            healthBar.fillAmount = 0f;
        }
    }
}
