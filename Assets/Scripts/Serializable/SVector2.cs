using System;
using UnityEngine;

namespace Assets.Scripts.Serializable
{
    /// <summary> Serializable version of UnityEngine.Vector2. </summary>
    [Serializable]
    public struct SVector2
    {
        public float x;
        public float y;

        public SVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
            => $"[x, y]";

        public static implicit operator Vector2(SVector2 s)
            => new Vector2(s.x, s.y);

        public static implicit operator SVector2(Vector2 v)
            => new SVector2(v.x, v.y);


        public static SVector2 operator +(SVector2 a, SVector2 b)
            => new SVector2(a.x + b.x, a.y + b.y);

        public static SVector2 operator -(SVector2 a, SVector2 b)
            => new SVector2(a.x - b.x, a.y - b.y);

        public static SVector2 operator -(SVector2 a)
            => new SVector2(-a.x, -a.y);

        public static SVector2 operator *(SVector2 a, float m)
            => new SVector2(a.x * m, a.y * m);

        public static SVector2 operator *(float m, SVector2 a)
            => new SVector2(a.x * m, a.y * m);

        public static SVector2 operator /(SVector2 a, float d)
            => new SVector2(a.x / d, a.y / d);
    }
}
