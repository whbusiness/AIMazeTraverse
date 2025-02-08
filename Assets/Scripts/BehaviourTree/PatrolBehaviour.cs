using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using bTree;
using UnityEngine.Rendering.Universal;

public class Directions
{
    public string right = "Right";
    public string left = "Left";
    public string forward = "Forward";
    public string back = "Backward";
}

public class PatrolBehaviour : Node
{
    private Transform aiTransform;
    private GameObject currentTile, closestTile, prevTile, leastTileUsed;
    private List<GameObject> listOfTiles = new(), allTilesNearby = new();
    private float closestDist = Mathf.Infinity, lowestAmountOfTimesUsed = Mathf.Infinity, patrolSpeed = 10;
    public GameObject coinFound;
    Directions dir = new Directions();
    private string currentDir = "";
    bool hitMovingWall = false;
    private GameObject stats;

    public PatrolBehaviour(Transform _aiTransform, Transform tileTransform)
    {
        currentTile = tileTransform.gameObject;
        aiTransform = _aiTransform;
    }
    public override State Evaluate()
    {
        if(currentTile == null)
        {
            currentTile = BTREEAI.FindClosestTileToMoveTo();
        }
        if (currentTile != null)
        {
            BTREEAI.stateTxt.SetText("Current Behaviour: Patrol");
            if (Vector3.Distance(aiTransform.position, currentTile.transform.position) <= .3f)
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
                else
                {
                    hitMovingWall = false;
                    stats = GameObject.FindGameObjectWithTag("Stats");
                    currentTile.GetComponent<CheckForDeadend>().WaypointUsed();
                    BTREEAI.waypointsVisited++;
                    stats.GetComponent<Stats>().btreeVariables.amountOfWaypointsVisited++;
                    BTREEAI.waypointTxt.SetText("Waypoint: " + BTREEAI.waypointsVisited);
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

                }







                foreach (var tile in allTilesNearby)
                {
                    if (tile.GetComponent<CheckForDeadend>().amountOfTimesUsed < lowestAmountOfTimesUsed)
                    {
                        leastTileUsed = tile;
                        lowestAmountOfTimesUsed = tile.GetComponent<CheckForDeadend>().amountOfTimesUsed;
                    }
                }

                BTREEAI.dirTxt.SetText("Direction: " + currentDir);
                prevTile = currentTile;
                currentTile = leastTileUsed;

                allTilesNearby.Clear();

            }
            else
            {
                if (!hitMovingWall)
                {
                    currentTile.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    aiTransform.position = Vector3.MoveTowards(aiTransform.position, new Vector3(currentTile.transform.position.x, aiTransform.position.y, currentTile.transform.position.z), BTREEAI.speed * Time.deltaTime);
                }
            }
            Debug.DrawLine(currentTile.transform.position, currentTile.transform.position + currentTile.transform.forward * 1, Color.red);
            Debug.DrawLine(currentTile.transform.position, currentTile.transform.position - currentTile.transform.forward * 1, Color.green);
            Debug.DrawLine(currentTile.transform.position, currentTile.transform.position + currentTile.transform.right * 1, Color.blue);
            Debug.DrawLine(currentTile.transform.position, currentTile.transform.position - currentTile.transform.right * 1, Color.black);

        }
        state = State.RUNNING;
        return state;
    }
}
