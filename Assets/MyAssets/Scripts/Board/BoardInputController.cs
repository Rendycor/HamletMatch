using UnityEngine;
using UnityEngine.InputSystem;

public class BoardInputController : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current == null || Camera.main == null)
            return;

        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                TypeInput type = hit.collider.GetComponent<TypeInput>();
                if (type != null)
                {
                    TypeInput.TryBeginDrag(type);
                }
            }
        }

        if (Mouse.current.leftButton.isPressed)
        {
            TypeInput.UpdateDrag();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            TypeInput.EndDrag();
        }
    }
}
