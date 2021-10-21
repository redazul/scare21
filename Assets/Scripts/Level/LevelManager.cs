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

    [SerializeField]
    private List<SpawnArea> catSpawnAreas;

    [SerializeField]
    private GameObject catPrefab;

    [SerializeField]
    private List<SpawnArea> trapSpawnAreas;

    [SerializeField]
    private GameObject trapPrefab;

    private List<GameObject> spawnedCheeseObjects;
    private List<GameObject> spawnedCatObjects;
    private List<GameObject> spawnedTrapObjects;

    public static LevelManager Instance = null;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one LevelManager in this scene.");
        }
        Instance = this;

        spawnedCheeseObjects = new List<GameObject>();
        spawnedCatObjects = new List<GameObject>();
        spawnedTrapObjects = new List<GameObject>();
    }

    void Start()
    {
        SpawnAllSpawnables();
    }

    public void CleanAndRespawnAll()
    {
        DespawnAllSpawnables();
        SpawnAllSpawnables();
    }

    public void SpawnAllSpawnables()
    {
        //cheese
        int amountCheeseToSpawn = ScoreManager.GetCheeseAmountNeeded();
        SpawnMultiplePrefabs(amountCheeseToSpawn, cheesePrefab, cheeseSpawnAreas, spawnedCheeseObjects, checkObstructionLayerMask);

        //cats
        int amountCatsToSpawn = 2;
        SpawnMultiplePrefabs(amountCatsToSpawn, catPrefab, catSpawnAreas, spawnedCatObjects, checkObstructionLayerMask);

        //traps
        int amountTrapsToSpawn = 3;
        SpawnMultiplePrefabs(amountTrapsToSpawn, trapPrefab, trapSpawnAreas, spawnedTrapObjects, checkObstructionLayerMask);
    }

    private void DespawnAllSpawnables()
    {
        DespawnAllGameObjects(spawnedCheeseObjects);
        DespawnAllGameObjects(spawnedCatObjects);
        DespawnAllGameObjects(spawnedTrapObjects);
    }

    public void DespawnSingleCheese(GameObject cheeseObjectToDespawn)
    {
        spawnedCheeseObjects.RemoveAll(c => c == cheeseObjectToDespawn);
        Destroy(cheeseObjectToDespawn);
    }


    private static void DespawnAllGameObjects(List<GameObject> listToDespawn)
    {
        for (int i = listToDespawn.Count - 1; i >= 0; i--)
        {
            Destroy(listToDespawn[i]);
            listToDespawn.RemoveAt(i);
        }
    }

    private static void SpawnMultiplePrefabs(int amountToSpawn, GameObject prefab, List<SpawnArea> spawnAreas, List<GameObject> managingList, LayerMask obstructionMask)
    {
        if (spawnAreas == null || spawnAreas.Count == 0)
        {
            Debug.LogWarning("Could not spawn any cheese because no spawn locations are given");
            return;
        }

        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject result = TrySpawningPrefab(spawnAreas, prefab, obstructionMask);
            if (result != null)
            {
                result.GetComponent<ISpawnable>().SetSpawned(true);
                managingList.Add(result);
            }
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
