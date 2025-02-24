using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ai_StateMachine", menuName = "Behaviors/ai_StateMachine")]
public class AiBehaviorManager : ScriptableObject
{
    [System.Serializable]
    public class AiDefinition
    {
        public ScriptableObject AiConfig; // Reference to the AI scriptable object
        public string AssociatedStateName; // Name of the associated state
    }

    // Backing fields
    /////////////////////////////////////////////////////////////////////
    [SerializeField] private string startingStateConfigData;
    [SerializeField] private string startingStateName;
    [SerializeField] private BehaviorFactory behaviorFactory;  // Reference to Factory
    [SerializeField] private List<AiDefinition> aiDefinitions = new List<AiDefinition>();

    // Readonly properties
    /////////////////////////////////////////////////////////////////////
    public string StartingStateConfigData => startingStateConfigData;
    public string StartingStateName => startingStateName;
    public BehaviorFactory BehaviorFactory => behaviorFactory;
    public IReadOnlyList<AiDefinition> AiDefinitions => aiDefinitions; // Stop external moding

    public ScriptableObject GetConfigByStateName(string associatedStateName)
    {
        AiDefinition aiDefinition = aiDefinitions.Find(def => def.AssociatedStateName == associatedStateName);

        if (aiDefinition != null)
        {
            return aiDefinition.AiConfig;
        }

        // Handle error
        Debug.Log("Config with name " + associatedStateName + " not found.");
        return null;
    }
}