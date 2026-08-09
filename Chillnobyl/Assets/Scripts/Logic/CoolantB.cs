using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoolantB : CoolantBase
{
    public CoolantB(UnityAction<ReactorParameterType, int> intervalReachHandler, float fullCoolantTemperatureDPS, float? value = null, Func<float> defaultDeltaFunc = null, Func<bool> hasFailed = null, List<ParameterInfluence> influencedParameters = null, List<float> stateTresholds = null) : base(coolantType: ReactorParameterType.CoolantB, intervalReachHandler, value, defaultDeltaFunc, hasFailed, stateTresholds: stateTresholds)
    {
        float fullCoolantTemperatureDeltaPerSecond = fullCoolantTemperatureDPS;
        type = ReactorParameterType.CoolantB;
        
        this.influencedParameters = new List<ParameterInfluence>();
        this.influencedParameters.Add(new ParameterInfluence(
                ReactorParameterType.Temperature,
                () => { return this.value/maxValue * fullCoolantTemperatureDeltaPerSecond * LogicConstants.tickPeroidInSeconds; }
                )
        );
        
    }
}