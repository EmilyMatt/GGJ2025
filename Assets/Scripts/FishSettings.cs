using System;
using UnityEditor.Animations;
using UnityEngine;

namespace FishSettings
{
    [Serializable]
    public class FishStats
    {
        public AnimatorController adultSprite;
        public float adultThreshold = 10;
        public Vector4 aquariumRanges;
        public float hungerThreshold = 10;
        public float maxHungerLevel;
        public float starveThreshold = 5;
        public AnimatorController youngSprite;
        public float youngThreshold = 5;
    }

    public enum FishDirection
    {
        Left,
        Right
    }

    public enum LifeStage
    {
        Beby,
        Young,
        Adult
    }


    public enum FishState
    {
        Chillin,
        Hungry,
        Sick,
        Dead
    }

    public enum TargetType
    {
        None,
        Idle,
        Food
    }
}