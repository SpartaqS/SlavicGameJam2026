using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FuelRodControl : MonoBehaviour, IParameterControlPanel, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float fuelRodPerState = 0.1f;
    [SerializeField] private float pushDistance = 0.25f;
    [SerializeField] private float maxPushBackDistance = 0.025f;
    [SerializeField] private Vector3 localPushDirection = Vector3.forward;
    private Reactor reactor = null;

    [SerializeField] private ReactorParameterType parameterType = ReactorParameterType.FuelRodInputPercent;

    [SerializeField] LocalAudioSourceManager localAudioSourceManager;

    private bool pressed = false;
    private Vector3 startingPos;

    public bool isMalfunctioning { get; private set; }

    public Func<float> deltaOnClick
    {
        get
        {
            return () =>
            {
                Debug.Log("Delta on Click in Fuel Control");
                return 0f;
            };
        }
    }

    public Func<float> deltaOnState
    {
        get
        {
            return () =>
            {
                return pressed ? fuelRodPerState : 0f;
            };
        }
    }

    // ---------- Unity methods

    private void Awake()
    {
        reactor = FindFirstObjectByType<Reactor>();
        startingPos = transform.position;
    }

    private void Update()
    {
        if (!pressed)
        {
            transform.position = Vector3.MoveTowards(transform.position, startingPos, maxPushBackDistance);
        }
    }

    // ---------- Event methods

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        transform.localPosition += localPushDirection.normalized * pushDistance;
        SoundManager._Instance.PlaySound(localAudioSourceManager.audioSource, SoundManager.Sound.ButtonDown);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        SoundManager._Instance.PlaySound(localAudioSourceManager.audioSource, SoundManager.Sound.ButtonUp);
    }

    public ReactorParameterType controlledParameterType => ReactorParameterType.FuelRodInputPercent;
}
