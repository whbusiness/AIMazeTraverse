using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AmountOfTimesLoaded
{
    public static int timesLoaded;
}

public class NextLevel : MonoBehaviour
{
    [SerializeField]
    private Button nextLevelBtn, quitBtn;
    // Start is called before the first frame update
    void Start()
    {
        StateManager.isFinished = false;
        BTREEAI.isFinished = false;
        AmountOfTimesLoaded.timesLoaded += 1;
        print("Level: " + AmountOfTimesLoaded.timesLoaded);
        if(AmountOfTimesLoaded.timesLoaded >= 3)
        {
            nextLevelBtn.gameObject.SetActive(false);
            quitBtn.gameObject.SetActive(true);
            Invoke(nameof(OnQuitBtn), 2);
        }
        else
        {
            MazeGenerator.maxCoins += 2;
            Invoke(nameof(OnNextLevel), 2);
        }
    }

    public void OnNextLevel()
    {
        MazeTile.movingWalls.Clear();
        SceneManager.LoadScene("SampleScene");
    }

    public void OnQuitBtn()
    {
        print("Quit");
        Application.Quit();
    }
}
