using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ParameterSlider : MonoBehaviour
{
    [SerializeField] private Reactor reactor;
    [SerializeField] private ReactorParameterType parameterType;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        ReactorParameter parameter = reactor.GetParameter(parameterType);
        slider.value = parameter.Value / (parameter.MaxValue - parameter.MinValue);
    }
}
