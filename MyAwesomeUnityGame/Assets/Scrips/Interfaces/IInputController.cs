using System;
using UnityEngine;

namespace Interfaces
{
    public interface IInputController
    {
        event Action Jump;
        
        float GetSpeed();

        void Setup(float newMaxSpeed, float newAcceleration, float newStopSmoothingFactor);
    }
}