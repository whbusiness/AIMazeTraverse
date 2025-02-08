using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class StateManager : MonoBehaviour
{
    BaseState currentState;
    public PatrolState patrolState = new PatrolState();
    public MoveToCoinsState moveToCoinsState = new MoveToCoinsState();
    public GoalReachedState goalReachedState = new GoalReachedState();
    MazeGenerator m_Generator;
    public int coinsCollected, maxCoins;
    public DateTime timeStarted, timeEnded;
    public TimeSpan totalTime;
    public Stats statistics;
    public TextMeshProUGUI coinsTxt, dirTxt, stateTxt, aiTypeTxt, waypointTxt;
    public static bool isFinished = false;
    public static int waypointsVisited;
    // Start is called before the first frame update
    void Start()
    {
        statistics = FindObjectOfType<Stats>();
        coinsTxt = GameObject.Find("CoinsTxt1").GetComponent<TextMeshProUGUI>();
        dirTxt = GameObject.Find("DirectionTxt1").GetComponent<TextMeshProUGUI>();
        stateTxt = GameObject.Find("StateTxt1").GetComponent<TextMeshProUGUI>();
        aiTypeTxt = GameObject.Find("AiTypeTxt1").GetComponent<TextMeshProUGUI>();
        waypointTxt = GameObject.Find("WaypointTxt1").GetComponent<TextMeshProUGUI>();
        aiTypeTxt.SetText("Type: FSM");
        coinsTxt.SetText("Coins: " + statistics.fsmVariables.amountOfCoins);
        timeStarted = DateTime.Now;
        m_Generator = FindObjectOfType<MazeGenerator>();
        waypointsVisited = 0;
        maxCoins = MazeGenerator.maxCoins;
        currentState = patrolState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(BaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
