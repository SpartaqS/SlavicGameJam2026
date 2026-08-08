using System;
using System.Collections.Generic;
using UnityEngine;

public class CoolantB : CoolantBase
{
    public CoolantB(float? value = null, Func<float> defaultDeltaFunc = null, Func<bool> hasFailed = null, List<ParameterInfluence> influencedParameters = null) : base(value, defaultDeltaFunc, hasFailed)
    {
        float fullCoolantTemperatureDeltaPerSecond = -20f;
        type = ReactorParameterType.CoolantB;
        
        this.influencedParameters = new List<ParameterInfluence>();
        this.influencedParameters.Add(new ParameterInfluence(
                ReactorParameterType.Temperature,
                () => { return this.value/maxValue * fullCoolantTemperatureDeltaPerSecond * LogicConstants.tickPeroidInSeconds; }
                )
        );
        
    }
}