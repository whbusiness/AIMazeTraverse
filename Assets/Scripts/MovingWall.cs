using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    Vector3 ogPos, newPos;
    float speed;
    bool moveToNewPos = true, moveBack = false;
    // Start is called before the first frame update
    void Start()
    {
        ogPos = transform.position;
        speed = Random.value;
        newPos = new Vector3(ogPos.x, ogPos.y, ogPos.z - 1.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, newPos) >= .01 && moveToNewPos)
        {
            transform.position = Vector3.Lerp(transform.position, newPos, 3 * Time.deltaTime);
        }
        else
        {
            moveToNewPos = false;
            moveBack = true;
        }

        if(Vector3.Distance(transform.position, ogPos) >= .01 && moveBack)
        {
            transform.position = Vector3.Lerp(transform.position, ogPos, 3 * Time.deltaTime);
        }
        else
        {
            moveBack = false;
            moveToNewPos=true;
        }
    }
}
