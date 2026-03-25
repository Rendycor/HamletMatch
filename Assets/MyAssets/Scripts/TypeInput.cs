using UnityEngine;
using UnityEngine.InputSystem;

public class TypeInput : MonoBehaviour
{
    public Vector2Int GridPos;

    private static TypeInput activeType;
    private static Vector2 startWorldPos;

    private const float DRAG_THRESHOLD = 0.2f;

    private BoardManager board;

    void Awake()
    {
        board = FindFirstObjectByType<BoardManager>();
    }

    public static void TryBeginDrag(TypeInput type)
    {
        if (type.board.IsBusy)
            return;
            
        activeType = type;

        Vector3 world = Camera.main.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );
        world.z = 0f;
        startWorldPos = world;
    }

    public static void UpdateDrag()
    {
        if (activeType == null) return;

        Vector3 world = Camera.main.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );
        world.z = 0f;

        Vector2 delta = (Vector2)world - startWorldPos;
        if (delta.magnitude < DRAG_THRESHOLD) return;

        Vector2Int dir =
            Mathf.Abs(delta.x) > Mathf.Abs(delta.y)
                ? (delta.x > 0 ? Vector2Int.right : Vector2Int.left)
                : (delta.y > 0 ? Vector2Int.up : Vector2Int.down);

        Vector2Int target = activeType.GridPos + dir;

        if (activeType.board.IsInsideBoard(target))
        {
            if (activeType.board.IsBusy)
                return;
            activeType.board.SwapAndResolve(activeType.GridPos, target);
        }

        activeType = null;
    }

    public static void EndDrag()
    {
        activeType = null;
    }
}
