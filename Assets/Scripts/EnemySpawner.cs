using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxEnemies = 10;
    [SerializeField] private Tilemap targetTilemap;
    [SerializeField] private float spawnRadius = 10f;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private List<Vector3> validSpawnPositions = new List<Vector3>();

    private void Start()
    {
        PopulateValidSpawnPositions();
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        // Limpiar la lista de enemigos destruidos
        activeEnemies.RemoveAll(enemy => enemy == null);
    }

    private System.Collections.IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (activeEnemies.Count < maxEnemies)
            {
                // Generar posición aleatoria en el Tilemap
                Vector3 spawnPosition = GetRandomTilePosition();

                // Instanciar el enemigo
                GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                activeEnemies.Add(newEnemy);            }
            else
            {
                Debug.Log("Límite de enemigos alcanzado. Esperando...");
            }

            // Esperar el intervalo
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void PopulateValidSpawnPositions()
    {
        if (targetTilemap == null)
        {
            Debug.LogError("Target Tilemap no asignado. Asignando fallback al spawner.");
            validSpawnPositions.Add(transform.position);
            return;
        }

        BoundsInt bounds = targetTilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                if (targetTilemap.GetTile(cellPosition) != null)
                {
                    // Centrar en el tile (asumiendo tiles de 1x1)
                    Vector3 worldPosition = targetTilemap.CellToWorld(cellPosition) + new Vector3(0.5f, 0.5f, 0f);

                    // Verificar si está dentro del radio
                    if (Vector3.Distance(worldPosition, transform.position) <= spawnRadius)
                    {
                        validSpawnPositions.Add(worldPosition);
                    }
                }
            }
        }

        if (validSpawnPositions.Count == 0)
        {
            Debug.LogWarning("No se encontraron tiles válidos dentro del radio. Usando posición del spawner como fallback.");
            validSpawnPositions.Add(transform.position);
        }
    }

    private Vector3 GetRandomTilePosition()
    {
        if (validSpawnPositions.Count > 0)
        {
            int randomIndex = Random.Range(0, validSpawnPositions.Count);
            return validSpawnPositions[randomIndex];
        }
        else
        {
            return transform.position;
        }
    }
}