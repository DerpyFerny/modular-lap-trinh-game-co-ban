using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum BattleState { START, PLAYER_TURN, ENEMY_TURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerStation;
    public Transform enemyStation;

    public BattleHUD enemyHUD;
    public BattleHUD playerHUD;
    private Unit playerUnit;
    private PlayerAttackChoice playerDamage;
    private Unit enemyUnit;
    public BattleState state;
    public UnityEvent BattleUIEnable;
    public UnityEvent BattleUIDisable;
    private GameManagerRPG gmRPG;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerObj = Instantiate(playerPrefab, playerStation);
        playerObj.GetComponent<EmenyCollided>().enabled = false;
        playerObj.GetComponent<EDmovement>().enabled = false;
        playerUnit = playerObj.GetComponent<Unit>();
        playerDamage = playerObj.GetComponent<PlayerAttackChoice>(); 

        GameObject enemyObj = Instantiate(enemyPrefab, enemyStation);
        enemyUnit = enemyObj.GetComponent<Unit>();

        enemyHUD.SetHUD(enemyUnit);
        playerHUD.SetHUD(playerUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYER_TURN;
    }
    public void OnPunchButton(PlayerAttackChoice playerDamage)
    {
        if(state != BattleState.PLAYER_TURN)
            return;
        StartCoroutine(PlayerAttack(playerDamage.punchDamage));
        BattleUIDisable.Invoke();
    }

    public void OnKickButton(PlayerAttackChoice playerDamage)
    {
        if(state != BattleState.PLAYER_TURN)
            return;
        StartCoroutine(PlayerAttack(playerDamage.kickDamage));
        BattleUIDisable.Invoke();
    }

    IEnumerator PlayerAttack(float damage)
    {
        bool isDead = enemyUnit.TakeDamage(damage);

        enemyHUD.SetHP(enemyUnit.currentHP,enemyUnit.maxHP);

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMY_TURN;
            StartCoroutine(EnemyTurn());
        }
    }

    //Ending battle
    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            enemyHUD.uniqueDialogueText.text = "you won";
            gmRPG.GetComponent<GameManagerRPG>().changeSceneFromBattle(playerUnit);
        } else if (state == BattleState.LOST)
        {
            enemyHUD.uniqueDialogueText.text = "you lost";
        }
    }
    IEnumerator EnemyTurn()
    {
        enemyHUD.uniqueDialogueText.text = enemyUnit.name + " Attacked!";
        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP, playerUnit.maxHP);

        yield return new WaitForSeconds(1f);
        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYER_TURN;
            BattleUIEnable.Invoke();
        }
    }
}
