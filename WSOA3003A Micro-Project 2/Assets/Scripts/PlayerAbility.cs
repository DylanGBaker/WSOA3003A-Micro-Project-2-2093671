using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    public int abilityHitChance = 0;

    public void IncreaseChance(int ChanceIncrease)
    {
        abilityHitChance += ChanceIncrease;
    }
}
