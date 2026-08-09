using UnityEngine;

public class FuelRodInserter : MonoBehaviour
{
    [SerializeField] private ReactorParameterType parameterType = ReactorParameterType.FuelRodInputPercent;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private Reactor reactor;

    private void Awake()
    {
        reactor = FindFirstObjectByType<Reactor>();
    }

    void Update()
    {
        ReactorParameter parameter = reactor.GetParameter(parameterType);

        Vector3 localPos = transform.localPosition;
        localPos.y = Mathf.Lerp(minY, maxY, Mathf.InverseLerp(parameter.MinValue, parameter.MaxValue, parameter.Value));
        transform.localPosition = localPos;
    }
}
