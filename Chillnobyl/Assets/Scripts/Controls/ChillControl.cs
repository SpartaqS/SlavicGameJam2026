using UnityEngine;
using UnityEngine.EventSystems;

public class ChillControl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform leverBase;
    [SerializeField] private float maxDragDistance = 4f;
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
            leverBase.rotation = Quaternion.Euler(
                Mathf.LerpAngle(
                    leverBase.rotation.eulerAngles.x,
                    0f,
                    leverBackLerpValue),
                0f,
                0f);

            if (Mathf.Abs(leverBase.rotation.eulerAngles.x) < leverBackLerpMin)
                leverBase.rotation = Quaternion.Euler(0f, 0f, 0f);
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

            Vector3 localHitPoint = leverBase != null
                ? leverBase.InverseTransformPoint(hitPoint)
                : hitPoint;

            localStartDragPos = transform.localPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = mainCamera.ScreenPointToRay(eventData.position);

        if (draggingPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);

            Vector3 localHitPoint = leverBase != null
                ? leverBase.InverseTransformPoint(hitPoint)
                : hitPoint;

            float lerpValue = (localStartDragPos.y - localHitPoint.y) / maxDragDistance;
            float newAngle = Mathf.Lerp(0f, maxAngle, lerpValue);
            leverBase.rotation = Quaternion.Euler(newAngle, 0f, 0f);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
    }
}
