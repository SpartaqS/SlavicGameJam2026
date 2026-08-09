using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Temperature : ReactorParameter
{
    readonly float minAllowedValue = 100f; // BALANCEPARAM
    readonly float maxAllowedValue = 1000f; // BALANCEPARAM
    readonly float coolantRDeltaPer100DPerSecond = -1f; // BALANCEPARAM
    readonly float coolantGDeltaPer100DPerSecond = -1f; // BALANCEPARAM
    readonly float coolantBDeltaPer100DPerSecond = -1f; // BALANCEPARAM

    public Temperature(UnityAction<ReactorParameterType, int> intervalReachHandler, Vector3 coolantDeltaPer100DPerSecond, float? value = null, Func<float> defaultDeltaFunc = null, Func<bool> hasFailed = null, List<ParameterInfluence> influencedParameters = null, List<float> stateTresholds = null) : base(type: ReactorParameterType.Temperature, intervalReachHandler, value, stateTresholds)
    {
        //Debug.Log("Derieved constructor");
        coolantRDeltaPer100DPerSecond = coolantDeltaPer100DPerSecond.x;
        coolantGDeltaPer100DPerSecond = coolantDeltaPer100DPerSecond.y;
        coolantBDeltaPer100DPerSecond = coolantDeltaPer100DPerSecond.z;
        minValue = 90f;
        maxValue = 1100f;

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
            this.influencedParameters.Add(new ParameterInfluence(
                    ReactorParameterType.CoolantG,
                    () => { return this.value / 100f * coolantGDeltaPer100DPerSecond * LogicConstants.tickPeroidInSeconds; }
                    )
            );
            this.influencedParameters.Add(new ParameterInfluence(
                    ReactorParameterType.CoolantB,
                    () => { return this.value / 100f * coolantBDeltaPer100DPerSecond * LogicConstants.tickPeroidInSeconds; }
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