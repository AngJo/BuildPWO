using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour {

    public enum PerformAction {
    WAIT,
    TAKEACTION,
    PERFORMACTION
    }

    public PerformAction battleState;

    public List<HandleTurn> performList = new List<HandleTurn>();
    public List<GameObject> heroesinGame = new List<GameObject>();
    public List<GameObject> enemiesinGame = new List<GameObject>();

	// Use this for initialization
	void Start () {
        battleState = PerformAction.WAIT;
        enemiesinGame.AddRange(GameObject.FindGameObjectsWithTag("enemy"));
        heroesinGame.AddRange(GameObject.FindGameObjectsWithTag("hero"));
	}
	
	// Update is called once per frame
	void Update () {
        switch (battleState) {
            case (PerformAction.WAIT):
                if (performList.Count > 0)
                {
                    battleState = PerformAction.TAKEACTION;
                }
                break;
            case (PerformAction.TAKEACTION):
                GameObject performer = GameObject.Find(performList[0].Attacker);
                if (performList[0].type == "Enemy") {
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    ESM.heroToAttack = performList[0].AttackersTarget;
                    ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                }
                if (performList[0].type == "Hero")
                {

                }
                battleState = PerformAction.PERFORMACTION;
                break;
            case (PerformAction.PERFORMACTION): break;
        }
	}

    public void CollectActions(HandleTurn input) {
        performList.Add(input);
    }
}
