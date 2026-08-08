using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoolantG : CoolantBase
{
    public CoolantG(UnityAction<ReactorParameterType, int> intervalReachHandler, float fullCoolantTemperatureDPS, float? value = null, Func<float> defaultDeltaFunc = null, Func<bool> hasFailed = null, List<ParameterInfluence> influencedParameters = null, List<float> stateTresholds = null) : base(coolantType: ReactorParameterType.CoolantG, intervalReachHandler, value, defaultDeltaFunc, hasFailed, stateTresholds: stateTresholds)
    {
        float fullCoolantTemperatureDeltaPerSecond = fullCoolantTemperatureDPS;
        type = ReactorParameterType.CoolantG;
        
        this.influencedParameters = new List<ParameterInfluence>();
        this.influencedParameters.Add(new ParameterInfluence(
                ReactorParameterType.Temperature,
                () => { return this.value/maxValue * fullCoolantTemperatureDeltaPerSecond * LogicConstants.tickPeroidInSeconds; }
                )
        );
        
    }
}