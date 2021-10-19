using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    List<Vector3> cheeseSpawnLocations;

    [SerializeField]
    int amountCheeseToSpawn = 10;

    [SerializeField]
    GameObject cheesePrefab;

    List<GameObject> spawnedCheeseObjects;

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
        if (cheeseSpawnLocations == null || cheeseSpawnLocations.Count == 0)
        {
            Debug.LogWarning("Could not spawn any cheese because no spawn locations are given");
            return;
        }
        List<Vector3> locations = new List<Vector3>(cheeseSpawnLocations);

        for(int i = 0; i < amountCheeseToSpawn; i++)
        {
            if(locations.Count == 0)
            {
                Debug.Log("could not spawn cheese #" + i + ", there were not enough locations provided");
            }

            int locationIndex = Random.Range(0, locations.Count);
            GameObject result = TrySpawnAt(cheesePrefab, locations[locationIndex], 0);
            if (result != null) {
                result.GetComponent<Cheese>().SetWasSpawned(true);
                spawnedCheeseObjects.Add(result);
            }
        }
    }

    private void DespawnAllGameObjects(List<GameObject> listToDespawn)
    {
        for (int i = listToDespawn.Count - 1; i >= 0; i--)
        {
            Destroy(listToDespawn[i]);
            listToDespawn.RemoveAt(i);
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

    public void SetCheeseAmountToSpawn(int amountCheeseToSpawn)
    {
        this.amountCheeseToSpawn = amountCheeseToSpawn;
    }

    private GameObject TrySpawnAt(GameObject prefab, Vector3 location, float checkRange)
    {
        if(checkRange == 0)
        {
            return Instantiate(prefab, location, Quaternion.identity);
        }

        //TODO: check if there is space in the specified location
        return Instantiate(prefab, location, Quaternion.identity);
    }

}
