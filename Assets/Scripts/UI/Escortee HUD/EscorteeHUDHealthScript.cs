using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EscorteeHUDHealthScript : MonoBehaviour
{
    public Image healthBar;
    public Image healthBarOutline;
    public Image healthBarBox;
    public TextMeshProUGUI healthText;

    float health;
    float maxHealth;
    float lerpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        lerpSpeed = 6f * Time.deltaTime;
        GetInitHealthValues();
        HealthBarFiller();

        //healthBarOutline.enabled = false;
        //healthBarBox.enabled = false;
        //healthBar.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        maxHealth = GameManager.Instance.gameEscortee.ActiveEscortee.healthScript.MaxHealth;
        health = GameManager.Instance.gameEscortee.ActiveEscortee.healthScript.CurrentHealth;

        healthText.text = GameManager.Instance.gameEscortee.ActiveEscortee.id.ToString().Replace('_', ' ') + " health";

        HealthBarFiller();

        if (GameManager.Instance.gameEscortee.ActiveEscortee.healthScript.IsDead == false)
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
            //healthBarOutline.enabled = false;
            //healthBarBox.enabled = false;
            //healthBar.enabled = false;
        }
    }

    void GetInitHealthValues()
    {
        health = GameManager.Instance.gameEscortee.ActiveEscortee.healthScript.CurrentHealth;
        maxHealth = GameManager.Instance.gameEscortee.ActiveEscortee.healthScript.MaxHealth;
        healthText.text = GameManager.Instance.gameEscortee.ActiveEscortee.name + " health";
    }

    void HealthBarFiller()
    {
        if (GameManager.Instance.gameEscortee.ActiveEscortee.healthScript.IsDead == false)
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, lerpSpeed);

            Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));
            healthBar.color = healthColor;
        }
        else
        {
            healthBar.fillAmount = 0f;
        }
    }
}
