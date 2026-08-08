using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FuelRodInputPercent : ReactorParameter
{
    float fullFuelInputTemperatureDeltaPerSecond = 20f;
    float fullFuelInputConsumptionDeltaPerSecond = 10f;

    public FuelRodInputPercent(UnityAction<ReactorParameterType, int> intervalReachHandler, Vector2 temperatureAndFuelUsageDeltas, float? value = null, Func<float> defaultDeltaFunc = null, Func<bool> hasFailed = null, List<ParameterInfluence> influencedParameters = null, List<float> stateTresholds = null) : base(type: ReactorParameterType.FuelRodInputPercent, intervalReachHandler, value, stateTresholds)
    {
        type = ReactorParameterType.FuelRodInputPercent;
        fullFuelInputTemperatureDeltaPerSecond = temperatureAndFuelUsageDeltas.x;
        fullFuelInputConsumptionDeltaPerSecond = temperatureAndFuelUsageDeltas.y;
        minValue = 0f;
        maxValue = 100f;
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
                    () => { return this.value/100f * fullFuelInputTemperatureDeltaPerSecond * LogicConstants.tickPeroidInSeconds; }
                    )
            );
            this.influencedParameters.Add(new ParameterInfluence(
                    ReactorParameterType.FuelAmount,
                    () => { return -this.value/100f * fullFuelInputConsumptionDeltaPerSecond * LogicConstants.tickPeroidInSeconds; ; }
                    ) // - because more rod inputted => more fuel used
            );
        }
    }
}