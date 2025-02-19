
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiStateManager : MonoBehaviour
{
    public enum ControlState { RandomWalk, HereAndBack, Stunned, Null, Death, Pause, Resume }

    public BehaviorFactory _behaviorFactory; // Behavior Factory ScriptableObject
    public AiBehaviorManager StateMachineMetaData; // StateMachine Meta Data ScriptableObject
    public ObjectOverlayTextRenderer ObjectOverlayRenderer;
    public GameEvent AiEvent;
    public GameObject Target;
    public Vector3 AiStartingPos;

    private AiBaseState _currentState;
    private bool _isStart = false;
    private AiBaseState _bankedSwitchState = null;
    private AiBaseState _spawnState;
    private bool _isPaused = false;

    private void Start()
    {
        // On start set a _CurrentState
        AiBaseState initialAiState = _behaviorFactory.GetBehavior(StateMachineMetaData.StartingStateName);
        _spawnState = _behaviorFactory.GetBehavior("ai_SpawnState");

        if (initialAiState != null)
        {
            _currentState = initialAiState;
            _currentState.EnterState(this);
        }
        else
        {
            Debug.LogError("AI Manager issue Initializing startingStateName was : " + StateMachineMetaData.StartingStateName);
        }

        ObjectOverlayRenderer.TopText = StateMachineMetaData.name;

        if (!_isStart && _bankedSwitchState != null)
        {
            _currentState = _bankedSwitchState;
            _currentState.EnterState(this);
        }

        if (StateMachineMetaData.UseSpawnState)
            PlaySpawnState();

        _isStart = true;
        AiStartingPos = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isPaused)
            return;

        _currentState.UpdateState(this); // _CurrentStates Update
    }

    // Passing Collision Enter to active state
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isPaused)
            return;

        _currentState.OnEnter(this, collision);
    }

    // Passing Collision Enter to active state
    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.LogError("Trigger entered!");
        _currentState.OnTriggerEnter(this, other);
    }

    // Passing Collision Stay to active state
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_isPaused)
            return;

        _currentState.OnOverlap(this, collision);
    }

    // Passing Collision Exit to active state
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_isPaused)
            return;

        _currentState.OnExit(this, collision);
    }

    public void SwitchState(string behaviorName)
    {
        AiBaseState behavior = _behaviorFactory.GetBehavior(behaviorName);

        if (_isPaused)
            return;


        // Check if the behaviorName is null or empty.
        if (string.IsNullOrEmpty(behaviorName))
        {
            Debug.Log("BehaviorName is null or empty.");
            return;
        }

        // Check if the behavior is registered before switching to it.
        if (_behaviorFactory == null)
        {
            Debug.LogError("BehaviorFactory is not assigned.");
            return;
        }

        // Check if the retrieved behavior is null.
        if (behavior == null)
        {
            Debug.LogError("Behavior not found: " + behaviorName);
            return;
        }

        if (!_isStart)
        {
            _bankedSwitchState = behavior;
        }

        _currentState = behavior;
        _currentState.EnterState(this);
            
    }

    public void SwitchState(ControlState state)
    {
        StartCoroutine(DelayedSwitch(state));

        /*switch (state)
        {
            case ControlState.RandomWalk:
                SwitchState("ai_RandomWalkState");
                break;

            case ControlState.HereAndBack:
                SwitchState("ai_HereAndBackState");
                break;

            case ControlState.Stunned:
                SwitchState("ai_StunnedState");
                break;

            case ControlState.Null:
                SwitchState("ai_NullState");
                break;

            case ControlState.Death:
                SwitchState("ai_DeathState");
                break;

            case ControlState.Pause:
                _isPaused = true;
                break;

            case ControlState.Resume:
                _isPaused = false;
                SwitchState(_currentState.ToString()); // restart current state
                break;
        }*/
    }

    // Launch a coroutine with a GameObject
    public void LaunchCoroutine(Func<GameObject, IEnumerator> coroutineFunction, GameObject obj)
    {
        StartCoroutine(coroutineFunction(obj));
    }

    public void PlaySpawnState()
    {
        if (_spawnState != null)
        {
            _currentState = _spawnState;
            _currentState.EnterState(this);
        }
    }

    public void PlayBankedState()
    {
        if (_bankedSwitchState != null)
        {
            _currentState = _bankedSwitchState;
            _currentState.EnterState(this);
            _bankedSwitchState = null;
            _spawnState = null;
        }
    }

    private IEnumerator DelayedSwitch(ControlState state)
    {
        yield return new WaitForSeconds(0.6f); // Adjust the delay time as needed

        switch (state)
        {
            case ControlState.RandomWalk:
                SwitchState("ai_RandomWalkState");
                break;

            case ControlState.HereAndBack:
                SwitchState("ai_HereAndBackState");
                break;

            case ControlState.Stunned:
                SwitchState("ai_StunnedState");
                break;

            case ControlState.Null:
                SwitchState("ai_NullState");
                break;

            case ControlState.Death:
                SwitchState("ai_DeathState");
                break;

            case ControlState.Pause:
                _isPaused = true;
                break;

            case ControlState.Resume:
                _isPaused = false;
                SwitchState(_currentState.ToString()); // restart the current state
                break;
        }
    }
}
