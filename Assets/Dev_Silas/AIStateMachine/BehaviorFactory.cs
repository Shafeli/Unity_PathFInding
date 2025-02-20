using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ai_BehaviorFactory", menuName = "Behaviors/ai_BehaviorFactory")]
public class BehaviorFactory : ScriptableObject
{
    private Dictionary<string, Type> _behaviorTypes = new Dictionary<string, Type>();
    // Register behavior
    public void RegisterBehavior(string identifier, Type behaviorType)
    {
        if (!_behaviorTypes.ContainsKey(identifier))
        {
            _behaviorTypes[identifier] = behaviorType;
        }
    }

    // Retrieve a behavior 
    public AiBaseState GetBehavior(string identifier)
    {
        if (string.IsNullOrEmpty(identifier))
        {
            // Handle the case where the identifier is null or empty
            Debug.Log("BehaviorFactory requesting identifier Null");
            return null;
        }

        if (_behaviorTypes.ContainsKey(identifier))
        {
            Type behaviorType = _behaviorTypes[identifier];
            if (typeof(AiBaseState).IsAssignableFrom(behaviorType))
            {
                // Create an instance of the specified behavior type
                AiBaseState behavior = Activator.CreateInstance(behaviorType) as AiBaseState;
                return behavior;
            }
        }

        // Return null if the behavior not found
        return null;
    }

    /*
     This dynamically discovers and registers script classes that inherit from AiBaseState during script enabling.
     When the script is disabled, it clears the registered types to release resources and avoid memory leaks.
    */
    public void OnEnable()
    {
        // Load all scripts of a type
        var scriptTypes = AppDomain.CurrentDomain.GetAssemblies()  // Get all loaded assemblies

            .SelectMany(assembly => assembly.GetTypes())  // Get all types in these assemblies

            .Where(type => typeof(AiBaseState).IsAssignableFrom(type) && !type.IsAbstract);  // Filter types based on inheritance

        foreach (var scriptType in scriptTypes)
        {
            RegisterBehavior(scriptType.Name, scriptType);  // Register each script with factory based on inspector
        }
    }

    public void OnDisable()
    {
        _behaviorTypes.Clear();  // Clear behavior
    }

    public string[] RegisteredBehaviors => _behaviorTypes.Keys.ToArray();
}

