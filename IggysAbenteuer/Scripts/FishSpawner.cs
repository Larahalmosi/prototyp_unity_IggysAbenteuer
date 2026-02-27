using UnityEngine;
using System.Collections.Generic;
public class FishSpawner : MonoBehaviour
{
    [Header("Fisch-Prefabs")]
    public GameObject[] fishPrefabs;
     
    [Header("Spawn-Bereich")]
    public Vector2 spawnAreaMin = new Vector2(-8f, -4f);
    public Vector2 spawnAreaMax = new Vector2(8f, 4f);
    
    [Header("Fisch-Verhalten")]
    public int fishCount = 15;
    public float moveSpeed = 2f;
    public float changeDirectionInterval = 3f;
    
    private List<GameObject> spawnedFish = new List<GameObject>();
    
    void Start()
    {
        SpawnFish();
    }
    
    void SpawnFish()
    {for (int i = 0; i < fishCount; i++) 
        {
            Vector2 spawnPos = new Vector2(
                Random.Range(spawnAreaMin.x + 1f, spawnAreaMax.x - 1f),
                Random.Range(spawnAreaMin.y + 0.5f, spawnAreaMax.y - 0.5f)
            );
       // Zufälliges Prefab aus Array
            GameObject randomFish = fishPrefabs[Random.Range(0, fishPrefabs.Length)];
            GameObject fish = Instantiate(randomFish, spawnPos, Quaternion.identity);
            
            spawnedFish.Add(fish);

            // Bewegung direkt zuweisen
            FishAI ai = fish.AddComponent<FishAI>();
            ai.spawner = this;
        }
    }
    
    // Öffentlich für FishAI
    public Vector2 GetSwimBoundsMin() => spawnAreaMin;
    public Vector2 GetSwimBoundsMax() => spawnAreaMax;
    public float GetMoveSpeed() => moveSpeed;
    public float GetChangeDirectionInterval() => changeDirectionInterval;
}