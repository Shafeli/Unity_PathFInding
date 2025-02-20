using UnityEngine;

public class Kinematic : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _maxAcceleration = 2f;
    [SerializeField] private float _maxAngularVelocity = 180f; // Degrees per second
    [SerializeField] private float _maxAngularAcceleration = 90f;

    private Transform _location;
    private BrokkrVector2.Vector2 _velocity;
    private BrokkrVector2.Vector2 _acceleration;
    private float _angularVelocity;
    private float _angularAcceleration;

    public Transform Location => _location; // Read-only property
    public BrokkrVector2.Vector2 Velocity => _velocity; // Read-only property
    public BrokkrVector2.Vector2 Acceleration => _acceleration; // Read-only property
    public float AngularVelocity => _angularVelocity; // Read-only property
    public float AngularAcceleration => _angularAcceleration; // Read-only property
    public float MaxSpeed => _maxSpeed;

    void Start()
    {
        _location = gameObject.transform;
        _velocity = new BrokkrVector2.Vector2(0, 0);
        _acceleration = new BrokkrVector2.Vector2(0, 0);
        _angularVelocity = 0f;
        _angularAcceleration = 0f;
    }

    public void UpdateKinematics(float deltaTime)
    {
        // Apply acceleration
        _velocity += _acceleration * deltaTime;

        // Clamp velocity
        if (_velocity.Length() > _maxSpeed)
        {
            _velocity = _velocity.Normalize() * _maxSpeed;
        }

        // Apply position change (using Vector3 conversion for Unity)
        _location.position += new Vector3(_velocity.X, _velocity.Y, 0) * deltaTime;

        // Apply angular acceleration
        _angularVelocity += _angularAcceleration * deltaTime;
        _angularVelocity = Mathf.Clamp(_angularVelocity, -_maxAngularVelocity, _maxAngularVelocity);

        // Apply rotation
        _location.Rotate(0, 0, _angularVelocity * deltaTime);
    }

    public void SetLinearAcceleration(BrokkrVector2.Vector2 acceleration)
    {
        _acceleration = new BrokkrVector2.Vector2(
            Mathf.Clamp(acceleration.X, -_maxAcceleration, _maxAcceleration),
            Mathf.Clamp(acceleration.Y, -_maxAcceleration, _maxAcceleration)
        );
    }

    public void SetAngularAcceleration(float angularAcceleration)
    {
        _angularAcceleration = Mathf.Clamp(angularAcceleration, -_maxAngularAcceleration, _maxAngularAcceleration);
    }

    public void Stop()
    {
        _velocity = new BrokkrVector2.Vector2(0, 0);
        _angularVelocity = 0f;
        _acceleration = new BrokkrVector2.Vector2(0, 0);
        _angularAcceleration = 0f;
    }
}
