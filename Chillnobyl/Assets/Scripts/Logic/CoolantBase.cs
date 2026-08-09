using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CoolantBase : ReactorParameter
{
    protected float fullCoolantTemperatureDeltaPerSecond = 0f;
    public CoolantBase(ReactorParameterType coolantType, UnityAction<ReactorParameterType, int> intervalReachHandler, float? value = null, Func<float> defaultDeltaFunc = null, Func<bool> hasFailed = null, List<ParameterInfluence> influencedParameters = null, List<float> stateTresholds = null) : base(type: coolantType, intervalReachHandler, value, stateTresholds: stateTresholds)
    {
        type = ReactorParameterType.CoolantR;

        minValue = LogicConstants.minCoolantColorAmount;
        maxValue = LogicConstants.maxCoolantColorAmount;

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