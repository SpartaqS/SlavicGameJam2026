using UnityEngine;

public class ParameterGauge : MonoBehaviour
{
    [SerializeField] ReactorParameterType parameterType = ReactorParameterType.FuelAmount;
    [SerializeField] private Transform needle;
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;
    private Reactor reactor;

    private void Awake()
    {
        reactor = FindFirstObjectByType<Reactor>();
    }

    void Update()
    {
        ReactorParameter parameter = reactor.GetParameter(parameterType);
        float needleAngle = Mathf.LerpAngle(minAngle, maxAngle, parameter.Value / (parameter.MaxValue - parameter.MinValue));
        needle.localRotation = Quaternion.Euler(needleAngle, -90f, -90f);
    }
}
