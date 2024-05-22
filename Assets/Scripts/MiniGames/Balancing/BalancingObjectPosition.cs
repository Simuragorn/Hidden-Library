using Assets.Scripts.Enums;
using Assets.Scripts.Serializable;
using System;

namespace Assets.Scripts.MiniGames.Balancing
{
    [Serializable]
    public class BalancingObjectPosition
    {
        public BalancingObjectTypeEnum BalancingObjectType;
        public SVector2 LocalPosition;
        public SVector3 LocalRotation;
    }
}
