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

    private EscorteeScript escorteeScript;
    private HealthScript healthScript;

    // Start is called before the first frame update
    void Start()
    {
        lerpSpeed = 30f * Time.deltaTime;

        healthScript = Utilities.FindParentOfType<HealthScript>(transform, out _);
        escorteeScript = Utilities.FindParentOfType<EscorteeScript>(transform, out _);

        //healthBarOutline.enabled = false;
        //healthBarBox.enabled = false;
        //healthBar.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetHealthValues();
        HealthBarFiller();

        if (healthScript.IsDead == false)
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

    void GetHealthValues()
    {
        health = healthScript.CurrentHealth;
        maxHealth = healthScript.MaxHealth;
        healthText.text = escorteeScript.id.ToString().Replace('_', ' ') + " health";
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
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, targetFillAmount, lerpSpeed);

            Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));
            healthBar.color = healthColor;
        }
        else
        {
            healthBar.fillAmount = 0f;
        }
    }
}
