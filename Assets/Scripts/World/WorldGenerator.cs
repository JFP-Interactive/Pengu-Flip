using UnityEngine;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject initPrefab;
    [SerializeField]
    private int initPrefabCount;

    [SerializeField]
    private GameObject cliffBoundaryPrefab; 

    [SerializeField]
    private Vector3 cliffBoundaryOffset = new Vector3(0f, -2f, -5f);

    [SerializeField]
    private float cliffBoundaryRandomScalePercentage = 10.0f;

    public GameObject targetObject;

    [SerializeField]
    private float spawnDistanceThreshold = 100.0f;

    [SerializeField]
    private Vector3 startSpawnPoint = Vector3.zero;

    [SerializeField]
    private List<GameObject> spawnedObjects = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < initPrefabCount; i++)
        {
            SpawnNextObject(initPrefab);
        }
    }

    private void Update()
    {
        if (spawnedObjects.Count == 0)
        {
            SpawnNextObject(initPrefab);
        }
        else
        {
            GameObject lastSpawnedObject = spawnedObjects[spawnedObjects.Count - 1];
            float distanceToTarget = Vector3.Distance(lastSpawnedObject.transform.position, targetObject.transform.position);

            if (Mathf.Round(distanceToTarget) < spawnDistanceThreshold)
            {
                SpawnNextObject();
            }
        }

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
                spawnedObjects.RemoveAt(i);
            }
        }
    }

    private int ChooseRandomObjectIndex(List<ChildModuleInfo> possibleChildModules)
    {
        List<int> weightedIndices = new List<int>();
        int currentIndex = 0;
        foreach (ChildModuleInfo childInfo in possibleChildModules)
        {
            if (childInfo.Priority > 0)
            {
                for (int i = 0; i < childInfo.Priority; i++)
                {
                    weightedIndices.Add(currentIndex);
                }
            }
            currentIndex++;
        }
        int randomIndex = weightedIndices[Random.Range(0, weightedIndices.Count)];
        return randomIndex;
    }

    private void SpawnNextObject(GameObject customPrefab = null)
    {
        if (spawnedObjects.Count == 0)
        {
            GameObject spawnedObject = Instantiate(initPrefab, startSpawnPoint, Quaternion.Euler(0, 90, 0));
            spawnedObject.name = spawnedObject.name.Replace("(Clone)", "");
            spawnedObjects.Add(spawnedObject);

            // Spawn CliffBoundaryPrefab as child with offset position and random scale
            GameObject cliffBoundary = Instantiate(cliffBoundaryPrefab, spawnedObject.transform);
            cliffBoundary.transform.localPosition = cliffBoundaryOffset;
            ApplyRandomScale(cliffBoundary);
        }

        GameObject lastSpawnedObject = spawnedObjects[spawnedObjects.Count - 1];
        GroundModule lastGroundModule = lastSpawnedObject.GetComponent<GroundModule>();
        List<ChildModuleInfo> possibleChildModules = lastGroundModule.ChildObjects;

        if (customPrefab != null)
        {
            GameObject spawnedObject = Instantiate(customPrefab, lastGroundModule.GetAnchorPosition(), Quaternion.Euler(0, 90, 0));
            spawnedObject.name = spawnedObject.name.Replace("(Clone)", "");
            spawnedObjects.Add(spawnedObject);

            // Spawn CliffBoundaryPrefab as child with offset position and random scale
            GameObject cliffBoundary = Instantiate(cliffBoundaryPrefab, spawnedObject.transform);
            cliffBoundary.transform.localPosition = cliffBoundaryOffset;
            ApplyRandomScale(cliffBoundary);
        }
        else
        {
            int randomIndex = ChooseRandomObjectIndex(possibleChildModules);
            GameObject randomPrefab = possibleChildModules[randomIndex].childObject;
            GameObject spawnedObject = spawnedObject = Instantiate(randomPrefab, lastGroundModule.GetAnchorPosition(), Quaternion.Euler(0, 90, 0));
            spawnedObject.name = spawnedObject.name.Replace("(Clone)", "");
            spawnedObjects.Add(spawnedObject);

            // Spawn CliffBoundaryPrefab as child with offset position and random scale
            GameObject cliffBoundary = Instantiate(cliffBoundaryPrefab, spawnedObject.transform);
            cliffBoundary.transform.localPosition = cliffBoundaryOffset;
            ApplyRandomScale(cliffBoundary);
        }
    }

    private void ApplyRandomScale(GameObject obj)
    {
        // Berechnen Sie den zufälligen Skalierungsfaktor basierend auf dem Prozentsatz
        float randomScaleFactor = 1.0f + Random.Range(-cliffBoundaryRandomScalePercentage / 100.0f, cliffBoundaryRandomScalePercentage / 100.0f);

        // Setzen Sie die Skalierung des Objekts
        obj.transform.localScale *= randomScaleFactor;
    }
}
