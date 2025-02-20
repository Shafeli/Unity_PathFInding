using System;
using UnityEngine;

public class AiStateManager : MonoBehaviour
{
    // Serialized backing fields
    [SerializeField] private BehaviorFactory _behaviors;
    [SerializeField] private AiBehaviorManager _stateMachineMetaDatas;
    [SerializeField] private ObjectOverlayTextRenderer _objectOverlayRenderer;

    [SerializeField] private Kinematic _kinematic;
    [SerializeField] private float _rotationSpeed = 200f;

    // Readonly properties
    public BehaviorFactory BehaviorFactory => _behaviors;
    public AiBehaviorManager StateMachineMetaData => _stateMachineMetaDatas;
    public float MaxSpeed => _kinematic.MaxSpeed;
    public float RotationSpeed => _rotationSpeed;

    // Private fields
    private AiBaseState _currentState;
    private AiBaseState _bankedSwitchState;
    private bool _isStarted = false;
    private bool _isPaused = false;

    private BrokkrVector2.Vector2 _velocity;
    private float _currentRotation;

    private void Start()
    {
        InitializeStateMachine();
    }

    private void Update()
    {
        if (_currentState == null || _isPaused)
            return;

        _currentState.UpdateState(this);
        ApplyMovement();

        if (_objectOverlayRenderer != null)
            _objectOverlayRenderer.BottomText = _currentState.ToString();
    }

    private void InitializeStateMachine()
    {
        if (BehaviorFactory == null)
        {
            Debug.LogError("BehaviorFactory is missing.");
            return;
        }

        _currentState = BehaviorFactory.GetBehavior(StateMachineMetaData?.StartingStateName);
        if (_currentState == null)
        {
            Debug.Log($"Failed to initialize AI state: {StateMachineMetaData?.StartingStateName}");
            return;
        }

        _currentState.EnterState(this);
        _objectOverlayRenderer.BottomText = _currentState.ToString();

        if (!_isStarted && _bankedSwitchState != null)
        {
            _currentState = _bankedSwitchState;
            _currentState.EnterState(this);
        }

        _isStarted = true;
    }

    public void SetSteering(Vector2 direction, float speedFactor = 1.0f)
    {
        BrokkrVector2.Vector2 directionConvert = BrokkrVector2.Vector2.FromUnityVector(direction);

        BrokkrVector2.Vector2 desiredVelocity = directionConvert.Normalize() * MaxSpeed * Mathf.Clamp01(speedFactor);

        _velocity = BrokkrVector2.Vector2.Lerp(_velocity, desiredVelocity, 0.1f); // Smooth acceleration
    }

    private void ApplyMovement()
    {
        transform.position += (Vector3)BrokkrVector2.Vector2.ToUnityVector(_velocity) * Time.deltaTime;

        if (_velocity.LengthSquared() > 0.01f)
        {
            float targetAngle = Mathf.Atan2(_velocity.Y, _velocity.X) * Mathf.Rad2Deg;
            _currentRotation = Mathf.LerpAngle(_currentRotation, targetAngle, _rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, _currentRotation);
        }
    }

    public void SwitchState(string behaviorName)
    {
        if (_isPaused || string.IsNullOrEmpty(behaviorName))
            return;

        AiBaseState newState = BehaviorFactory?.GetBehavior(behaviorName);
        if (newState == null)
        {
            Debug.LogError($"Behavior not found: {behaviorName}");
            return;
        }

        if (!_isStarted)
        {
            _bankedSwitchState = newState;
            return;
        }

        _currentState = newState;
        _currentState.EnterState(this);
    }

    // Collision handling
    private void OnCollisionEnter2D(Collision2D collision) => _currentState?.OnEnter(this, collision);
    private void OnTriggerEnter2D(Collider2D other) => _currentState?.OnTriggerEnter(this, other);
    private void OnCollisionStay2D(Collision2D collision) => _currentState?.OnOverlap(this, collision);
    private void OnCollisionExit2D(Collision2D collision) => _currentState?.OnExit(this, collision);
}
