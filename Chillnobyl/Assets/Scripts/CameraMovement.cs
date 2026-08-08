using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private Collider2D boundingBox;

    //input values
    private Vector3 moveDirection;

    // ---------- Unity methods

    private void Update()
    {
        transform.position += moveDirection * movementSpeed * Time.deltaTime;
        if (!boundingBox.OverlapPoint(transform.position))
            transform.position = boundingBox.ClosestPoint(transform.position);
    }

    // ---------- Input methods

    public void OnMove(InputAction.CallbackContext ctx)
    {
        switch (ctx.phase)
        {
            case InputActionPhase.Performed:
                moveDirection = ctx.ReadValue<Vector2>();
                break;
            case InputActionPhase.Canceled:
                moveDirection = Vector3.zero;
                break;
        }
    }
}
