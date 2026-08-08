using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class Reactor : MonoBehaviour
{
    [SerializeField] float tickPeroidInSeconds;
    [SerializeField] float tickTimer = 0f;

    [SerializeField] List<ReactorParameter> parameters;
    [SerializeField] List<IParameterControlPanel> controlPanels;
    // TODO?
    //OnFail <reason>

    [Header("Reactor parameters values - Game Designers/Balancers welcome")]
    [Header("Temperature")]
    [SerializeField] float temperatureStartValue = 300f;
    [SerializeField] float minAllowedTemperature = 100f;
    [SerializeField] float maxAllowedTemperature = 100f;
    [SerializeField] float temperatureDeltaPerFullCoolantRPerSecond = -20f;
    [SerializeField] float temperatureDeltaPerFullCoolantGPerSecond = -20f;
    [SerializeField] float temperatureDeltaPerFullCoolantBPerSecond = -20f;

    [Header("Coolant")]
    [SerializeField] [Range(LogicConstants.minCoolantColorAmount, LogicConstants.maxCoolantColorAmount)] 
    float coolantRStartValue = 32f;
    [SerializeField] [Range(LogicConstants.minCoolantColorAmount, LogicConstants.maxCoolantColorAmount)] 
    float coolantGStartValue = 32f;
    [SerializeField] [Range(LogicConstants.minCoolantColorAmount, LogicConstants.maxCoolantColorAmount)] 
    float coolantBStartValue = 32f;
    [SerializeField] float coolantRDeltaPer100DPerSecond = -1f;
    [SerializeField] float coolantGDeltaPer100DPerSecond = -1f;
    [SerializeField] float coolantBDeltaPer100DPerSecond = -1f;

    [Header("Fuel")]
    [SerializeField] [Range(LogicConstants.minFuelAmount, LogicConstants.maxFuelAmount)]
    float startingRodInputPercent = 10f;
    [SerializeField] [Range(LogicConstants.minFuelAmount, LogicConstants.maxFuelAmount)] 
    float startingFuelAmount = 10f;
    [SerializeField] float fullFuelInputTemperatureDeltaPerSecond = 20f;
    [SerializeField] float fullFuelInputConsumptionDeltaPerSecond = 10f;


    GameplayManager gameplayManager;

    public void ApplyOnClickDelta(ReactorParameterType parameterType, float delta)
    {
        foreach (ReactorParameter parameter in parameters) { 
            if(parameterType == parameter.Type)
            {
                parameter.ApplyDelta(delta);
                break;
            }
        }
    }

    public ReactorParameter GetParameter(ReactorParameterType parameterType)
    {
        foreach (ReactorParameter parameter in parameters)
        {
            if (parameterType == parameter.Type)
            {
                return parameter;
            }
        }

        return null;
    }

    private void Awake()
    {// Initialize Parameters and their relationships
        gameplayManager = FindFirstObjectByType<GameplayManager>();

        InitializeReactorParameters();

        tickPeroidInSeconds = LogicConstants.tickPeroidInSeconds;

        // Get references to all control panels
        controlPanels = new List<IParameterControlPanel>();
        var theInterface = typeof(IParameterControlPanel);
        var types = AppDomain.CurrentDomain.GetAssemblies() // Get all assemblies
            .SelectMany(a => a.GetTypes()) // Get all types from all assemblies
            .Where(t => theInterface.IsAssignableFrom(t)); // Search for types that implement the interface

        var allGameObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IParameterControlPanel>();
        foreach(IParameterControlPanel cp in allGameObjects)
        {
            controlPanels.Add(cp);
        }
            
    }

    [ContextMenu("Set reactor parameters to config values")]
    void InitializeReactorParameters()
    {
        parameters = new List<ReactorParameter>();

        Vector3 CoolantsRGBLossPer100D = new Vector3(coolantRDeltaPer100DPerSecond, coolantGDeltaPer100DPerSecond, coolantBDeltaPer100DPerSecond);
        Vector2 fuelRodDeltas = new Vector2(fullFuelInputTemperatureDeltaPerSecond, fullFuelInputConsumptionDeltaPerSecond);

        ReactorParameter temperature = new Temperature(coolantDeltaPer100DPerSecond: CoolantsRGBLossPer100D, value: temperatureStartValue);
        ReactorParameter fuelRodInputPercent = new FuelRodInputPercent(value: startingRodInputPercent, temperatureAndFuelUsageDeltas: fuelRodDeltas);
        ReactorParameter fuelAmount = new FuelAmount(value: startingFuelAmount);
        ReactorParameter coolantRAmount = new CoolantR(fullCoolantTemperatureDPS: temperatureDeltaPerFullCoolantRPerSecond,
                                                        value: coolantRStartValue);
        ReactorParameter coolantGAmount = new CoolantG(fullCoolantTemperatureDPS: temperatureDeltaPerFullCoolantGPerSecond,
                                                        value: coolantGStartValue);
        ReactorParameter coolantBAmount = new CoolantB(fullCoolantTemperatureDPS: temperatureDeltaPerFullCoolantBPerSecond,
                                                        value: coolantBStartValue);

        parameters.Add(temperature);
        parameters.Add(fuelRodInputPercent);
        parameters.Add(fuelAmount);
        parameters.Add(coolantRAmount);
        parameters.Add(coolantGAmount);
        parameters.Add(coolantBAmount);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DebugPrintParameters();
        tickTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {// tick system here?
        tickTimer += Time.deltaTime;
        if (tickTimer > tickPeroidInSeconds)
        {
            tickTimer -= tickPeroidInSeconds;
            GameTick();
        }
    }

    private void GameTick()
    {
        ComputeParameters();
        CheckForFailure();
        DebugPrintParameters();
    }


    private void ComputeParameters() // happens per logic tick
    {
        Dictionary<ReactorParameterType, List<float>> deltasForParameters = new Dictionary<ReactorParameterType, List<float>>();

        // create disctionary for colleting deltas
        var parametersType = EnumUtil.GetValues<ReactorParameterType>();
        foreach (ReactorParameterType parameterType in parametersType)
        {
            deltasForParameters.Add(parameterType, new List<float>());
        }

        // loop thorugh parameters, collect deltas for every parameter
        foreach (ReactorParameter parameter in parameters)
        {
            if(parameter.InfluencedParameters != null && parameter.InfluencedParameters.Count > 0)
            {
                foreach (ParameterInfluence ip in parameter.InfluencedParameters)
                {
                    deltasForParameters[ip.InfluencedParameter].Add(ip.DeltaFunc());
                }
            }
        }

        // loop thorugh controls, collect deltas for evert parameter
        foreach (IParameterControlPanel controlPanel in controlPanels)
        {
            if (!MathF.Equals(controlPanel.deltaOnState, 0f))
            {
                deltasForParameters[controlPanel.controlledParameterType].Add(controlPanel.deltaOnState());
            }
        }

        // apply deltas

        foreach (ReactorParameterType parameterType in parametersType)
        {
            float sum = 0; // sum of deltas per logic tick
            foreach(float delta in deltasForParameters[parameterType])
            {
                sum += delta;
            }

            foreach(ReactorParameter reactorParameter in parameters)
            {
                if(reactorParameter.Type == parameterType)
                {
                    Debug.Log($"Applying delta {sum} to {parameterType}");
                    reactorParameter.ApplyDelta(sum);
                    break;// should be only one parameter of each type
                }
            }
        }
    }

    private void CheckForFailure()
    {
        // loop through parameters and check if we died
        foreach (ReactorParameter parameter in parameters)
        {
            if(parameter.HasFailed())
            {
                // TODO trigger failstate
                // loss of control + animations + whatever
                gameplayManager.HandleGameLoss(parameter.Type, parameter.WasTooHigh());
            }
        }
    }

    private void DebugPrintParameters()
    {
        foreach (ReactorParameter parameter in parameters)
        {
            Debug.Log($"{parameter.Type.ToString()} : {parameter.Value.ToString()}");
        }
    }
}
