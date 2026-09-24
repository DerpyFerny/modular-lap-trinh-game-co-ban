using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private Vector2 minBounds = new Vector2(0, 0);
    [SerializeField] private Vector2 maxBounds = new Vector2(7, 7);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) Move(Vector2.up);
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) Move(Vector2.down);
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) Move(Vector2.left);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) Move(Vector2.right);
    }

    private void Move(Vector2 direction)
    {
        // Tính toán vị trí mới
        Vector2 newPos = (Vector2)transform.position + (direction * gridSize);

        // Ép làm tròn số để TUYỆT ĐỐI không bị lệch
        // Đồng thời Clamp (khóa) không cho đi ra khỏi bàn cờ
        newPos.x = Mathf.Clamp(Mathf.Round(newPos.x), minBounds.x, maxBounds.x);
        newPos.y = Mathf.Clamp(Mathf.Round(newPos.y), minBounds.y, maxBounds.y);

        transform.position = newPos;
    }
}