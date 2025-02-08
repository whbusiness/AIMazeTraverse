using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAI : MonoBehaviour
{
    public void DestroyGameObject()
    {
        print("DESTROYAI");
        Destroy(gameObject);
    }
}
