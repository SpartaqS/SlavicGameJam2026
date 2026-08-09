using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ReactorParameter
{
    protected ReactorParameterType type;
    protected float value;
    protected float minValue = 0f;
    protected float maxValue = 100f;
    protected Func<float> defaultDeltaFunc;
    protected Func<bool> hasFailed;
    protected List<ParameterInfluence> influencedParameters;

    protected List<float> stateTresholds;
    protected int currentState;

    public UnityEvent<ReactorParameterType, int> OnIntervalReach = new UnityEvent<ReactorParameterType, int>();

    protected ReactorParameter(ReactorParameterType type, UnityAction<ReactorParameterType, int> intervalReachHandler, float ? value = null, List<float> stateTresholds = null)
    {
        this.type = type;
        OnIntervalReach.AddListener(intervalReachHandler);
        //Debug.Log("BaseConstructor");
        if (value != null)
            this.value = value.Value;
        else
            this.value = 200f;

        if (stateTresholds == null)
            this.stateTresholds = new List<float>();
        else
        {
            this.stateTresholds = stateTresholds;
        }
        // calculate first state
        currentState = GeteCurrentState();
        OnIntervalReach?.Invoke(this.type, currentState);
    }

    public float Value { get => value; }
    public float MinValue { get => minValue; }
    public float MaxValue { get => maxValue; }
    public virtual void ApplyDelta(float delta)
    {
        value += delta;
        value = Mathf.Clamp(value, minValue, maxValue);

        // check if we reached new state
        int newCurrentState = GeteCurrentState();
        if (newCurrentState != currentState)
        {
            currentState = newCurrentState;
            OnIntervalReach?.Invoke(this.type, newCurrentState);
        }
    }
    public Func<float> DefaultDeltaFunc { get => defaultDeltaFunc; }
    public Func<bool> HasFailed { get => hasFailed; }

    public virtual bool WasTooHigh() { return false; }
    public virtual bool WasTooLow() { return false; }
    public List<ParameterInfluence> InfluencedParameters { get => influencedParameters; }
    public ReactorParameterType Type { get => type; set => type = value; }

    private int GeteCurrentState()
    {
        int newCurrentState = 0;

        for (int i = 0; i < this.stateTresholds.Count; i++)
        {
            float curTresh = this.stateTresholds[i];
            if (value > curTresh)
            {
                newCurrentState = i + 1;
            }
            else
            {// found interval within which is the value
                break; // i == 0 => leftmost (state = 0)
            }
        }

        return newCurrentState;
    }
}

public class ParameterInfluence
{
    ReactorParameterType influencedParameter;
    Func<float> deltaFunc;

    public ParameterInfluence(ReactorParameterType influencedParameter, Func<float> deltaFunc)
    {
        this.influencedParameter = influencedParameter;
        this.deltaFunc = deltaFunc;
    }

    public ReactorParameterType InfluencedParameter { get => influencedParameter; }
    public Func<float> DeltaFunc { get => deltaFunc; }
}

public enum ReactorParameterType
{
    Temperature = 0,
    FuelRodInputPercent = 1,
    FuelAmount = 2,
    CoolantR = 3,
    CoolantG = 4,
    CoolantB = 5,
    //PowerOutput = 6
}
