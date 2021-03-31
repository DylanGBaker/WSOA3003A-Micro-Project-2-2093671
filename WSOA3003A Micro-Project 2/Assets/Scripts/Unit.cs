using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int unitHealth;
    public int unitDamage = 3;
    public int unitDefense = 2;
    public int abilityDamage = 8;

    public bool TakeDamage(int damage)
    {
        unitHealth -= damage;

        if (unitHealth <= 0)
        {
            unitHealth = 0;
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public void useDefense(int defense)
    {
        unitHealth += defense;

        if (unitHealth >= 20)
            unitHealth = 20;
        else
            return;
    }

    public bool useAbility(int abilitydamage)
    {
        unitHealth -= abilitydamage;

        if (unitHealth <= 0)
            return true;
        else
            return false;
    }
}
