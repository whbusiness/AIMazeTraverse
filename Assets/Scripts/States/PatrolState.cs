using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : BaseState
{
    private List<GameObject> listOfTiles = new();
    public float patrolSpeed;
    [SerializeField] private GameObject currentTile, prevTile, nextTile, leastTileUsed;
    private GameObject closestTile;
    private float closestDist;
    private GameObject aiPref;
    int layerMask;
    private List<GameObject> allTilesNearby = new();
    private float lowestAmountOfTimesUsed = Mathf.Infinity;
    public GameObject coinFound;
    Directions dir = new();
    private string currentDir;
    bool hitMovingWall = false;
    public override void EnterState(StateManager ai)
    {
        Debug.Log("Entered");
        prevTile = null;
        patrolSpeed = 100;
        layerMask = 1 << 7;
        layerMask = ~layerMask;
        hitMovingWall = false;
        ai.stateTxt.SetText("State: Patrol");
        aiPref = GameObject.FindGameObjectWithTag("FSMAI");
        currentTile = FindClosestTileToMoveTo();
        coinFound = null;
    }

    public override void UpdateState(StateManager ai)
    {
        /*if(aiPref != null && currentTile != null)
        {
            Debug.Log(currentTile.gameObject.name);
            if (Vector3.Distance(aiPref.transform.position, currentTile.transform.position) <= 1)
            {
                Debug.Log("New Move");
                currentTile = FindClosestTileToMoveTo();
            }
            else
            {
                Debug.Log("Move");
                aiPref.transform.position = Vector3.MoveTowards(aiPref.transform.position, currentTile.transform.position, patrolSpeed * Time.deltaTime);
            }
        }*/
        if(aiPref == null)
        {
            aiPref = GameObject.FindGameObjectWithTag("FSMAI");
            currentTile = FindClosestTileToMoveTo();
        }
        if(aiPref != null && currentTile != null)
        {
        if (Vector3.Distance(aiPref.transform.position, currentTile.transform.position) <= .3f)
        {
            currentTile.GetComponent<MeshRenderer>().material.color = Color.gray;
                if (Physics.Raycast(currentTile.transform.position, currentTile.transform.forward, out RaycastHit wallHit, 1) && wallHit.collider.gameObject.CompareTag("MovingWall"))
                {
                    hitMovingWall = true;
                }
                else if (Physics.Raycast(currentTile.transform.position, -currentTile.transform.forward, out RaycastHit wallHit2, 1) && wallHit2.collider.gameObject.CompareTag("MovingWall"))
                {
                    hitMovingWall = true;
                }
                else if (Physics.Raycast(currentTile.transform.position, currentTile.transform.right, out RaycastHit wallHit3, 1) && wallHit3.collider.gameObject.CompareTag("MovingWall"))
                {
                    hitMovingWall = true;
                }
                else if (Physics.Raycast(currentTile.transform.position, -currentTile.transform.right, out RaycastHit wallHit4, 1) && wallHit4.collider.gameObject.CompareTag("MovingWall"))
                {
                    hitMovingWall = true;
                }

            else if (Physics.Raycast(currentTile.transform.position, currentTile.transform.forward, out RaycastHit coin1, Mathf.Infinity) && coin1.collider.gameObject.CompareTag("FSMCoins"))
                {
                    hitMovingWall = false;
                    Debug.Log("Hit Coin");
                coinFound = coin1.collider.gameObject;
                ai.SwitchState(ai.moveToCoinsState);
            }
            else if (Physics.Raycast(currentTile.transform.position, currentTile.transform.right, out RaycastHit coin2, Mathf.Infinity) && coin2.collider.gameObject.CompareTag("FSMCoins"))
                {
                    hitMovingWall = false;
                    Debug.Log("Hit Coin");
                coinFound = coin2.collider.gameObject;
                ai.SwitchState(ai.moveToCoinsState);
            }
            else if (Physics.Raycast(currentTile.transform.position, -currentTile.transform.forward, out RaycastHit coin3, Mathf.Infinity) && coin3.collider.gameObject.CompareTag("FSMCoins"))
                {
                    hitMovingWall = false;
                    Debug.Log("Hit Coin");
                coinFound = coin3.collider.gameObject;
                ai.SwitchState(ai.moveToCoinsState);
            }
            else if (Physics.Raycast(currentTile.transform.position, -currentTile.transform.right, out RaycastHit coin4, Mathf.Infinity) && coin4.collider.gameObject.CompareTag("FSMCoins"))
                {
                    hitMovingWall = false;
                    Debug.Log("Hit Coin");
                coinFound = coin4.collider.gameObject;
                ai.SwitchState(ai.moveToCoinsState);
            }
            else
            {
                hitMovingWall = false;
                    currentTile.GetComponent<CheckForDeadend>().WaypointUsed();
                    StateManager.waypointsVisited++;
                    ai.waypointTxt.SetText("Waypoint: " + StateManager.waypointsVisited);
                    ai.statistics.fsmVariables.amountOfWaypointsVisited++;
                    lowestAmountOfTimesUsed = Mathf.Infinity;

                    if (Physics.Raycast(currentTile.transform.position, currentTile.transform.forward, out RaycastHit hit, 1) && hit.collider.gameObject != prevTile)
                {
                    if (hit.collider.gameObject.CompareTag("Waypoints"))
                    {
                            currentDir = dir.forward;
                        allTilesNearby.Add(hit.collider.gameObject);
                    }
                }
                if (Physics.Raycast(currentTile.transform.position, -currentTile.transform.forward, out RaycastHit hit2, 1) && hit2.collider.gameObject != prevTile)
                {
                    if (hit2.collider.gameObject.CompareTag("Waypoints"))
                    {
                            currentDir = dir.back;
                            allTilesNearby.Add(hit2.collider.gameObject);
                    }
                }
                if (Physics.Raycast(currentTile.transform.position, currentTile.transform.right, out RaycastHit hit3, 1) && hit3.collider.gameObject != prevTile)
                {
                    if (hit3.collider.gameObject.CompareTag("Waypoints"))
                    {
                            currentDir = dir.right;
                            allTilesNearby.Add(hit3.collider.gameObject);
                    }
                }
                if (Physics.Raycast(currentTile.transform.position, -currentTile.transform.right, out RaycastHit hit4, 1) && hit4.collider.gameObject != prevTile)
                {
                    if (hit4.collider.gameObject.CompareTag("Waypoints"))
                    {
                            currentDir = dir.left;
                            allTilesNearby.Add(hit4.collider.gameObject);
                    }
                }


                foreach (var tile in allTilesNearby)
                {
                    if (tile.GetComponent<CheckForDeadend>().amountOfTimesUsed < lowestAmountOfTimesUsed)
                    {
                        leastTileUsed = tile;
                        lowestAmountOfTimesUsed = tile.GetComponent<CheckForDeadend>().amountOfTimesUsed;
                    }
                }

                prevTile = currentTile;
                currentTile = leastTileUsed;

                allTilesNearby.Clear();

            }
                ai.dirTxt.SetText("Direction: " + currentDir);




                /*if(Physics.Raycast(currentTile.transform.position, currentTile.transform.forward, out RaycastHit hit, 1) && hit.collider.gameObject.CompareTag("Waypoints") && hit.collider.gameObject != prevTile && hit.collider.gameObject.GetComponent<CheckForDeadend>().amountOfTimesUsed<2)
                {
                    Debug.Log("Hit Forward");
                    prevTile = currentTile;
                    currentTile = hit.collider.gameObject;
                }
                else if(Physics.Raycast(currentTile.transform.position, currentTile.transform.right, out RaycastHit hit2, 1) && hit2.collider.gameObject.CompareTag("Waypoints") && hit2.collider.gameObject != prevTile && hit2.collider.gameObject.GetComponent<CheckForDeadend>().amountOfTimesUsed < 2)
                {
                    Debug.Log("Hit Right");
                    prevTile = currentTile;
                    currentTile = hit2.collider.gameObject;
                }
                else if (Physics.Raycast(currentTile.transform.position, -currentTile.transform.forward, out RaycastHit hit3, 1) && hit3.collider.gameObject.CompareTag("Waypoints") && hit3.collider.gameObject != prevTile && hit3.collider.gameObject.GetComponent<CheckForDeadend>().amountOfTimesUsed < 2)
                {
                    Debug.Log("Hit Backward");
                    prevTile = currentTile;
                    currentTile = hit3.collider.gameObject;
                }
                else if (Physics.Raycast(currentTile.transform.position, -currentTile.transform.right, out RaycastHit hit4, 1) && hit4.collider.gameObject.CompareTag("Waypoints") && hit4.collider.gameObject != prevTile && hit4.collider.gameObject.GetComponent<CheckForDeadend>().amountOfTimesUsed < 2)
                {
                    Debug.Log("Hit Left");
                    prevTile = currentTile;
                    currentTile = hit4.collider.gameObject;
                }
                else
                {
                    Debug.Log("Fix Pls");
                    //prevPrevTile = prevTile;
                    prevTile = currentTile;
                    //currentTile = prevTile;
                }*/
            }
        else
        {
                if (!hitMovingWall)
                {
                    currentTile.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    aiPref.transform.position = Vector3.MoveTowards(aiPref.transform.position, new Vector3(currentTile.transform.position.x, aiPref.transform.position.y, currentTile.transform.position.z), patrolSpeed * Time.deltaTime);
                }
        }
        Debug.DrawLine(currentTile.transform.position, currentTile.transform.position + currentTile.transform.forward * 1, Color.red);
        Debug.DrawLine(currentTile.transform.position, currentTile.transform.position - currentTile.transform.forward * 1, Color.green);
        Debug.DrawLine(currentTile.transform.position, currentTile.transform.position + currentTile.transform.right * 1, Color.blue);
        Debug.DrawLine(currentTile.transform.position, currentTile.transform.position - currentTile.transform.right * 1, Color.black);

        }
    }

    public override void OnCollisionEnter(StateManager ai)
    {

    }

    private GameObject FindClosestTileToMoveTo()
    {
        listOfTiles.AddRange(GameObject.FindGameObjectsWithTag("Waypoints"));
        if (listOfTiles.Contains(prevTile))
        {
            listOfTiles.Remove(prevTile);
            Debug.Log("Removed");
        }
        currentTile = null;
        closestDist = Mathf.Infinity;
        foreach (var tile in listOfTiles)
        {
                float getDist = (aiPref.transform.position - tile.transform.position).sqrMagnitude;
                if (getDist < closestDist)
                {
                    closestTile = tile;
                    closestDist = getDist;
                }
        }
        prevTile = currentTile;

        return closestTile;
    }
}
