using System;
using System.Collections.Generic;
using UnityEngine;

public class Temperature : ReactorParameter
{
    readonly float minAllowedValue = 100f; // BALANCEPARAM
    readonly float maxAllowedValue = 1000f; // BALANCEPARAM
    readonly float coolantRDeltaPer100DPerSecond = -1f; // BALANCEPARAM
    //readonly float coolantGDeltaPer100DPerSecond = -1f; // BALANCEPARAM
    //readonly float coolantBDeltaPer100DPerSecond = -1f; // BALANCEPARAM

    public Temperature(Vector3 coolantDeltaPer100DPerSecond, float? value = null, Func<float> defaultDeltaFunc = null, Func<bool> hasFailed = null, List<ParameterInfluence> influencedParameters = null)
    {
        coolantRDeltaPer100DPerSecond = coolantDeltaPer100DPerSecond.x;
        //coolantGDeltaPer100DPerSecond = coolantDeltaPer100DPerSecond.y;
        //coolantBDeltaPer100DPerSecond = coolantDeltaPer100DPerSecond.z;
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
            this.influencedParameters = new List<ParameterInfluence>();
            this.influencedParameters.Add(new ParameterInfluence(
                    ReactorParameterType.CoolantR,
                    () => { return this.value / 100f * coolantRDeltaPer100DPerSecond * LogicConstants.tickPeroidInSeconds; }
                    )
            );
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