using System;
using UnityEngine;
using System.Collections.Generic;


public class BrokkrVector2
{
    public struct Vector2
    {
        // Constant avoid division by zero
        private const float KTolerance = 1e-6f;

        public float m_x;
        public float m_y;

        // Constructor
        public Vector2(float x, float y)
        {
            m_x = x;
            m_y = y;
        }

        // Properties for X and Y
        public float X => m_x;
        public float Y => m_y;

        // Prime operators
        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.m_x + b.m_x, a.m_y + b.m_y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.m_x - b.m_x, a.m_y - b.m_y);
        public static Vector2 operator *(Vector2 v, float scalar) => new Vector2(v.m_x * scalar, v.m_y * scalar);
        public static Vector2 operator /(Vector2 v, float scalar)
        {
            if (Math.Abs(scalar) < KTolerance) throw new DivideByZeroException("Prob shouldn't divide by zero.");
            return new Vector2(v.m_x / scalar, v.m_y / scalar);
        }

        public static Vector2 operator -(Vector2 v) => new Vector2(-v.m_x, -v.m_y);
        public static bool operator ==(Vector2 a, Vector2 b) => (a.m_x == b.m_x && a.m_y == b.m_y);
        public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);

        public override bool Equals(object obj)
        {
            if (obj is Vector2 other)
                return this == other;
            return false;
        }

        public override int GetHashCode() => HashCode.Combine(m_x, m_y);

        // Math
        public static Vector2 FromPolar(float radius, float angleRadians)
        {
            float x = radius * (float)Math.Cos(angleRadians);
            float y = radius * (float)Math.Sin(angleRadians);
            return new Vector2(x, y);
        }

        public float Length() => (float)Math.Sqrt(m_x * m_x + m_y * m_y);
        public float LengthSquared() => (m_x * m_x + m_y * m_y);

        public Vector2 Normalize()
        {
            float length = Length();
            if (length > KTolerance)
            {
                return new Vector2(m_x / length, m_y / length);
            }
            return ZeroVector();
        }

        public static Vector2 ZeroVector() => new Vector2(0, 0);

        public float Dot(Vector2 other) => (m_x * other.m_x) + (m_y * other.m_y);

        public Vector2 ProjectOnto(Vector2 other)
        {
            float scalar = Dot(other) / other.LengthSquared();
            return other * scalar;
        }

        public Vector2 OrthogonalProjectOnto(Vector2 other) => this - ProjectOnto(other);

        public float ManhattanDistance(Vector2 other) => Math.Abs(m_x - other.m_x) + Math.Abs(m_y - other.m_y);

        public bool IsZero(float tolerance = KTolerance) => Math.Abs(m_x) < tolerance && Math.Abs(m_y) < tolerance;

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
    }
}

