using System;
using Unity.VisualScripting;
using UnityEngine;

public class DebugFuelRodControl : MonoBehaviour, IParameterControlPanel
{
    public ReactorParameterType controlledParameterType => ReactorParameterType.FuelRodInputPercent;
    [SerializeField] private float fuelRodIncreasePerDelta = 0.01f;
    [SerializeField] private bool lowerRods = true;

    public bool isMalfunctioning { get; private set; }

    public Func<float> deltaOnClick
    {
        get {
            return () =>
            {
                return 0f;
            };
        }
    }

    public Func<float> deltaOnState
    {
        get
        {
            return () =>
            {
                float mult = -1f;
                if (lowerRods)
                    mult = 1f;
                return fuelRodIncreasePerDelta * mult;
            };
        }
    }
}
