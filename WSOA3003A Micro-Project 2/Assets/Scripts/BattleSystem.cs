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
    public GameObject playerAttackSymbol;
    public GameObject playerDefenceSymbol;
    public GameObject enemyAttackSymbol;
    public GameObject enemyDefenceSymbol;
    public GameObject playerAbilitySymbol;
    public int enemyRandomNum;
    public int abilityRandomNumber;

    [SerializeField] SceneMangr sceneMangr;
    [SerializeField] Unit playerUnit;
    [SerializeField] Unit enemyUnit;
    [SerializeField] PlayerAbility playerAbility;
    [SerializeField] GameUI gameUI;
    [SerializeField] HealthBar playerHealthBar;
    [SerializeField] HealthBar enemyHealthBar;
    [SerializeField] PartcleSystm partcleSystm;


    private void Start()
    {
        gameUI.enemiesChoicePercentage.text = "Enemy has a 90% of attacking";
        unitDead = false;
        usedAbility = false;
        playerUnit.unitHealth = 20;
        enemyUnit.unitHealth = 23;
        playerHealthBar.setHealth(playerUnit.unitHealth);
        enemyHealthBar.setHealth(enemyUnit.unitHealth);
        state = BattleState.Start;
        StartCoroutine(SetupBattle());
        playerAttackSymbol.SetActive(false);
        playerDefenceSymbol.SetActive(false);
        enemyAttackSymbol.SetActive(false);
        enemyDefenceSymbol.SetActive(false);
        playerAbilitySymbol.SetActive(false);
    }

    private void Update()
    {
        setUnitAttackAndDefense();

        if (enemyUnit.unitHealth >= 15)
            gameUI.enemiesChoicePercentage.text = "Enemy has a 90% of attacking";

        if (enemyUnit.unitHealth < 15 && enemyUnit.unitHealth > 5)
            gameUI.enemiesChoicePercentage.text = "Enemy has a 70% of attacking";

        if (enemyUnit.unitHealth <= 5)
            gameUI.enemiesChoicePercentage.text = "Enemy has a 10% of attacking";
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
        playerAttackSymbol.SetActive(false);
        playerDefenceSymbol.SetActive(false);
        enemyAttackSymbol.SetActive(false);
        enemyDefenceSymbol.SetActive(false);
        playerAbilitySymbol.SetActive(false);

        gameUI.PlayerTurnText.text = "Your Turn....";

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
        playerAttackSymbol.SetActive(true);
        unitDead = enemyUnit.TakeDamage(playerUnit.unitDamage);
        enemyHealthBar.setHealth(enemyUnit.unitHealth);
        state = BattleState.EnemyTurn;
        Instantiate(partcleSystm.enemyBloodeffect, enemyUnit.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(2f);

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
        {
            playerAbility.IncreaseChance(10);
        }
        else if (usedAbility == true)
        {
            playerAbility.abilityHitChance = 0;
        }
    }


    IEnumerator playerDefense()
    {
        playerDefenceSymbol.SetActive(true);
        playerUnit.useDefense(playerUnit.unitDefense);
        playerHealthBar.setHealth(playerUnit.unitHealth);
        Instantiate(partcleSystm.defenseEffct, playerUnit.transform.position, Quaternion.identity);
        state = BattleState.EnemyTurn;

        yield return new WaitForSeconds(2f);  
        enemyTurn();
    }


    public void enemyTurn()
    {
        playerAttackSymbol.SetActive(false);
        playerDefenceSymbol.SetActive(false);
        enemyAttackSymbol.SetActive(false);
        enemyDefenceSymbol.SetActive(false);
        playerAbilitySymbol.SetActive(false);

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
        enemyAttackSymbol.SetActive(true);
        unitDead = playerUnit.TakeDamage(enemyUnit.unitDamage);
        playerHealthBar.setHealth(playerUnit.unitHealth);
        Instantiate(partcleSystm.playerBloodeffect, playerUnit.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(2f);

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
        enemyDefenceSymbol.SetActive(true);
        enemyUnit.useDefense(enemyUnit.unitDefense);
        enemyHealthBar.setHealth(enemyUnit.unitHealth);
        Instantiate(partcleSystm.defenseEffct, enemyUnit.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(2f);
        state = BattleState.PlayerTurn;
        playerTurn();
    }

    public void abilityButton()
    {
        if (state != BattleState.PlayerTurn)
            return;

        playerAbilitySymbol.SetActive(true);
        usedAbility = true;
        abilityRandomNumber = Random.Range(1, 11);
        gameUI.abilityButton.SetActive(false);

        if (playerAbility.abilityHitChance == 10)
        {
            if (abilityRandomNumber > 9)
            {
                unitDead = enemyUnit.useAbility(playerUnit.abilityDamage);

                if (enemyUnit.unitHealth <= 0)
                {
                    state = BattleState.Won;
                    sceneMangr.LoadNewScene("Win Scene");
                }
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
                unitDead = enemyUnit.useAbility(playerUnit.abilityDamage);

                if (enemyUnit.unitHealth <= 0)
                {
                    state = BattleState.Won;
                    sceneMangr.LoadNewScene("Win Scene");
                }
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
                unitDead = enemyUnit.useAbility(playerUnit.abilityDamage);

                if (enemyUnit.unitHealth <= 0)
                {
                    state = BattleState.Won;
                    sceneMangr.LoadNewScene("Win Scene");
                }
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
                unitDead = enemyUnit.useAbility(playerUnit.abilityDamage);

                if (enemyUnit.unitHealth <= 0)
                {
                    state = BattleState.Won;
                    sceneMangr.LoadNewScene("Win Scene");
                }
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
                unitDead = enemyUnit.useAbility(playerUnit.abilityDamage);

                if (enemyUnit.unitHealth <= 0)
                {
                    state = BattleState.Won;
                    sceneMangr.LoadNewScene("Win Scene");
                }
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
                unitDead = enemyUnit.useAbility(playerUnit.abilityDamage);

                if (enemyUnit.unitHealth <= 0)
                {
                    state = BattleState.Won;
                    sceneMangr.LoadNewScene("Win Scene");
                }
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
                unitDead = enemyUnit.useAbility(playerUnit.abilityDamage);

                if (enemyUnit.unitHealth <= 0)
                {
                    state = BattleState.Won;
                    sceneMangr.LoadNewScene("Win Scene");
                }
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
                unitDead = enemyUnit.useAbility(playerUnit.abilityDamage);

                if (enemyUnit.unitHealth <= 0)
                {
                    state = BattleState.Won;
                    sceneMangr.LoadNewScene("Win Scene");
                }
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
                unitDead = enemyUnit.useAbility(playerUnit.abilityDamage);

                if (enemyUnit.unitHealth <= 0)
                {
                    state = BattleState.Won;
                    sceneMangr.LoadNewScene("Win Scene");
                }
            }
            else
            {
                enemyTurn();
            }
        }

        if (playerAbility.abilityHitChance >= 100)
        {
            unitDead = enemyUnit.useAbility(playerUnit.abilityDamage);

            if (enemyUnit.unitHealth <= 0)
            {
                state = BattleState.Won;
                sceneMangr.LoadNewScene("Win Scene");
            }
                
        }
    }
}
