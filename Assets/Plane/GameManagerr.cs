using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerr : MonoBehaviour
{
    [Header("Difficulty Settings")]
    [SerializeField] private int startingDangerTiles = 5; 
    [SerializeField] private int tilesIncreasePerRound = 2; 
    [SerializeField] private int maxDangerTiles = 60; 

    private int currentDangerTiles; 
    [SerializeField] private Transform player;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int gridSize = 8;

    // normal chess
    private Dictionary<Vector2, SpriteRenderer> tiles = new Dictionary<Vector2, SpriteRenderer>();
    // Lava
    private List<Vector2> dangerZones = new List<Vector2>();

    private int score = 0;

    void Start()
    {
        GenerateChessboard();
        currentDangerTiles = startingDangerTiles;
        StartCoroutine(GameLoop()); 
    }

    void GenerateChessboard()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector2 pos = new Vector2(x, y);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity);

                // chessboard
                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                tiles.Add(pos, sr);
            }
        }
        ResetTileColors(); //reset board
    }

    IEnumerator GameLoop()
    {
        Debug.Log("Game bắt đầu sau 3 giây...");
        yield return new WaitForSeconds(3f);

        while (true) 
        {
            dangerZones.Clear();
            ResetTileColors();

            
            //random range
            while (dangerZones.Count < currentDangerTiles)
            {
                Vector2 randomPos = new Vector2(Random.Range(0, gridSize), Random.Range(0, gridSize));

                                if (!dangerZones.Contains(randomPos))
                {
                    dangerZones.Add(randomPos);
                    tiles[randomPos].color = new Color(1f, 0.5f, 0.5f); // Sáng Đỏ
                }
            }

            
            yield return new WaitForSeconds(1f);

            
            ResetTileColors();

            
            Debug.Log("Move, bro");
            yield return new WaitForSeconds(1.5f);

            // score check
            Vector2 playerPos = new Vector2(Mathf.Round(player.position.x), Mathf.Round(player.position.y));

            if (dangerZones.Contains(playerPos))
            {
                Debug.Log("GAME OVER!");
                yield break; 
            }
            else
            {
                // increase difficulty
                currentDangerTiles += tilesIncreasePerRound;
                currentDangerTiles = Mathf.Min(currentDangerTiles, maxDangerTiles);
                score += 10;
                Debug.Log("Safe, Point: " + score);
            }

            
            foreach (Vector2 pos in dangerZones)
            {
                tiles[pos].color = Color.red; 
            }

            
            yield return new WaitForSeconds(1f);
        }
    }

    void ResetTileColors()
    {
        foreach (var kvp in tiles)
        {
            Vector2 pos = kvp.Key;
            
            bool isOffset = (pos.x + pos.y) % 2 == 1;
            kvp.Value.color = isOffset ? new Color(0.8f, 0.8f, 0.8f) : Color.white;
        }
    }
}