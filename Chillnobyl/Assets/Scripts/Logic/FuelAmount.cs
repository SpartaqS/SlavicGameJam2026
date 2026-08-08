using System;
using System.Collections.Generic;
using UnityEngine;

public class FuelAmount : ReactorParameter
{
    readonly float minAllowedValue = 0f;  // BALANCEPARAM

    public FuelAmount(float? value = null, Func<float> defaultDeltaFunc = null, Func<bool> hasFailed = null, List<ParameterInfluence> influencedParameters = null)
    {
        type = ReactorParameterType.FuelAmount;

        minValue = LogicConstants.minFuelAmount;
        maxValue = LogicConstants.maxFuelAmount;
        if (value != null)
            this.value = value.Value;
        else
            this.value = 10f;

        if (defaultDeltaFunc != null)
            this.defaultDeltaFunc = defaultDeltaFunc;
        else
            this.defaultDeltaFunc = () => { return 0; };

        if(hasFailed != null)
            this.hasFailed = hasFailed;
        else
        {
            this.hasFailed = WasTooLow;
        }
        if(influencedParameters != null)
            this.influencedParameters = influencedParameters;
        else
        {
            this.influencedParameters = null;
        }
    }

    public override bool WasTooLow()
    {
        return value <= minAllowedValue;
    }
}