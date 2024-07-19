using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeController : MonoBehaviour
{
    public Tilemap tilemap;
    public LayerMask collisionLayer; // Layer mask to specify which layers to check for collision
    public float moveTime = 0.2f; // Time it takes to move from one tile to the next

    private Vector3Int targetPosition;

    void Start()
    {
        // Initialize the player's starting position on the tilemap
        targetPosition = tilemap.WorldToCell(transform.position);
        transform.position = tilemap.CellToWorld(targetPosition);
    }

    void Update()
    {
        if (!IsMoving())
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                TryMove(Vector3Int.up);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                TryMove(Vector3Int.down);
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                TryMove(Vector3Int.left);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                TryMove(Vector3Int.right);
        }
    }

    void TryMove(Vector3Int direction)
    {
        Vector3Int newPosition = targetPosition + direction;

        // Check if the target position has a collision
        if (!HasCollision(newPosition))
        {
            targetPosition = newPosition;
            StartCoroutine(MoveToPosition(tilemap.CellToWorld(targetPosition)));
        }
    }

    bool HasCollision(Vector3Int cellPosition)
    {
        Vector3 worldPosition = tilemap.CellToWorld(cellPosition) + tilemap.tileAnchor;
        Collider2D hit = Physics2D.OverlapBox(worldPosition, tilemap.cellSize * 0.9f, 0, collisionLayer);
        return hit != null;
    }

    bool IsMoving()
    {
        return transform.position != tilemap.CellToWorld(targetPosition);
    }

    System.Collections.IEnumerator MoveToPosition(Vector3 target)
    {
        Vector3 start = transform.position;
        float elapsed = 0;
        while (elapsed < moveTime)
        {
            transform.position = Vector3.Lerp(start, target, elapsed / moveTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
    }
}
