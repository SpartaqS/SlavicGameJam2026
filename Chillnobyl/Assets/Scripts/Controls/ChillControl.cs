using UnityEngine;
using UnityEngine.EventSystems;

public class ChillControl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform leverBase;
    [SerializeField] private Transform leverRotationBase;
    [SerializeField] private float maxDragDistance = 4f;
    [SerializeField] private float minAngle = 0f;
    [SerializeField] private float maxAngle = -60f;
    [SerializeField] private float leverBackLerpValue = 0.05f;
    [SerializeField] private float leverBackLerpMin = 0.01f;

    private Reactor reactor = null;
    private bool dragging;

    private Vector3 localStartDragPos;

    private Plane draggingPlane;
    private Camera mainCamera = null;

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
