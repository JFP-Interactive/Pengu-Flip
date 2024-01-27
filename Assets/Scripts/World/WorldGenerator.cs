
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject[] prefabsToSpawn; // Array von Prefabs, die gesnapped werden sollen
    public float spawnInterval = 1.0f; // Intervall zwischen den Spawns

    private float timer = 0.0f;

    private void Start()
    {
        // Den ersten Spawn initiieren
        int randomIndex = Random.Range(0, prefabsToSpawn.Length);
        GameObject randomPrefab = prefabsToSpawn[randomIndex];
        SpawnRandomObject(Vector3.zero, randomPrefab);
    }

    private void Update()
    {
        // �berwachen der Zeit und ausl�sen des Spawns, wenn der Timer abgelaufen ist
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0.0f;

            // Den n�chsten Ankerpunkt als Position f�r das neue Objekt verwenden
            Vector3 nextAnchorPoint = GetLastSpawnedObjectAnchorPosition();

            // Ein zuf�lliges Objekt aus dem Array ausw�hlen und spawnen
            int randomIndex = Random.Range(0, prefabsToSpawn.Length);
            GameObject randomPrefab = prefabsToSpawn[randomIndex];
            SpawnRandomObject(nextAnchorPoint, randomPrefab);
        }
    }

    private Vector3 GetLastSpawnedObjectAnchorPosition()
    {
        GameObject[] spawnedObjects = GameObject.FindGameObjectsWithTag("lastSpawnedModule");
        if (spawnedObjects.Length > 0)
        {
            GameObject lastSpawnedObject = spawnedObjects[spawnedObjects.Length - 1];
            return lastSpawnedObject.GetComponent<GroundModule>().GetAnchorPosition();
        }
        return Vector3.zero;
    }

    private void SpawnRandomObject(Vector3 position, GameObject prefab)
    {
        GameObject spawnedObject = Instantiate(prefab, position, Quaternion.identity);

        spawnedObject.tag = "lastSpawnedModule";
    }
}