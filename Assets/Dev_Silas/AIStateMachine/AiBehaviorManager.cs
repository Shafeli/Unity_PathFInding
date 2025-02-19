using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AiFactory", menuName = "AiStateMachine/AiBehaviorManager")]
public class AiBehaviorManager : ScriptableObject
{
    [System.Serializable]
    public class AiDefinition
    {
        public ScriptableObject AiConfig; // Reference to the AI scriptable object
        public string AssociatedStateName; // Name of the associated state
    }

    [SerializeField] public string StartingStateConfigData; // Name or identifier
    [SerializeField] public string StartingStateName;
    [SerializeField] public bool UseSpawnState = false;

    public BehaviorFactory behaviorFactory; // Reference to Factory
    public List<AiDefinition> aiDefinitions = new List<AiDefinition>();

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