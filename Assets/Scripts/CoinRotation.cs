using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    [SerializeField]
    private float speed;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime , 0);
    }

    public void DestroyCoin()
    {
        Destroy(gameObject);
    }
}
