using UnityEngine;

[CreateAssetMenu(fileName = "ai_PathingConfig", menuName = "Behaviors/Config/PathingConfig")]
public class ai_PathingConfig : ScriptableObject
{
    public float Speed = 10f;
    public float ArrivalThreshold = 0.1f;
}
