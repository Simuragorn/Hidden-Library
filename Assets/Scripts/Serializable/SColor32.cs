using System;
using UnityEngine;

namespace Assets.Scripts.Serializable
{
    /// <summary> Serializable version of UnityEngine.Color32 without transparency. </summary>
    [Serializable]
    public struct SColor32
    {
        public byte r;
        public byte g;
        public byte b;

        public SColor32(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public SColor32(Color32 c)
        {
            r = c.r;
            g = c.g;
            b = c.b;
        }

        public override string ToString()
            => $"[{r}, {g}, {b}]";

        public static implicit operator Color32(SColor32 rValue)
            => new Color32(rValue.r, rValue.g, rValue.b, a: byte.MaxValue);

        public static implicit operator SColor32(Color32 rValue)
            => new SColor32(rValue.r, rValue.g, rValue.b);
    }
}
