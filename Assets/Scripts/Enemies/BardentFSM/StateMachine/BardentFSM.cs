using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BardentFSM
{
    public BardentState CurrentState { get; private set; }

    public void Initialize(BardentState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(BardentState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
