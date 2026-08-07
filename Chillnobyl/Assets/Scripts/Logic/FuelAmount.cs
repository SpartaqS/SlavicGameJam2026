using System;
using System.Collections.Generic;
using UnityEngine;

public class FuelAmount : ReactorParameter
{
    float minValue = -1f;
    float maxValue = 100f;
    float minAllowedValue = 0f;  // BALANCEPARAM

    public override void ApplyDelta(float delta)
    {
        base.ApplyDelta(delta);
        value = Mathf.Clamp(value, minValue, maxValue);
    }
    public FuelAmount(float? value = null, Func<float> defaultDeltaFunc = null, Func<bool> hasFailed = null, List<ParameterInfluence> influencedParameters = null)
    {
        type = ReactorParameterType.FuelAmount;
        if (value != null)
            this.value = value.Value;
        else
            this.value = 75f;

        if (defaultDeltaFunc != null)
            this.defaultDeltaFunc = defaultDeltaFunc;
        else
            this.defaultDeltaFunc = () => { return 0; };

        if(hasFailed != null)
            this.hasFailed = hasFailed;
        else
        {
            this.hasFailed = () => { return value <= minAllowedValue; };
        }
        if(influencedParameters != null)
            this.influencedParameters = influencedParameters;
        else
        {
            this.influencedParameters = null;
        }
    }
}