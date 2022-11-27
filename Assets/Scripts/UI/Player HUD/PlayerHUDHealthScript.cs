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

    // Start is called before the first frame update
    void Start()
    {
        lerpSpeed = 4f * Time.deltaTime;

        GetHealthValues();
        HealthBarFiller();
        ColorChanger();
    }

    // Update is called once per frame
    void Update()
    {
        health = GameManager.Instance.gamePlayer.ActivePlayer.healthScript.CurrentHealth;

        HealthBarFiller();
        ColorChanger();

        //Debug.Log("Current health: " + health + "\nMax Health: " + maxHealth);
    }

    void GetHealthValues()
    {
        health = GameManager.Instance.gamePlayer.ActivePlayer.healthScript.CurrentHealth;
        maxHealth = GameManager.Instance.gamePlayer.ActivePlayer.healthScript.MaxHealth;
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

    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));
        healthBar.color = healthColor;
    }
}
