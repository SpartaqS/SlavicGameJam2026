using System;
using UnityEngine;

public class FuelControl : MonoBehaviour, IParameterControlPanel
{
    [SerializeField] private float fuelPerPump = 10f;

    public bool isMalfunctioning { get; private set; }

    public Func<float> deltaOnClick
    {
        get {
            return () =>
            {
                Debug.Log("Delta on Click in Fuel Control");
                return fuelPerPump;
            };
        }
    }

    public Func<float> deltaOnState
    {
        get
        {
            return () =>
            {
                Debug.Log("Delta on State in Fuel Control");
                return 0f;
            };
        }
    }

    public ReactorParameterType controlledParameterType => ReactorParameterType.FuelRodInputPercent;
}
