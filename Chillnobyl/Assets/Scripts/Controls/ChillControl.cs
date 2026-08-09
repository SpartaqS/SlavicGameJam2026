using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChillControl : MonoBehaviour, IParameterControlPanel, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform leverBase;
    [SerializeField] private Transform leverRotationBase;
    [SerializeField] private float maxDragDistance = 4f;
    [SerializeField] private float minAngle = 0f;
    [SerializeField] private float maxAngle = -60f;
    [SerializeField] private float leverBackLerpValue = 0.05f;
    [SerializeField] private float leverBackLerpMin = 0.01f;
    [SerializeField] private ReactorParameterType parameterType = ReactorParameterType.CoolantR;
    [SerializeField] private float coolantPerDistancePerState = 1f;

    private Reactor reactor = null;
    private bool dragging;

    private Vector3 localStartDragPos;

    private Plane draggingPlane;
    private Camera mainCamera = null;
    [SerializeField] LocalAudioSourceManager localAudioSourceManager;
    public ReactorParameterType controlledParameterType => parameterType;

    public bool isMalfunctioning { get; set; }

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
                float currentAngle = (leverBase.localRotation.eulerAngles.x + Mathf.Abs(minAngle)) % 360;
                if(currentAngle > minAngle + Mathf.Abs(minAngle))
                {
                    float soundVolume = Mathf.InverseLerp(minAngle + Mathf.Abs(minAngle), maxAngle + Mathf.Abs(minAngle), currentAngle);
                    localAudioSourceManager.SetVolume(soundVolume);
                    localAudioSourceManager.PlayInLoop(SoundManager.Sound.ChillFaucet);
                    // play currentAngle/(minAngle + Mathf.Abs(minAngle))

                }
                else
                {
                    //stop
                    localAudioSourceManager.StopSound();
                }
                return currentAngle > minAngle + Mathf.Abs(minAngle) ? Mathf.InverseLerp(minAngle + Mathf.Abs(minAngle), maxAngle + Mathf.Abs(minAngle), currentAngle) * coolantPerDistancePerState : 0f;
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

    private void Update()
    {
        if (!dragging)
        {
            leverBase.localRotation = Quaternion.Euler(
                Mathf.LerpAngle(
                    leverBase.localRotation.eulerAngles.x,
                    minAngle,
                    leverBackLerpValue),
                0f,
                0f);

            if (Mathf.Abs(leverBase.localRotation.eulerAngles.x) < leverBackLerpMin)
                leverBase.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    // ---------- Event methods

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;

        Ray ray = mainCamera.ScreenPointToRay(eventData.position);

        if (draggingPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);

            Vector3 localHitPoint = leverRotationBase != null
                ? leverRotationBase.InverseTransformPoint(hitPoint)
                : hitPoint;

            localStartDragPos = localHitPoint;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = mainCamera.ScreenPointToRay(eventData.position);

        if (draggingPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);

            Vector3 localHitPoint = leverRotationBase != null
                ? leverRotationBase.InverseTransformPoint(hitPoint)
                : hitPoint;

            float lerpValue = (localStartDragPos.y - localHitPoint.y) / maxDragDistance;
            float newAngle = Mathf.LerpAngle(minAngle, maxAngle, lerpValue);
            leverBase.localRotation = Quaternion.Euler(newAngle, 0f, 0f);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
    }
}
