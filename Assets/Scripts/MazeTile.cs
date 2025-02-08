using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeTile : MonoBehaviour
{
    [SerializeField] private GameObject _leftWall;

    [SerializeField] private GameObject _rightWall;

    [SerializeField] private GameObject _frontWall;

    [SerializeField] private GameObject _backWall;

    [SerializeField] private GameObject _unvisitedBlock;

    public static List<GameObject> movingWalls = new();
    bool dontSpawn = false;

    private void Start()
    {
        dontSpawn = false;
    }

    public bool IsVisited { get; private set; }

    public void Visit()
    {
        IsVisited = true;
        _unvisitedBlock.SetActive(false);
    }

    public void ClearLeftWall()
    {
        var rand = Random.Range(0, 100);
        if(rand >= 70)
        {
            foreach(var item in movingWalls)
            {
                if(Vector3.Distance(_leftWall.transform.position, item.transform.position) <= 7)
                {
                    dontSpawn = true;
                    print("Too Close To Another Moving Wall");
                }
            }
            if(!dontSpawn)
            {
                _leftWall.AddComponent<MovingWall>();
                _leftWall.GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
                _leftWall.tag = "MovingWall";
                movingWalls.Add(_leftWall);
            }
            else
            {
                print("Not Spawning This Moving Wall");
                _leftWall.SetActive(false);
            }
        }
        else
        {
            _leftWall.SetActive(false);
        }
        //
    }

    public void ClearRightWall()
    {
        _rightWall.SetActive(false);
    }

    public void ClearFrontWall()
    {
        _frontWall.SetActive(false);
    }

    public void ClearBackWall()
    {
        _backWall.SetActive(false);
    }
}

