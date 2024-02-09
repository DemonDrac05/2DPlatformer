using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState CurrentPlayerState;

    public void Initialize(PlayerState startingState)
    {
        CurrentPlayerState = startingState;
        CurrentPlayerState.EnterState();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentPlayerState.ExitState();
        CurrentPlayerState = newState;
        CurrentPlayerState.EnterState();
    }
}
