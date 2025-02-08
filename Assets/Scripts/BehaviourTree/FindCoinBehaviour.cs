using bTree;
using UnityEngine;

public class FindCoinBehaviour : Node
{
    private Transform aiTransform;
    public GameObject coinFound;
    public FindCoinBehaviour(Transform _aiTransform)
    {
        Debug.Log("FindCoinB");
        aiTransform = _aiTransform;
    }
    public override State Evaluate()
    {
        object t = GetData("coin");
        if(t == null)
        {
            if (Physics.Raycast(aiTransform.position, aiTransform.forward, out RaycastHit coin1, Mathf.Infinity) && coin1.collider.gameObject.CompareTag("Coin"))
            {
                Debug.Log("Hit Coin");
                coinFound = coin1.collider.gameObject;
            }
            else if (Physics.Raycast(aiTransform.position, aiTransform.right, out RaycastHit coin2, Mathf.Infinity) && coin2.collider.gameObject.CompareTag("Coin"))
            {
                Debug.Log("Hit Coin");
                coinFound = coin2.collider.gameObject;
            }
            else if (Physics.Raycast(aiTransform.position, -aiTransform.forward, out RaycastHit coin3, Mathf.Infinity) && coin3.collider.gameObject.CompareTag("Coin"))
            {
                Debug.Log("Hit Coin");
                coinFound = coin3.collider.gameObject;
            }
            else if (Physics.Raycast(aiTransform.position, -aiTransform.right, out RaycastHit coin4, Mathf.Infinity) && coin4.collider.gameObject.CompareTag("Coin"))
            {
                Debug.Log("Hit Coin");
                coinFound = coin4.collider.gameObject;
            }

            if(coinFound != null)
            {
                BTREEAI.stateTxt.SetText("Behaviour: FindCoin");
                parent.parent.SetData("coin", coinFound.transform);
                state = State.SUCCESS;
                return state;
            }
            state = State.FAILURE;
            return state;
        }
        state = State.SUCCESS;
        return state;
    }

}
