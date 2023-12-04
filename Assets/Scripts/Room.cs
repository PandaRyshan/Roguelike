using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Door Objects")]
    public Transform northDoor;
    public Transform southDoor;
    public Transform eastDoor;
    public Transform westDoor;

    [Header("Wall Objects")]
    public Transform northWall;
    public Transform southWall;
    public Transform eastWall;
    public Transform westWall;

    [Header("Values")]
    public int insideWidth;
    public int insideHeight;

    [Header("Prefabs")]
    public GameObject enemyPrefab;
    public GameObject coinPrefab;
    public GameObject healthPrefab;
    public GameObject keyPrefab;
    public GameObject exitDoorPrefab;

    private List<Vector3> usedPositions = new List<Vector3>();

    // to create coins, create enemies, create health, create key, create exit door
    public void GenerateInterior()
    {
        // do we spawn enemies?
        if (Random.value < Generation.instance.enemySpawnChance)
        {
            SpawnPrefab(enemyPrefab, 1, Generation.instance.maxEnemiesPerRoom + 1);
        }
        if (Random.value < Generation.instance.coiniSpawnChance)
        {
            SpawnPrefab(coinPrefab, 1, Generation.instance.maxCoinsPerRoom + 1);
        }
        if (Random.value < Generation.instance.healthSpawnChance)
        {
            SpawnPrefab(healthPrefab, 1, Generation.instance.maxHealthPerRoom + 1);
        }
    }

    // pre-set prefab of spawnable object, min and max number of objects to spawn
    public void SpawnPrefab(GameObject prefab, int min = 0, int max = 0)
    {
        int num = 1;
        if (min != 0 || max != 0)
        {
            num = Random.Range(min, max);
        }
        for (int x = 0; x < num; x++)
        {
            GameObject obj = Instantiate(prefab);
            Vector3 pos = transform.position + new Vector3(Random.Range(-insideWidth / 2, insideWidth / 2 + 1),
                    Random.Range(-insideHeight / 2, insideHeight / 2 + 1), 0);
            while (usedPositions.Contains(pos))
            {
                pos = transform.position + new Vector3(Random.Range(-insideWidth / 2, insideWidth / 2 + 1),
                    Random.Range(-insideHeight / 2, insideHeight / 2 + 1), 0);
            }
            obj.transform.position = pos;
            usedPositions.Add(pos);

            if (prefab == enemyPrefab)
            {
                EnemyManager.instance.enemies.Add(obj.GetComponent<Enemy>());
            }
        }
    }
}
