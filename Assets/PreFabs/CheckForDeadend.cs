using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForDeadend : MonoBehaviour
{
    public int amountOfTimesUsed = 0;
    private GameObject aiPref;
    private void Start()
    {
        amountOfTimesUsed = 0;
    }

    public void WaypointUsed()
    {
        amountOfTimesUsed++;
    }

    public void ResetUsage()
    {
        if(GetComponent<MeshRenderer>().material.color == Color.yellow)
        {
            print("Change Colour To Grey");
            GetComponent<MeshRenderer>().material.color = Color.gray;
        }
        amountOfTimesUsed = 0;
    }
}
