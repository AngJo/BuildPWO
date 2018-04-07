using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour {
    public BaseEnemy enemy;
    private BattleStateMachine BSM;
    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }

    public TurnState currentState;

    //for the ProgressBar
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    private Vector3 startPosition;
    private bool actionStarted = false;
    public GameObject heroToAttack;
    private float animSpeed = 5f;
    // Use this for initialization

    void Start() {
        currentState = TurnState.PROCESSING;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update() {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                UpgradeProgressBar();
                break;

            case (TurnState.CHOOSEACTION):
                ChooseAction();
                currentState = TurnState.WAITING;
                break;

            case (TurnState.WAITING): break;

            case (TurnState.ACTION): StartCoroutine(TimeForAction());
                break;

            case (TurnState.DEAD): break;
        }
    }

    void UpgradeProgressBar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        float calc_cooldown = cur_cooldown / max_cooldown;
        if (cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.CHOOSEACTION;
        }
    }

    void ChooseAction() {

        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = enemy.name;
        myAttack.type = "Enemy";
        myAttack.AttacksGameObject = this.gameObject;
        myAttack.AttackersTarget = BSM.heroesinGame[Random.Range(0, BSM.heroesinGame.Count)];
        BSM.CollectActions(myAttack);
    }

    private IEnumerator TimeForAction() {
        if (actionStarted) {
            yield break;
        }

        actionStarted = true;
        Vector3 heroPosition = new Vector3(heroToAttack.transform.position.x-1.5f, heroToAttack.transform.position.y, heroToAttack.transform.position.z);
        while (MoveTowardsEnemy(heroPosition)) {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        Vector3 firstPosition = startPosition;
        while (MoveTowardsStart(firstPosition)) { yield return null; }

        BSM.performList.RemoveAt(0);
        BSM.battleState = BattleStateMachine.PerformAction.WAIT;

        actionStarted = false;
        cur_cooldown = 0f;
        currentState = TurnState.PROCESSING;
    }

    private bool MoveTowardsEnemy(Vector3 target) {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));

    }

    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));

    }
}
