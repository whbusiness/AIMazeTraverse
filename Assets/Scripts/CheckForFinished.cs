using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckForFinished : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(StateManager.isFinished && BTREEAI.isFinished)
        {
            SceneManager.LoadScene("EndScene");
        }
    }
}
