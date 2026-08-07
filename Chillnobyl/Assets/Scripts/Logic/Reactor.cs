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

    public void applyOnClickDelta(ReactorParameterType parameterType, float delta)
    {
        foreach (ReactorParameter parameter in parameters) { 
            if(parameterType == parameter.Type)
            {
                parameter.ApplyDelta(delta);
                break;
            }
        }
    }

    private void Awake() //TODO move to scriptable objects or sth so it is editable outside of code?
    {// Initialize Parameters and their relationships
        parameters = new List<ReactorParameter>();

        ReactorParameter temperature = new Temperature();
        ReactorParameter fuelRodInputPercent = new FuelRodInputPercent();
        ReactorParameter fuelAmount = new FuelAmount();
        parameters.Add(temperature);
        parameters.Add(fuelRodInputPercent);
        parameters.Add(fuelAmount);

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
                Debug.LogWarning($"Failed because: {parameter.Type.ToString()}");
                // TODO trigger failstate
                // loss of control + animations + whatever
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
