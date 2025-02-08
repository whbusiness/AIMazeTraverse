using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GoalReachedState : BaseState
{
    public override void EnterState(StateManager ai)
    {
        ai.stateTxt.SetText("State: GoalReached");
        ai.totalTime = ai.timeEnded - ai.timeStarted;
        ai.statistics.fsmVariables.FSM_overallTime += ai.totalTime;
        ai.statistics.aiType = Stats.WhichAI.FSM;
        StateManager.isFinished = true;
        var aiType = GameObject.FindGameObjectWithTag("FSMAI");
        aiType.GetComponent<DestroyAI>().DestroyGameObject();
    }

    public override void UpdateState(StateManager ai)
    {

    }

    public override void OnCollisionEnter(StateManager ai)
    {

    }
}
