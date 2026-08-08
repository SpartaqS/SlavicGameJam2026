using System;
using System.Collections.Generic;
using UnityEngine;

public class Temperature : ReactorParameter
{
    readonly float minAllowedValue = 100f; // BALANCEPARAM
    readonly float maxAllowedValue = 1000f; // BALANCEPARAM
    public Temperature(float? value = null, Func<float> defaultDeltaFunc = null, Func<bool> hasFailed = null, List<ParameterInfluence> influencedParameters = null)
    {
        type = ReactorParameterType.Temperature;
        minValue = 90f;
        maxValue = 1100f;

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
        return WasTooHigh() || WasTooLow();
    }

    public override bool WasTooHigh()
    {
        return value >= maxAllowedValue;
    }

    public override bool WasTooLow()
    {
        return value <= minAllowedValue;
    }
}