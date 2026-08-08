using System;
using System.Collections.Generic;
using UnityEngine;

public class CoolantR : CoolantBase
{
    public CoolantR(float fullCoolantTemperatureDPS, float? value = null, Func<float> defaultDeltaFunc = null, Func<bool> hasFailed = null, List<ParameterInfluence> influencedParameters = null) : base(value, defaultDeltaFunc, hasFailed)
    {
        float fullCoolantTemperatureDeltaPerSecond = fullCoolantTemperatureDPS;
        type = ReactorParameterType.CoolantR;
        
        this.influencedParameters = new List<ParameterInfluence>();
        this.influencedParameters.Add(new ParameterInfluence(
                ReactorParameterType.Temperature,
                () => { return this.value/maxValue * fullCoolantTemperatureDeltaPerSecond * LogicConstants.tickPeroidInSeconds; }
                )
        );
        
    }
}