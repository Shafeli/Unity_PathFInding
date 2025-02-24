using System;
using UnityEngine;
using System.Collections.Generic;


public class BrokkrVector2
{
    public struct Vector2
    {
        // Constant avoid division by zero
        private const float KTolerance = 1e-6f;

        private float _x;
        private float _y;

        // Constructor
        public Vector2(float x, float y)
        {
            _x = x;
            _y = y;
        }

        // Properties for X and Y
        public float X => _x;
        public float Y => _y;

        // Prime ops
        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a._x + b._x, a._y + b._y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a._x - b._x, a._y - b._y);
        public static Vector2 operator *(Vector2 v, float scalar) => new Vector2(v._x * scalar, v._y * scalar);
        public static Vector2 operator /(Vector2 v, float scalar)
        {
            if (Math.Abs(scalar) < KTolerance) throw new DivideByZeroException("Prob shouldn't divide by zero.");
            return new Vector2(v._x / scalar, v._y / scalar);
        }

        public static Vector2 operator -(Vector2 v) => new Vector2(-v._x, -v._y);
        public static bool operator ==(Vector2 a, Vector2 b) => (a._x.Equals(b._x) && a._y.Equals(b._y));
        public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);

        public override bool Equals(object obj)
        {
            if (obj is Vector2 other)
                return this == other;
            return false;
        }

        public float Length() => (float)Math.Sqrt(_x * _x + _y * _y);
        public float LengthSquared() => (_x * _x + _y * _y);

        public Vector2 Normalize()
        {
            float length = Length();
            if (length > KTolerance)
            {
                return new Vector2(_x / length, _y / length);
            }
            return ZeroVector();
        }

        public static Vector2 ZeroVector() => new Vector2(0, 0);

        public float Dot(Vector2 other) => (_x * other._x) + (_y * other._y);

        public bool IsZero(float tolerance = KTolerance) => Math.Abs(_x) < tolerance && Math.Abs(_y) < tolerance;

        public static List<Vector2> LinearSpace(Vector2 start, Vector2 end, int numSteps)
        {
            var result = new List<Vector2>();
            if (numSteps <= 1) return new List<Vector2> { start };

            Vector2 step = (end - start) / (numSteps - 1);
            for (int i = 0; i < numSteps; i++)
            {
                result.Add(start + step * i);
            }

            return result;
        }

        public static Vector2 Lerp(Vector2 start, Vector2 end, float t) => start + (end - start) * t;

        public static Vector2 MaxVector() => new Vector2(float.MaxValue, float.MaxValue);

        public float DistanceTo(Vector2 other) => (this - other).Length();

        //convert Unity Vector2 to BrokkrVector2.Vector2
        public static Vector2 FromUnityVector(UnityEngine.Vector2 unityVector)
        {
            return new Vector2(unityVector.x, unityVector.y);
        }

        //convert BrokkrVector2.Vector2 to Unity Vector2
        public static UnityEngine.Vector2 ToUnityVector(Vector2 brokkrVector)
        {
            return new UnityEngine.Vector2(brokkrVector.X, brokkrVector.Y);
        }

        // Convert Unity Vector3 to BrokkrVector2.Vector2 Z = 0
        public static Vector2 FromUnityVector(UnityEngine.Vector3 unityVector)
        {
            return new Vector2(unityVector.x, unityVector.y);
        }

        // Convert BrokkrVector2.Vector2 to Unity Vector3 Z = 0 defaulted
        public static UnityEngine.Vector3 ToUnityVector(Vector2 brokkrVector, float z = 0f)
        {
            return new UnityEngine.Vector3(brokkrVector.X, brokkrVector.Y, z);
        }
    }
}

