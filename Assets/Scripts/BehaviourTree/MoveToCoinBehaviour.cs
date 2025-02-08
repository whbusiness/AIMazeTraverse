using bTree;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToCoinBehaviour : Node
{
    public Transform aiTransform, coinToMoveTo;
    private GameObject stats;
    public MoveToCoinBehaviour(Transform _aiTransform)
    {
        Debug.Log("CoinB");
        aiTransform = _aiTransform;
    }

    public override State Evaluate()
    {
        coinToMoveTo = (Transform)GetData("coin");
        BTREEAI.stateTxt.SetText("Behaviour: MoveToCoin");
        if (Vector3.Distance(aiTransform.position, coinToMoveTo.position) >=.3f)
        {
            aiTransform.position = Vector3.MoveTowards(aiTransform.position, new Vector3(coinToMoveTo.transform.position.x, aiTransform.position.y, coinToMoveTo.transform.position.z), BTREEAI.speed * Time.deltaTime);
        }
        else
        {
            stats = GameObject.FindGameObjectWithTag("Stats");
            coinToMoveTo.GetComponent<CoinRotation>().DestroyCoin();
            stats.GetComponent<Stats>().btreeVariables.amountOfCoins++;
            BTREEAI.coinsTxt.SetText("Coins: " + stats.GetComponent<Stats>().btreeVariables.amountOfCoins);
            ClearData("coin");
            int amountOfCoinsLeft = GameObject.FindGameObjectsWithTag("Coin").Length; 
            if(amountOfCoinsLeft <= 1)
            {
                Debug.Log("No Coins Left");
                stats.GetComponent<Stats>().aiType = Stats.WhichAI.BTREE;
                stats.GetComponent<Stats>().btreeVariables.BTREE_overallTime += DateTime.Now - BTREEAI.startTime;
                BTREEAI.isFinished = true;
                var aiType = GameObject.FindGameObjectWithTag("BTREEAI");
                aiType.GetComponent<DestroyAI>().DestroyGameObject();
                //SceneManager.LoadScene("EndScene");
            }
            //coinToMoveTo.gameObject.SetActive(false);
        }
        state = State.RUNNING;
        return state;
    }
}
