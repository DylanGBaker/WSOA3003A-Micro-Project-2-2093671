using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TMP_Text PlayerDamageValue;
    public TMP_Text PlayerDefenseValue;
    public TMP_Text PlayerAbilityPercentage;
    public TMP_Text EnemyDamageValue;
    public TMP_Text EnemyDefenseValue;
    public TMP_Text PlayerHealth;
    public TMP_Text EnemyHealth;
    public TMP_Text EnemyChoice;
    public TMP_Text PlayerTurnText;
    public GameObject abilityButton;

    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Unit playerUnit;
    [SerializeField] Unit enemyUnit;
    [SerializeField] PlayerAbility playerAbility;

    private void Update()
    {
        PlayerDamageValue.text = playerUnit.unitDamage.ToString();
        PlayerDefenseValue.text = playerUnit.unitDefense.ToString();
        PlayerAbilityPercentage.text = playerAbility.abilityHitChance.ToString() + "%";
        PlayerHealth.text = playerUnit.unitHealth.ToString();

        EnemyDamageValue.text = enemyUnit.unitDamage.ToString();
        EnemyDefenseValue.text = enemyUnit.unitDefense.ToString();
        EnemyHealth.text = enemyUnit.unitHealth.ToString();          
    }
}
