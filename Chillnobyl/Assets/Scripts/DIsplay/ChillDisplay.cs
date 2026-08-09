using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ChillDisplay : MonoBehaviour
{
    [SerializeField] private ReactorParameterType parameterTypeR = ReactorParameterType.CoolantR;
    [SerializeField] private ReactorParameterType parameterTypeG = ReactorParameterType.CoolantG;
    [SerializeField] private ReactorParameterType parameterTypeB = ReactorParameterType.CoolantB;
    [SerializeField] private Color readOnlyDebugColor;
    [SerializeField] private float alpha = 1f;

    private Reactor reactor;
    private MeshRenderer renderer;
    private MaterialPropertyBlock propertyBlock;

    private void Awake()
    {
        reactor = FindFirstObjectByType<Reactor>();
        renderer = GetComponent<MeshRenderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        ReactorParameter parameterR = reactor.GetParameter(parameterTypeR);
        ReactorParameter parameterG = reactor.GetParameter(parameterTypeG);
        ReactorParameter parameterB = reactor.GetParameter(parameterTypeB);

        Color chillColor = new Color(
            parameterR.Value / 255,
            parameterG.Value / 255,
            parameterB.Value / 255,
            alpha);

        Debug.LogWarning(chillColor);

        readOnlyDebugColor = chillColor;

        propertyBlock.SetColor("_Color", chillColor);
        renderer.SetPropertyBlock(propertyBlock);
    }
}
