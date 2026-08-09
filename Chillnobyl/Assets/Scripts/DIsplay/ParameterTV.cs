using UnityEngine;
using TMPro;

public class ParameterTV : MonoBehaviour
{
    [SerializeField] private ReactorParameterType parameterType = ReactorParameterType.FuelAmount;
    [SerializeField] private TMP_Text mainText;
    [SerializeField] private Color mainTextColor = Color.white;
    [SerializeField] private bool showMinText = false;
    [SerializeField] private TMP_Text minText;
    [SerializeField] private Color minTextColor = Color.white;
    [SerializeField] private bool showMaxText = false;
    [SerializeField] private TMP_Text maxText;
    [SerializeField] private Color maxTextColor = Color.white;

    [SerializeField] private string addedAtEnd = "";

    private Reactor reactor;

    private void Awake()
    {
        reactor = FindFirstObjectByType<Reactor>();
        mainText.color = mainTextColor;
        if (minText) minText.color = minTextColor;
        if (maxText) maxText.color = maxTextColor;
    }

    void Update()
    {
        ReactorParameter parameter = reactor.GetParameter(parameterType);

        mainText.text = parameter.Value.ToString("0.00") + addedAtEnd;
        minText.text = showMinText ? parameter.MinValue.ToString("0.00") : "";
        maxText.text = showMaxText ? parameter.MaxValue.ToString("0.00") : "";
    }
}
