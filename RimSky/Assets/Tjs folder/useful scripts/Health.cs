using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int currHealth;
    public Slider healthSlider;

    // Use this for initialization
    void Start()
    {
        currHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currHealth;
        }
        
        if (currHealth <= 0)
        {
           // Debug.Log("dead " + currHealth);
        }
    }

    public void Damage(int amount)
    {
        currHealth = Mathf.Clamp(currHealth - amount, 0, maxHealth);
    }

    public void Heal(int amount)
    {
        currHealth = Mathf.Clamp(currHealth + amount, 0, maxHealth);
    }

    public bool IsAlive()
    {
        return currHealth > 0;
    }

}
