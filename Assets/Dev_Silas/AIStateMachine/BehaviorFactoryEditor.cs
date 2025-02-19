using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BehaviorFactory))]
public class BehaviorFactoryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BehaviorFactory behaviorFactory = (BehaviorFactory)target;

        GUILayout.Label("Registered Behaviors:");

        foreach (string behaviorName in behaviorFactory.RegisteredBehaviors)
        {
            EditorGUILayout.LabelField(behaviorName);
        }
    }
}