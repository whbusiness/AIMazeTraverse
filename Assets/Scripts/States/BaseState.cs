using UnityEngine;
using UnityEngine.AI;

public abstract class BaseState
{
    public abstract void EnterState(StateManager ai);

    public abstract void UpdateState(StateManager ai);

    public abstract void OnCollisionEnter(StateManager ai);
}
