using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FuelAmount : ReactorParameter
{
    readonly float minAllowedValue = 0f;  // BALANCEPARAM

    public FuelAmount(UnityAction<ReactorParameterType, int> intervalReachHandler, float? value = null, Func<float> defaultDeltaFunc = null, Func<bool> hasFailed = null, List<ParameterInfluence> influencedParameters = null, List<float> stateTresholds = null) : base(type: ReactorParameterType.FuelAmount, intervalReachHandler, value, stateTresholds)
    {
        minValue = LogicConstants.minFuelAmount;
        maxValue = LogicConstants.maxFuelAmount;

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