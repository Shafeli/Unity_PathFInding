using System.Collections.Generic;
using UnityEngine;

public class ai_PathingBehavior : AiBaseState
{
    private List<Vector3> _path;
    private int _currentIndex;
    private float _speed = 2f;
    private float _arrivalThreshold = 0.1f; // Stop when close
    private SceneManager sceneManager;

    public ai_PathingBehavior()
    {
        _currentIndex = 0;
        _speed = 2f;
        _path = new List<Vector3>();
    }

    public override void EnterState(AiStateManager aiStateManager)
    {
        // Cast to the Config Type before requesting
        ai_PathingConfig configObject = (ai_PathingConfig)aiStateManager.StateMachineMetaData.GetConfigByStateName(this.ToString());

        // If Config is found set it
        if (configObject)
        {
            // Based on the config
            _speed = configObject.Speed;
            _arrivalThreshold = configObject.ArrivalThreshold;

        }

        sceneManager = aiStateManager.ActiveSceneManager;

    }

    public override void UpdateState(AiStateManager aiStateManager)
    {
        BrokkrVector2.Vector2 selfPosition = BrokkrVector2.Vector2.FromUnityVector(aiStateManager.transform.position);

        if (Input.GetMouseButtonDown(0))
        {
            aiStateManager.StopMovement();
            _currentIndex = 0;

            int x, y;
            Vector3 mousePosition = GeneralUtility.MouseUtility.GetWorldPosition();
            sceneManager.GetPathFindingGrid().XY(mousePosition, out x, out y);

            sceneManager.GetPathFindingGrid().XY(BrokkrVector2.Vector2.ToUnityVector(selfPosition, 0),out var selfX, out var selfY );

            _path = sceneManager.FindPath(sceneManager.GetPathFindingGrid().WorldPosition(selfX, selfY), mousePosition);
        }


        if (_path.Count == 0 || _currentIndex >= _path.Count)
        {   
            _path.Clear();
            _currentIndex = 0;
            aiStateManager.StopMovement();
            return; // No waypoints or finished
        }


        for (int i = _currentIndex; i < _path.Count - 1; i++) // Loop up to right before last cell
        {
            Vector3 start = _path[i];
            Vector3 end = _path[i + 1];
            Debug.DrawLine(start, end, Color.red, 0.5f);
        }

        
        BrokkrVector2.Vector2 target = BrokkrVector2.Vector2.FromUnityVector(_path[_currentIndex]);
        BrokkrVector2.Vector2 direction = (target - selfPosition).Normalize();
        BrokkrVector2.Vector2 velocity = direction * _speed;

        // Move towards target
        KinematicMove(aiStateManager, velocity, Time.deltaTime);

        // Check if AI reached the waypoint
        if (selfPosition.DistanceTo(target) < _arrivalThreshold)
        {
            _currentIndex++; // Move to the next point
        }
    }

    public override void OnEnter(AiStateManager aiStateManager, Collision2D collision) { }
    public override void OnExit(AiStateManager aiStateManager, Collision2D collision) { }
    public override void OnOverlap(AiStateManager aiStateManager, Collision2D collision) { }
}


