using System;
using System.Collections.Generic;
using UnityEngine;

public class FuelRodInputPercent : ReactorParameter
{
    float minValue = 0f;
    float maxValue = 1f;
    float FullFuelInputTemperatureDelta = 100f;

    public override void ApplyDelta(float delta)
    {
        base.ApplyDelta(delta);
        value = Mathf.Clamp(value, minValue, maxValue);
    }
    public FuelRodInputPercent(float? value = null, Func<float> defaultDeltaFunc = null, Func<bool> hasFailed = null, List<ParameterInfluence> influencedParameters = null)
    {
        type = ReactorParameterType.Temperature;
        if (value != null)
            this.value = value.Value;
        else
            this.value = 0.2f;

        if (defaultDeltaFunc != null)
            this.defaultDeltaFunc = defaultDeltaFunc;
        else
            this.defaultDeltaFunc = () => { return 0; };

        if(hasFailed != null)
            this.hasFailed = hasFailed;
        else
        {
            this.hasFailed = () => { return false; };
        }
        if(influencedParameters != null)
            this.influencedParameters = influencedParameters;
        else
        {
            this.influencedParameters = new List<ParameterInfluence>();
            this.influencedParameters.Add(new ParameterInfluence(
                    ReactorParameterType.Temperature,
                    () => { return this.value * FullFuelInputTemperatureDelta; }
                    )
            );
        }
    }
}