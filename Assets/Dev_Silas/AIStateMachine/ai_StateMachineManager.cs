using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ai_StateMachineManager : MonoBehaviour
{
    public enum StartingState { Alive, Dead }
    public enum StateMachineType {Physics, Animation }

    public StartingState AiStartingState = StartingState.Alive;

    [System.Serializable]
    public class AiCommunicationManagerDefinition
    {
        public StateMachineType ManagingType; // What is this reference 
        public AiStateManager AiStateMachine; // Reference to the AI state machine
    }

    public List<AiCommunicationManagerDefinition> CommunicationDefinitions = new List<AiCommunicationManagerDefinition>();

    // Start is called before the first frame update
    void Start()
    {

        if (AiStartingState == StartingState.Dead)
            KillThis();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KillThis()
    {
        AiCommunicationManagerDefinition definition = CommunicationDefinitions.Find
            (
                def => def.ManagingType == StateMachineType.Animation
                );

        if (definition != null)
        {
            definition.AiStateMachine.GetComponent<SpriteRenderer>().enabled = false;
        }

        foreach (var sateMachine in CommunicationDefinitions)
        {
            sateMachine.AiStateMachine.SwitchState("ai_DeathState");
        }
    }

    private void HandleDeath()
    {
        foreach (var sateMachine in CommunicationDefinitions)
        {
            sateMachine.AiStateMachine.SwitchState("ai_DeathState");
        }
    }
}
