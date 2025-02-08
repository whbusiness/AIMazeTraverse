using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class FSMAIVariables
{
    public TimeSpan FSM_overallTime;
    public int amountOfCoins;
    public int amountOfWaypointsVisited;
}
[System.Serializable]
public class BTREEAIVariables
{
    public TimeSpan BTREE_overallTime;
    public int amountOfCoins;
    public int amountOfWaypointsVisited;
}

public class Stats : MonoBehaviour
{
    public enum WhichAI
    {
        FSM,
        BTREE
    };
    public WhichAI aiType;
    public FSMAIVariables fsmVariables = new();
    public BTREEAIVariables btreeVariables = new();
    [SerializeField] private TextMeshProUGUI FSM_timeTakenTxt, BTREE_timeTakenTxt, FSM_CoinsTxt, BTREE_CoinsTxt, FSM_WaypointsTxt, BTREE_WaypointsTxt;

    // Start is called before the first frame update
    void Start()
    {
        print("Stats Ran");
        GameObject[] stat = GameObject.FindGameObjectsWithTag("Stats");

        if (stat.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        print("Scene Ran");
        var scene = SceneManager.GetActiveScene();
        if (scene.name == "EndScene")
        {
            print("End Scene");
            FSM_timeTakenTxt = GameObject.Find("FSMTime").GetComponent<TextMeshProUGUI>();
            BTREE_timeTakenTxt = GameObject.Find("BTREETime").GetComponent<TextMeshProUGUI>();
            FSM_CoinsTxt = GameObject.Find("FSMCoins").GetComponent<TextMeshProUGUI>();
            BTREE_CoinsTxt = GameObject.Find("BTREECoins").GetComponent<TextMeshProUGUI>();
            BTREE_WaypointsTxt = GameObject.Find("BTREEWaypointsVisited").GetComponent<TextMeshProUGUI>();
            FSM_WaypointsTxt = GameObject.Find("FSMWaypointsVisited").GetComponent<TextMeshProUGUI>();
        }
        if(FSM_timeTakenTxt != null && BTREE_timeTakenTxt != null)
        {
            FSM_timeTakenTxt.SetText("FSM Total Time: " + fsmVariables.FSM_overallTime);
            BTREE_timeTakenTxt.SetText("BTREE Total Time: " + btreeVariables.BTREE_overallTime);
            FSM_CoinsTxt.SetText("FSM Coins: " + fsmVariables.amountOfCoins);
            BTREE_CoinsTxt.SetText("BTREE Coins: " + btreeVariables.amountOfCoins);
            BTREE_WaypointsTxt.SetText("BTREE Waypoints: " + btreeVariables.amountOfWaypointsVisited);
            FSM_WaypointsTxt.SetText("FSM Waypoints: " + fsmVariables.amountOfWaypointsVisited);
        }
    }
}
