using System;
using System.Collections.Generic;
using UnityEngine;

public class Temperature : ReactorParameter
{
    float minAllowedTemperature = 100f; // BALANCEPARAM
    float maxAllowedTemperature = 1000f; // BALANCEPARAM
    public Temperature(float? value = null, Func<float> defaultDeltaFunc = null, Func<bool> hasFailed = null, List<ParameterInfluence> influencedParameters = null)
    {
        type = ReactorParameterType.Temperature;
        if (value != null)
            this.value = value.Value;
        else
            this.value = 200f;

        if (defaultDeltaFunc != null)
            this.defaultDeltaFunc = defaultDeltaFunc;
        else
            this.defaultDeltaFunc = () => { return 0; };

        if(hasFailed != null)
            this.hasFailed = hasFailed;
        else
        {
            this.hasFailed = HasFailedFunc;
        }
        if(influencedParameters != null)
            this.influencedParameters = influencedParameters;
        else
        {
            this.influencedParameters = null;
        }
    }

    private bool HasFailedFunc()
    {
        return value > maxAllowedTemperature || value < minAllowedTemperature;
    }
}