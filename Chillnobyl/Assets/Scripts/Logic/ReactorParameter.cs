using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReactorParameter
{
    protected ReactorParameterType type;
    protected float value;
    protected Func<float> defaultDeltaFunc;
    protected Func<bool> hasFailed;
    protected List<ParameterInfluence> influencedParameters;

    public float Value { get => value; }
    public virtual void ApplyDelta(float delta)
    {
        value += delta;
    }
    public Func<float> DefaultDeltaFunc { get => defaultDeltaFunc; }
    public Func<bool> HasFailed { get => hasFailed; } // failValue
    public List<ParameterInfluence> InfluencedParameters { get => influencedParameters; }
    public ReactorParameterType Type { get => type; set => type = value; }
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
    //FuelAmount = 2,
    //Coolant = 3,
    //PowerOutput = 4
}
