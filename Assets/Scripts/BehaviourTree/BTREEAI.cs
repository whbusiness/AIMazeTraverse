using bTree;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BTREEAI : bTree.Tree
{
    public static float speed = 100;
    public Stats statistic;
    public static DateTime startTime;
    public static TextMeshProUGUI coinsTxt, dirTxt, stateTxt, aiTypeTxt, waypointTxt;
    public static bool isFinished = false;
    public static int waypointsVisited;

    protected override Node SetupTree()
    {
        Debug.Log("Creating Tree");
        statistic = FindObjectOfType<Stats>();
        coinsTxt = GameObject.Find("CoinsTxt").GetComponent<TextMeshProUGUI>();
        dirTxt = GameObject.Find("DirectionTxt").GetComponent<TextMeshProUGUI>();
        stateTxt = GameObject.Find("StateTxt").GetComponent<TextMeshProUGUI>();
        aiTypeTxt = GameObject.Find("AiTypeTxt").GetComponent<TextMeshProUGUI>();
        waypointTxt = GameObject.Find("WaypointTxt").GetComponent<TextMeshProUGUI>();
        aiTypeTxt.SetText("Type: Btree");
        waypointsVisited = 0;
        coinsTxt.SetText("Coins: " + statistic.btreeVariables.amountOfCoins);
        startTime = DateTime.Now;
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new FindCoinBehaviour(GameObject.FindGameObjectWithTag("BTREEAI").transform),
                new MoveToCoinBehaviour(GameObject.FindGameObjectWithTag("BTREEAI").transform),
            }),
            new PatrolBehaviour(GameObject.FindGameObjectWithTag("BTREEAI").transform, FindClosestTileToMoveTo().transform),
        });
        //Node root = new PatrolBehaviour(GameObject.FindGameObjectWithTag("AI").transform, FindClosestTileToMoveTo().transform);
        return root;
    }
    public static GameObject FindClosestTileToMoveTo()
    {
        List<GameObject> listOfTiles = new(); float closestDist = Mathf.Infinity; GameObject closestTile = null;
        listOfTiles.Clear();
        listOfTiles.AddRange(GameObject.FindGameObjectsWithTag("Waypoints"));
        closestDist = Mathf.Infinity;
        foreach (var tile in listOfTiles)
        {
            float getDist = (GameObject.FindGameObjectWithTag("BTREEAI").transform.position - tile.transform.position).sqrMagnitude;
            if (getDist < closestDist)
            {
                closestTile = tile;
                closestDist = getDist;
            }
        }
        return closestTile;
    }


}
