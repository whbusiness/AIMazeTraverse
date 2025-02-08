using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class MoveToCoinsState : BaseState
{
    GameObject aiPref, coin;
    float moveToCoinSpeed;
    public override void EnterState(StateManager ai)
    {
        Debug.Log("Coin State Entered");
        moveToCoinSpeed = ai.patrolState.patrolSpeed;
        aiPref = GameObject.FindGameObjectWithTag("FSMAI");
        ai.stateTxt.SetText("State: MoveToCoins");
        coin = ai.patrolState.coinFound;
    }

    public override void UpdateState(StateManager ai)
    {
        if (Vector3.Distance(aiPref.transform.position, coin.transform.position) <= .3f)
        {
            ai.coinsCollected++;
            ai.statistics.fsmVariables.amountOfCoins++;
            ai.coinsTxt.SetText("Coins: " + ai.statistics.fsmVariables.amountOfCoins);
            coin.GetComponent<CoinRotation>().DestroyCoin();
            if(ai.coinsCollected >= ai.maxCoins)
            {
                ai.timeEnded = DateTime.Now;
                ai.SwitchState(ai.goalReachedState);
            }
            else
            {
                ai.SwitchState(ai.patrolState);
            }
        }
        else
        {
            Debug.Log("Move");
            aiPref.transform.position = Vector3.MoveTowards(aiPref.transform.position, new Vector3(coin.transform.position.x, aiPref.transform.position.y, coin.transform.position.z), moveToCoinSpeed * Time.deltaTime);
        }
    }

    public override void OnCollisionEnter(StateManager ai)
    {

    }
}
