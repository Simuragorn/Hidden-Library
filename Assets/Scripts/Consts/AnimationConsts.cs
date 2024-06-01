using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Consts
{
    public static class AnimationConsts
    {
        public const string AnimationStateKey = "state";
        public static class NarrativeIcon
        {
            public const int ShowIconValue = 0;
            public const int HideIconValue = 2;
        }
        public static class NarrativePanel 
        {
            public const int ShowPanelValue = 0;
            public const int HidePanelValue = 2;
        }

        public static class Character
        {
            public const string IdleAnimationName = "idle with blinking";
            public const string WalkingAnimationName = "walk";
            public const string TakeHighObjectAnimationName = "action: pick up high object";
        }

        public static class BalancingObject
        {
            public const string IsTouchedValueName = "isTouched";
        }
    }
}
