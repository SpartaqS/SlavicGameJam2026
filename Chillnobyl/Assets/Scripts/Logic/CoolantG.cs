using System;
using System.Collections.Generic;
using UnityEngine;

public class CoolantG : CoolantBase
{
    public CoolantG(float fullCoolantTemperatureDPS, float? value = null, Func<float> defaultDeltaFunc = null, Func<bool> hasFailed = null, List<ParameterInfluence> influencedParameters = null) : base(value, defaultDeltaFunc, hasFailed)
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