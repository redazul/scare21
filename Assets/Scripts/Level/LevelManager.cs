using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private const float SPAWN_MARGIN = 0.1f;

    private const int NUM_RETRIES_FOR_SPAWNING = 10;

    [SerializeField]
    private LayerMask checkObstructionLayerMask = 0;

    [SerializeField]
    private List<SpawnArea> cheeseSpawnAreas;

    [SerializeField]
    private GameObject cheesePrefab;

    private List<GameObject> spawnedCheeseObjects;

    public static LevelManager Instance = null;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one LevelManager in this scene.");
        }
        Instance = this;

        spawnedCheeseObjects = new List<GameObject>();

    }

    void Start()
    {
        SpawnAllCheese();
    }

    private void SpawnAllCheese()
    {
        int amountCheeseToSpawn = ScoreManager.GetCheeseAmountNeeded();

        if (cheeseSpawnAreas == null || cheeseSpawnAreas.Count == 0)
        {
            Debug.LogWarning("Could not spawn any cheese because no spawn locations are given");
            return;
        }

        for (int i = 0; i < amountCheeseToSpawn; i++)
        {
            GameObject result = TrySpawningPrefab(cheeseSpawnAreas, cheesePrefab, checkObstructionLayerMask);
            if (result != null)
            {
                result.GetComponent<Cheese>().SetWasSpawned(true);
                spawnedCheeseObjects.Add(result);
            }
        }

    }

    public void DespawnSingleCheese(GameObject cheeseObjectToDespawn)
    {
        spawnedCheeseObjects.RemoveAll(c => c == cheeseObjectToDespawn);
        Destroy(cheeseObjectToDespawn);
    }

    public void CleanAndRespawnCheese()
    {
        DespawnAllGameObjects(spawnedCheeseObjects);
        SpawnAllCheese();
    }

    private static void DespawnAllGameObjects(List<GameObject> listToDespawn)
    {
        for (int i = listToDespawn.Count - 1; i >= 0; i--)
        {
            Destroy(listToDespawn[i]);
            listToDespawn.RemoveAt(i);
        }
    }

    private static GameObject TrySpawningPrefab(List<SpawnArea> spawnAreas, GameObject prefab, LayerMask checkObstructionMask)
    {
        Vector3 prefabScale = prefab.transform.lossyScale; //warning - this is not very accurate
        float maxRadius = Mathf.Max(prefabScale.x * 2, prefabScale.z * 2) + SPAWN_MARGIN;

        for (int r = 0; r < NUM_RETRIES_FOR_SPAWNING; r++)
        {
            int locationIndex = Random.Range(0, spawnAreas.Count);

            GameObject result = TrySpawningPrefabAt(prefab, spawnAreas[locationIndex].GetRandomPointWithin(), maxRadius, checkObstructionMask);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    private static GameObject TrySpawningPrefabAt(GameObject prefab, Vector3 location, float checkRange, LayerMask checkObstructionMask)
    {
        if (checkRange == 0)
        {
            return Instantiate(prefab, location, Quaternion.identity);
        }

        Collider[] colliders = Physics.OverlapSphere(location, checkRange, checkObstructionMask);//2 is purely chosen arbitrarly
        if (colliders.Length == 0)
        {
            return Instantiate(prefab, location, Quaternion.identity);
        }

        return null;
    }

}
