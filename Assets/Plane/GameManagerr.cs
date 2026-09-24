using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerr : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int gridSize = 8;

    // Lưu trữ các ô vuông trên bàn cờ
    private Dictionary<Vector2, SpriteRenderer> tiles = new Dictionary<Vector2, SpriteRenderer>();
    // Lưu các ô nguy hiểm (đã sáng lên)
    private List<Vector2> dangerZones = new List<Vector2>();

    private int score = 0;

    void Start()
    {
        GenerateChessboard();
        StartCoroutine(GameLoop()); // Bắt đầu vòng lặp game
    }

    void GenerateChessboard()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector2 pos = new Vector2(x, y);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity);

                // Sơn màu caro (Trắng và Xám)
                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                tiles.Add(pos, sr);
            }
        }
        ResetTileColors(); // Cập nhật màu xen kẽ
    }

    IEnumerator GameLoop()
    {
        Debug.Log("Game bắt đầu sau 3 giây...");
        yield return new WaitForSeconds(3f);

        while (true) // Lặp lại các lượt chơi liên tục
        {
            dangerZones.Clear();
            ResetTileColors();

            // 1. Chọn ngẫu nhiên 15 ô sẽ sáng lên
            for (int i = 0; i < 50; i++)
            {
                Vector2 randomPos = new Vector2(Random.Range(0, gridSize), Random.Range(0, gridSize));
                if (!dangerZones.Contains(randomPos))
                {
                    dangerZones.Add(randomPos);
                    tiles[randomPos].color = Color.red; // Sáng Đỏ
                }
            }

            // 2. Hiện màu đỏ trong 1.5 giây để Player ghi nhớ
            yield return new WaitForSeconds(1.5f);

            // 3. TẮT SÁNG (Trả về màu caro bình thường)
            ResetTileColors();

            // 4. Cho người chơi 2 giây để di chuyển thoát thân
            Debug.Log("Di chuyển đi!");
            yield return new WaitForSeconds(2f);

            // 5. KIỂM TRA ĐIỂM
            Vector2 playerPos = new Vector2(Mathf.Round(player.position.x), Mathf.Round(player.position.y));

            if (dangerZones.Contains(playerPos))
            {
                Debug.Log("BẠN ĐÃ ĐẠP VÀO Ô SÁNG! GAME OVER!");
                yield break; // Kết thúc Game
            }
            else
            {
                score += 10;
                Debug.Log("An toàn! Điểm hiện tại: " + score);
            }

            // 6. Hiển thị lại các ô đã sáng để chứng minh Player đứng đúng
            foreach (Vector2 pos in dangerZones)
            {
                tiles[pos].color = new Color(1f, 0.5f, 0.5f); // Đỏ nhạt
            }

            // Đợi 1 giây rồi bắt đầu lượt mới
            yield return new WaitForSeconds(1f);
        }
    }

    void ResetTileColors()
    {
        foreach (var kvp in tiles)
        {
            Vector2 pos = kvp.Key;
            // Thuật toán kiểm tra ô chẵn lẻ để làm màu caro
            bool isOffset = (pos.x + pos.y) % 2 == 1;
            kvp.Value.color = isOffset ? new Color(0.8f, 0.8f, 0.8f) : Color.white;
        }
    }
}