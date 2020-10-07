using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalObjectManager : MonoBehaviour
{
    protected State currentState;
    protected State nextState;

    protected virtual void Update()
    {
        currentState?.Execute();
    }

    public virtual void ChangeState(State newState = null)
    {
        //newState==null permet de traiter les Idles des différentes config' : IA ou joueur
        if (currentState != null)
            currentState.Exit();
        if (newState == null)
        {
            if (nextState != null)
                currentState = nextState;
            else
                return;
        }
        else
        {
            if (nextState != null)
                currentState = nextState;
            else
                currentState = newState;
        }
        nextState = null;
        currentState.Enter();
    }

    public State CurrentState { get => currentState; }
}
