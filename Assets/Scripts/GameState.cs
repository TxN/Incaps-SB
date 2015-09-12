using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour 
{
    public static GameState Instance { get; private set; }

    public delegate void ChangeStateAction();
    public static event ChangeStateAction OnStateChanged;

    public delegate void ChangeCircuitEditAction();
    public static event ChangeCircuitEditAction OnCircuitSubstateChanged;


    public enum State
    {
        MainMenu,
        PlayMode,
        BuildMode,
        CircuitMode,
        EscMenu,
        PauseMode
    }

    public enum CircuitEditSubstate
    {
        GatePlacing,
        Wiring,
        Removing,
        EditOptions

    }

    public State CurrentGameState  { get; private set; }
    public CircuitEditSubstate CurrentCircuitSubstate { get; private set; }

    void Awake()
    {
       
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //remove after testing
        CurrentGameState = State.CircuitMode;
        CurrentCircuitSubstate = CircuitEditSubstate.Wiring;
    }

    public void SetGameState(State state) 
    {
        CurrentGameState = state;

        if (OnStateChanged != null)
        {
            OnStateChanged();
        }
    
    }

    public void SetCircuitEditSubstate(CircuitEditSubstate state) 
    {
        CurrentCircuitSubstate = state; 

        if (OnCircuitSubstateChanged != null)
        {
            OnCircuitSubstateChanged();
        }
    }

}
