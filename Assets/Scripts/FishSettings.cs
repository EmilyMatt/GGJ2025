using System;
using UnityEngine;

namespace FishSettings
{
    [Serializable]
    public class FishStats
    {
        public float youngThreshold = 35;
        public float adultThreshold = 60;
        public float hungerThreshold = 40;
        public float starveThreshold = 20;
        public float maxHungerLevel = 60;
        public Vector4 aquariumRanges = new(-6, 6, -4, 4);
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