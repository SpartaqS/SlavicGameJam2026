using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FuelControl : MonoBehaviour, IParameterControlPanel, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private float fuelPerPumpDistance = 0.1f;
    [SerializeField] private Vector2 moveClamp = new Vector2(0, 1);
    private Reactor reactor = null;

    [SerializeField] private ReactorParameterType parameterType = ReactorParameterType.FuelAmount;

    private Plane draggingPlane;

    private bool dragging = false;
    private Camera mainCamera = null;
    [SerializeField] LocalAudioSourceManager localAudioSourceManager;

    public bool isMalfunctioning { get; private set; }

    [Header("SFX - to control the sound of the pump")]
    [SerializeField] float pitchDiv = 2;

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
                //Debug.Log("Delta on State in Fuel Control");
                return 0f;
            };
        }
    }

    // ---------- Unity methods

    private void Awake()
    {
        draggingPlane = new Plane(-transform.forward, transform.position);
        mainCamera = Camera.main;
        reactor = FindFirstObjectByType<Reactor>();
    }

    // ---------- Event methods

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = mainCamera.ScreenPointToRay(eventData.position);

        if (draggingPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);

            Vector3 localHitPoint = transform.parent != null
                ? transform.parent.InverseTransformPoint(hitPoint)
                : hitPoint;

            Vector3 localPos = transform.localPosition;
            float oldY = localPos.y;
            localPos.y = Mathf.Clamp(localHitPoint.y, moveClamp.x, moveClamp.y);
            transform.localPosition = localPos;

            float distance = oldY - localPos.y;

            if (distance > 0)
            {
                float pumpAmount = distance * fuelPerPumpDistance;

                Debug.Log($"Pumped {pumpAmount} fuel...");
                reactor.ApplyOnClickDelta(parameterType, pumpAmount);
                localAudioSourceManager.SetPitch(distance/pitchDiv);
                localAudioSourceManager.PlayInLoop(SoundManager.Sound.Pump);
            }
            else
            {
                localAudioSourceManager.StopSound();
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        localAudioSourceManager.StopSound();
    }

    public ReactorParameterType controlledParameterType => ReactorParameterType.FuelRodInputPercent;
}
