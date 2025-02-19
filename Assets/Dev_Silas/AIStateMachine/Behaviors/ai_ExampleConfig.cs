using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ai_ExampleConfig", menuName = "Behaviors/ai_ExampleConfig")]
public class ai_ExampleConfig : ScriptableObject
{
    // Based on the config
    public float Range = 5.0f;
    public float Speed = 2.0f;
    public float StateTimerMax = 4.0f;
}
