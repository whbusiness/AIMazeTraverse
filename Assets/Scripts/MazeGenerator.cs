using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeTile mazeTilePrefab;
    [SerializeField]
    private GameObject examplePref, coinPref;
    [SerializeField]
    private List<GameObject> aiPrefs = new();
    [SerializeField] private int mazeWidth;
    [SerializeField] private int mazeDepth;
    public static int maxCoins;
    private MazeTile[,] mazeGrid;
    [SerializeField]
    private List<Vector3> coinLocations = new();
    private List<GameObject> coins = new();
    private int spawnCoinInt;
    private GameObject spawnCoin;
    private List<GameObject> mazeObjectsGO = new();
    private List<GameObject> newMazeObjects = new();

    [SerializeField] private List<GameObject> tiles = new();
    [SerializeField]
    private float offsetX;

    GameObject findFirstTile;
    public void Start()
    {
        if(maxCoins <= 5)
        {
            maxCoins = 5;
        }
        mazeGrid = new MazeTile[mazeWidth, mazeDepth];

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeDepth; z++)
            {
                mazeGrid[x, z] = Instantiate(mazeTilePrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        tiles.AddRange(GameObject.FindGameObjectsWithTag("LeftWall"));
        int j = 0;
        foreach(var t  in tiles)
        {           

            GameObject go = Instantiate(examplePref, t.GetComponent<MeshRenderer>().bounds.center + new Vector3(offsetX, 0, 0), Quaternion.identity);
            go.name += j;
            j++;
        }
        Debug.Log("WaypointsMade");
        tiles.Clear();

        tiles.AddRange(GameObject.FindGameObjectsWithTag("Waypoints"));

        Instantiate(aiPrefs[0], new Vector3(mazeGrid[0, 0].transform.position.x, .5f, mazeGrid[0, 0].transform.position.z), Quaternion.identity);

        for(int i = 0; i < maxCoins; i++)
        {
            do
            {
                spawnCoinInt = SpawnCoins();
            } while(spawnCoinInt == 0);

            spawnCoin = Instantiate(coinPref, tiles[spawnCoinInt].transform.position, Quaternion.identity);
            coinLocations.Add(tiles[spawnCoinInt].transform.position);
            coins.Add(spawnCoin);
        }

        tiles.Clear();
        mazeObjectsGO.AddRange(GameObject.FindGameObjectsWithTag("Waypoints"));
        mazeObjectsGO.AddRange(GameObject.FindGameObjectsWithTag("MazeTile"));
        mazeObjectsGO.AddRange(GameObject.FindGameObjectsWithTag("Coin"));
        mazeObjectsGO.AddRange(GameObject.FindGameObjectsWithTag("MovingWall"));

        

        GenerateMaze(null, mazeGrid[0, 0]);

        for (int i =0; i<mazeObjectsGO.Count; i++)
        {
            GameObject go = Instantiate(mazeObjectsGO[i], mazeObjectsGO[i].transform.position + new Vector3(22,0,0), Quaternion.identity);
            newMazeObjects.Add(go);
            if (findFirstTile == null && newMazeObjects[i].CompareTag("MazeTile"))
            {
                findFirstTile = newMazeObjects[i];
            }
            if (newMazeObjects[i].CompareTag("Coin"))
            {
                newMazeObjects[i].tag = "FSMCoins";
            }
        }
        Instantiate(aiPrefs[1], new Vector3(findFirstTile.transform.position.x, .5f, findFirstTile.transform.position.z), Quaternion.identity);
    }

    private void GenerateMaze(MazeTile previousCell, MazeTile currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        while (TryGetNextUnvisitedCell(currentCell, out MazeTile nextCell))
        {
            GenerateMaze(currentCell, nextCell);
        }
    }

    private bool TryGetNextUnvisitedCell(MazeTile currentCell, out MazeTile nextCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);
        nextCell = unvisitedCells.Count > 0 ? unvisitedCells[Random.Range(0, unvisitedCells.Count)] : null;
        return nextCell != null;
    }

    private List<MazeTile> GetUnvisitedCells(MazeTile currentCell)
    {
        int x = Mathf.RoundToInt(currentCell.transform.position.x);
        int z = Mathf.RoundToInt(currentCell.transform.position.z);

        List<MazeTile> unvisitedCells = new List<MazeTile>();

        foreach (var offset in new[] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) })
        {
            int newX = x + Mathf.RoundToInt(offset.x);
            int newZ = z + Mathf.RoundToInt(offset.y);

            if (IsWithinBounds(newX, newZ) && !mazeGrid[newX, newZ].IsVisited)
            {
                unvisitedCells.Add(mazeGrid[newX, newZ]);
            }
        }

        return unvisitedCells;
    }

    private bool IsWithinBounds(int x, int z)
    {
        return x >= 0 && x < mazeWidth && z >= 0 && z < mazeDepth;
    }

    private void ClearWalls(MazeTile previousCell, MazeTile currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        Vector3 positionDiff = currentCell.transform.position - previousCell.transform.position;

        if (positionDiff.x > 0)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
        }
        else if (positionDiff.x < 0)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
        }
        else if (positionDiff.z > 0)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
        }
        else if (positionDiff.z < 0)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
        }
    }

    int SpawnCoins()
    {
        float closestDist = Mathf.Infinity;
        var randomInt = Random.Range(0, tiles.Count);
        if (coinLocations.Count > 0)
        {
            foreach (var point in coinLocations)
            {
                var dist = (point - tiles[randomInt].transform.position).sqrMagnitude;
                if (dist < closestDist)
                {
                    closestDist = dist;
                    print("close: " + closestDist);
                }
            }
            if (closestDist > 5)
            {
                return randomInt;
            }
            else if (closestDist <= 5)
            {
                return 0;
            }
        }
        return randomInt;
    }

    public void ClearMaze()
    {
        aiPrefs.Remove(aiPrefs[0]);
        for(int i = 0; i< maxCoins; i++)
        {
            Instantiate(coinPref, coinLocations[i], Quaternion.identity);
        }
        List<GameObject> waypoints = new();
        waypoints.AddRange(GameObject.FindGameObjectsWithTag("Waypoints"));
        for(int i = 0; i < waypoints.Count; i++)
        {
            waypoints[i].GetComponent<CheckForDeadend>().ResetUsage();
        }
        Instantiate(aiPrefs[0], new Vector3(mazeGrid[0, 0].transform.position.x, .5f, mazeGrid[0, 0].transform.position.z), Quaternion.identity);
    }
}
