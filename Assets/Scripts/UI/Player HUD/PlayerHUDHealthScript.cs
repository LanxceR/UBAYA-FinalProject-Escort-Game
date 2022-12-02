using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDHealthScript : MonoBehaviour
{
    public Image healthBar;
    float health;
    float maxHealth;
    float lerpSpeed;

    private HealthScript healthScript;

    // Start is called before the first frame update
    void Start()
    {
        lerpSpeed = 50f * Time.deltaTime;

        healthScript = Utilities.FindParentOfType<HealthScript>(transform, out _);
    }

    // Update is called once per frame
    void Update()
    {
        GetHealthValues();
        HealthBarFiller();
        ColorChanger();

        //Debug.Log("Current health: " + health + "\nMax Health: " + maxHealth);
    }

    void GetHealthValues()
    {
        health = healthScript.CurrentHealth;
        maxHealth = healthScript.MaxHealth;
    }

    void HealthBarFiller()
    {
        if (healthScript.IsDead == false)
        {
            float targetFillAmount = 0;
            if (health != 0 || maxHealth != 0 || !float.IsNaN(health / maxHealth))
            {
                targetFillAmount = health / maxHealth;
            }

            if (float.IsNaN(healthBar.fillAmount)) healthBar.fillAmount = 0;
            //healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, targetFillAmount, lerpSpeed);
            healthBar.fillAmount = targetFillAmount;
        }
        else
        {
            healthBar.fillAmount = 0f;
        }
    }

    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));
        healthBar.color = healthColor;
    }
}
