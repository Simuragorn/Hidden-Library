using UnityEngine;

namespace Assets.Scripts.Core
{
    public static class MathHelper
    {
        public static int GetRandomSign()
        {
            return Random.value < 0.5 ? -1 : 1;
        }
    }
}
