using System;
using UnityEngine;

namespace Interfaces.Strategies.InputStrategies
{
    public abstract class BaseInputStrategy : MonoBehaviour, IInputController
    {
        private float _maxSpeed;
        private float _acceleration;
        private float _stopSmoothingFactor;

        [Header("Input Settings")] 
        public KeyCode jump;
        
        public event Action Jump;

        public abstract float GetSpeed();

        public void Setup(float newMaxSpeed, float newAcceleration, float newStopSmoothingFactor)
        {
            _maxSpeed = newMaxSpeed;
            _acceleration = newAcceleration;
            _stopSmoothingFactor = newStopSmoothingFactor;
        }
    }
}