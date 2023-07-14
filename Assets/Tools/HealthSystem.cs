using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HealthSystem
{

    public event EventHandler OnHealthChanged; 
    
    private float health;

    private float healthMax;

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
        
    } 
    
    public float HealthMax
    {
        get
        {
            return healthMax;
        }
        set
        {
            healthMax = value;
        }
        
    } 

    public HealthSystem(float healthMax)
    {
        this.health = healthMax;
        this.healthMax = healthMax;
    }
    
    public float GetHealthPercent()
    {
        return health / healthMax;
    }

    public bool Damage(float damageAmount) //return false if health < 0
    {
        bool alive = true;
        health -= damageAmount;
        if (health <= 0f)
        {
            health = 0f;
            alive = false;
        }
        
        if (OnHealthChanged != null)
            OnHealthChanged(this, EventArgs.Empty);

        return alive;
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
        if (health > healthMax)
            health = healthMax;
        
        if (OnHealthChanged != null)
            OnHealthChanged(this, EventArgs.Empty);
    }
    
    
    
}
