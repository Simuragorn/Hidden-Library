using System;
using UnityEngine;

namespace Assets.Scripts.Serializable
{
    /// <summary> Serializable version of UnityEngine.Quaternion. </summary>
    [Serializable]
    public struct SQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public SQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public override string ToString()
            => $"[{x}, {y}, {z}, {w}]";

        public static implicit operator Quaternion(SQuaternion s)
            => new Quaternion(s.x, s.y, s.z, s.w);

        public static implicit operator SQuaternion(Quaternion q)
            => new SQuaternion(q.x, q.y, q.z, q.w);
    }
}
