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

    private HealthScript healthScript;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.color = Color.red;
        lerpSpeed = 6f * Time.deltaTime;

        healthScript = Utilities.FindParentOfType<HealthScript>(transform, out _);

        healthBarOutline.enabled = false;
        healthBarBox.enabled = false;
        healthBar.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetHealthValues();
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
        health = healthScript.CurrentHealth;
        maxHealth = healthScript.MaxHealth;
    }

    void HealthBarFiller()
    {
        if(healthScript.IsDead == false)
        {
            float targetFillAmount = 0;
            if (health != 0 || maxHealth != 0 || !float.IsNaN(health / maxHealth))
            {
                targetFillAmount = health / maxHealth;
            }

            if (float.IsNaN(healthBar.fillAmount)) healthBar.fillAmount = 0;
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, targetFillAmount, lerpSpeed);
        }
        else
        {
            healthBar.fillAmount = 0f;
        }
    }
}
