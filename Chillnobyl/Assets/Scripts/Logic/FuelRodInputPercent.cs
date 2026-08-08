using System;
using System.Collections.Generic;
using UnityEngine;

public class FuelRodInputPercent : ReactorParameter
{
    float fullFuelInputTemperatureDeltaPerSecond = 20f;
    float fullFuelInputConsumptionDeltaPerSecond = 10f;

    public FuelRodInputPercent(Vector2 temperatureAndFuelUsageDeltas, float? value = null, Func<float> defaultDeltaFunc = null, Func<bool> hasFailed = null, List<ParameterInfluence> influencedParameters = null)
    {
        type = ReactorParameterType.FuelRodInputPercent;
        fullFuelInputTemperatureDeltaPerSecond = temperatureAndFuelUsageDeltas.x;
        fullFuelInputConsumptionDeltaPerSecond = temperatureAndFuelUsageDeltas.y;
        minValue = 0f;
        maxValue = 1f;
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
                    () => { return this.value * fullFuelInputTemperatureDeltaPerSecond * LogicConstants.tickPeroidInSeconds; }
                    )
            );
            this.influencedParameters.Add(new ParameterInfluence(
                    ReactorParameterType.FuelAmount,
                    () => { return -this.value * fullFuelInputConsumptionDeltaPerSecond * LogicConstants.tickPeroidInSeconds; ; }
                    ) // - because more rod inputted => more fuel used
            );
        }
    }
}