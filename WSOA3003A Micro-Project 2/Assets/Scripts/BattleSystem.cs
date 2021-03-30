using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState { Start, PlayerTurn, EnemyTurn, Won, Lost};

public class BattleSystem : MonoBehaviour
{
    public bool unitDead;
    public bool usedAbility;
    public BattleState state;
    public GameObject Player;
    public GameObject Enemy;
    public int enemyRandomNum;
    public int abilityRandomNumber;

    [SerializeField] SceneMangr sceneMangr;
    [SerializeField] Unit playerUnit;
    [SerializeField] Unit enemyUnit;
    [SerializeField] PlayerAbility playerAbility;
    [SerializeField] GameUI gameUI;


    private void Start()
    {
        unitDead = false;
        usedAbility = false;
        playerUnit.unitHealth = 20;
        enemyUnit.unitHealth = 23;
        state = BattleState.Start;
        StartCoroutine(SetupBattle());
    }

    private void Update()
    {
        setUnitAttackAndDefense();
    }

    private void setUnitAttackAndDefense()
    {
        /*
         * Changing the enemy units attack value and defense value according to the health.
         */
        if (enemyUnit.unitHealth >= 15)
        {
            enemyUnit.unitDamage = 3;
            enemyUnit.unitDefense = 1;
        }

        if (enemyUnit.unitHealth < 15 && enemyUnit.unitHealth > 5)
        {
            enemyUnit.unitDamage = 3;
            enemyUnit.unitDefense = 2;
        }

        if (enemyUnit.unitHealth <= 5)
        {
            enemyUnit.unitDamage = 3;
            enemyUnit.unitDefense = 3;
        }

        /*
         * Changing the players attack and defense values the same way.
         */

        if (playerUnit.unitHealth >= 15)
        {
            playerUnit.unitDamage = 2;
            playerUnit.unitDefense = 2;
        }

        if (playerUnit.unitHealth < 15 && playerUnit.unitHealth > 5)
        {
            playerUnit.unitDamage = 3;
            playerUnit.unitDefense = 2;
        }

        if (playerUnit.unitHealth <= 5)
        {
            playerUnit.unitDamage = 3;
            playerUnit.unitDefense = 2;
        }
    }

    IEnumerator SetupBattle()
    {
        yield return new WaitForSeconds(0.01f);

        playerUnit = Player.GetComponent<Unit>();
        enemyUnit = Enemy.GetComponent<Unit>();

        state = BattleState.PlayerTurn;
        playerTurn();
    }

    public void playerTurn()
    {
        gameUI.PlayerTurnText.text = "Your Turn....";
        gameUI.EnemyChoice.text = "";

        if (usedAbility == true)
            gameUI.PlayerAbilityPercentage.text = "0%";
    }

    public void attackButton()
    {
        if (state != BattleState.PlayerTurn)
            return;

        StartCoroutine(playerAttack());
    }

    public void defenseButton()
    {
        if (state != BattleState.PlayerTurn)
            return;

        StartCoroutine(playerDefense());
    }


    IEnumerator playerAttack()
    {
        if (usedAbility == true)
            unitDead = enemyUnit.useAbility(playerUnit.abilityDamage);
        else
            unitDead = enemyUnit.TakeDamage(playerUnit.unitDamage);

        state = BattleState.EnemyTurn;

        yield return new WaitForSeconds(0.5f);

        if (unitDead)
        {
            state = BattleState.Won;
            sceneMangr.LoadNewScene("Win Scene");
        }
        else
        {
            enemyTurn();
        }

        if (usedAbility == false)
            playerAbility.IncreaseChance(10);
    }


    IEnumerator playerDefense()
    {
        playerUnit.useDefense(playerUnit.unitDefense);

        yield return new WaitForSeconds(0.5f);
        state = BattleState.EnemyTurn;
        enemyTurn();
    }


    public void enemyTurn()
    {
        gameUI.PlayerTurnText.text = "";
        enemyRandomNum = Random.Range(1, 11);  

        if (enemyUnit.unitHealth >= 15) // Checking if enemy health is 15 or above.
        {
            if (enemyRandomNum > 1)// 90% chance of attacking
            {
                StartCoroutine(enemyAttack());
            }
            else
            {
                StartCoroutine(enemyDefense());
            }
        }
        
        if(enemyUnit.unitHealth < 15 && enemyUnit.unitHealth > 5) // Checking if health is above 5 and below 15.
        {
            if (playerUnit.unitHealth == enemyUnit.unitDamage || playerUnit.unitHealth < enemyUnit.unitDamage)
                StartCoroutine(enemyAttack());

            if (enemyRandomNum > 3) // 70% chance of attacking
            {
                StartCoroutine(enemyAttack());
            }
            else
            {
                StartCoroutine(enemyDefense());
            }
        }
        
        if (enemyUnit.unitHealth <= 5) // Checking if health is less than or equal to 5.
        {
            if (playerUnit.unitHealth == enemyUnit.unitDamage || playerUnit.unitHealth < enemyUnit.unitDamage)
                StartCoroutine(enemyAttack());

            if (enemyRandomNum > 9) // 10% chance of attacking
            {
                StartCoroutine(enemyDefense());
            }
            else
            {
                StartCoroutine(enemyAttack());
            }
        }     
    }

    IEnumerator enemyAttack()
    {
        gameUI.EnemyChoice.text = "Enemy is attacking";
        unitDead = playerUnit.TakeDamage(enemyUnit.unitDamage);

        yield return new WaitForSeconds(1f);

        if (unitDead)
        {
            state = BattleState.Lost;
            sceneMangr.LoadNewScene("Loss Scene");
        }
        else
        {
            state = BattleState.PlayerTurn;
            playerTurn();
        }
    }

    IEnumerator enemyDefense()
    {
        gameUI.EnemyChoice.text = "Enemy is defending";
        enemyUnit.useDefense(enemyUnit.unitDefense);

        yield return new WaitForSeconds(1f);
        state = BattleState.PlayerTurn;
        playerTurn();
    }

    public void abilityButton()
    {
        usedAbility = true;
        abilityRandomNumber = Random.Range(1, 11);
        gameUI.abilityButton.SetActive(false);

        if (playerAbility.abilityHitChance == 10)
        {
            if (abilityRandomNumber > 9)
            {
                StartCoroutine(playerAttack());
            }
            else
            {
                enemyTurn();
            }
        }

        if (playerAbility.abilityHitChance == 20)
        {
            if (abilityRandomNumber > 8)
            {
                StartCoroutine(playerAttack());
            }
            else
            {
                enemyTurn();
            }
        }

        if (playerAbility.abilityHitChance == 30)
        {
            if (abilityRandomNumber > 7)
            {
                StartCoroutine(playerAttack());
            }
            else
            {
                enemyTurn();
            }
        }

        if (playerAbility.abilityHitChance == 40)
        {
            if (abilityRandomNumber > 6)
            {
                StartCoroutine(playerAttack());
            }
            else
            {
                enemyTurn();
            }
        }

        if (playerAbility.abilityHitChance == 50)
        {
            if (abilityRandomNumber > 5)
            {
                StartCoroutine(playerAttack());
            }
            else
            {
                enemyTurn();
            }
        }

        if (playerAbility.abilityHitChance == 60)
        {
            if (abilityRandomNumber > 4)
            {
                StartCoroutine(playerAttack());
            }
            else
            {
                enemyTurn();
            }
        }

        if (playerAbility.abilityHitChance == 70)
        {
            if (abilityRandomNumber > 3)
            {
                StartCoroutine(playerAttack());
            }
            else
            {
                enemyTurn();
            }
        }

        if (playerAbility.abilityHitChance == 80)
        {
            if (abilityRandomNumber > 2)
            {
                StartCoroutine(playerAttack());
            }
            else
            {
                enemyTurn();
            }
        }

        if (playerAbility.abilityHitChance == 90)
        {
            if (abilityRandomNumber > 1)
            {
                StartCoroutine(playerAttack());
            }
            else
            {
                enemyTurn();
            }
        }

        if (playerAbility.abilityHitChance >= 100)
        {
                StartCoroutine(playerAttack());
        }
    }
}
