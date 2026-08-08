using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoolantBase : ReactorParameter
{
    protected float fullCoolantTemperatureDeltaPerSecond = 0f;
    public CoolantBase(float? value = null, Func<float> defaultDeltaFunc = null, Func<bool> hasFailed = null, List<ParameterInfluence> influencedParameters = null)
    {
        type = ReactorParameterType.CoolantR;

        minValue = LogicConstants.minCoolantColorAmount;
        maxValue = LogicConstants.maxCoolantColorAmount;
        if (value != null)
            this.value = value.Value;
        else
            this.value = 128f;

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
                    () => { return this.value/maxValue * fullCoolantTemperatureDeltaPerSecond * LogicConstants.tickPeroidInSeconds; }
                    )
            );
        }
    }
}