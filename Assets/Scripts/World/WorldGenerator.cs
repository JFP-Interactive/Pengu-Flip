using UnityEngine;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour
{
    public GameObject[] prefabsToSpawn; // Array von Prefabs, die gesnapped werden sollen
    public GameObject targetObject; // Das Zielobjekt
    public float spawnDistanceThreshold = 30.0f; // Maximale Entfernung zum Zielobjekt

    [SerializeField]
    private List<GameObject> spawnedObjects = new List<GameObject>(); // Liste der gespawnten Objekte

    private void Start()
    {
        // Den ersten Spawn initiieren
        SpawnObject(Vector3.zero);
    }

    private void Update()
    {
        if (spawnedObjects.Count == 0)
        {
            // Wenn keine Objekte gespawnt wurden, spawnen Sie das erste Objekt bei 0/0/0
            SpawnObject(Vector3.zero);
        }
        else
        {
            // Überwachen, ob das zuletzt gespawnte Objekt näher als die Schwelle ist
            GameObject lastSpawnedObject = spawnedObjects[spawnedObjects.Count - 1];
            float distanceToTarget = Vector3.Distance(lastSpawnedObject.transform.position, targetObject.transform.position);

            if (distanceToTarget <= spawnDistanceThreshold)
            {
                // Wenn das letzte Objekt näher als die Schwelle ist, spawnen Sie ein neues Objekt am Ankerpunkt des letzten Objekts
                SpawnObject(lastSpawnedObject.GetComponent<GroundModule>().GetAnchorPosition());
            }
        }

        // Überwachen und löschen Sie gespawnte Objekte, die außerhalb der Entfernungsschwelle sind
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            GameObject spawnedObject = spawnedObjects[i];
            if (spawnedObject != null)
            {
                float distanceToTarget = Vector3.Distance(spawnedObject.transform.position, targetObject.transform.position);
                if (distanceToTarget > spawnDistanceThreshold)
                {
                    Destroy(spawnedObject);
                    spawnedObjects.RemoveAt(i);
                }
            }
            else
            {
                // Wenn ein Objekt bereits zerstört wurde, aus der Liste entfernen
                spawnedObjects.RemoveAt(i);
            }
        }
    }

    private void SpawnObject(Vector3 position)
    {
        int randomIndex = Random.Range(0, prefabsToSpawn.Length);
        GameObject randomPrefab = prefabsToSpawn[randomIndex];
        GameObject spawnedObject = Instantiate(randomPrefab, position, Quaternion.identity);
        spawnedObjects.Add(spawnedObject); // Gespawntes Objekt zur Liste hinzufügen
    }
}
