using UnityEngine;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject initPrefab; // Array von Prefabs, die gesnapped werden sollen
    [SerializeField]
    private int initPrefabCount; 

    public GameObject targetObject; // Das Zielobjekt

    [SerializeField]
    private float spawnDistanceThreshold = 100.0f; // Maximale Entfernung zum Zielobjekt

    [SerializeField]
    private List<GameObject> spawnedObjects = new List<GameObject>(); // Liste der gespawnten Objekte

    private void Start()
    {
        // Erstelle das anf�ngliche Objekt, das immer als erstes spawnt
        for(int i = 0; i < initPrefabCount; i++)
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
            // �berwachen, ob das zuletzt gespawnte Objekt n�her als die Schwelle ist
            GameObject lastSpawnedObject = spawnedObjects[spawnedObjects.Count - 1];
            float distanceToTarget = Vector3.Distance(lastSpawnedObject.transform.position, targetObject.transform.position);

            if (Mathf.Round(distanceToTarget) < spawnDistanceThreshold)
            {
                // Wenn das letzte Objekt n�her als die Schwelle ist, w�hlen Sie das n�chste Objekt basierend auf Wahrscheinlichkeiten aus
                SpawnNextObject();
            }
        }

        // �berwachen und l�schen Sie gespawnte Objekte, die au�erhalb der Entfernungsschwelle sind
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
                // Wenn ein Objekt bereits zerst�rt wurde, aus der Liste entfernen
                spawnedObjects.RemoveAt(i);
            }
        }
    }


    private int ChooseRandomObjectIndex(List<ChildModuleInfo> possibleChildModules)
    {
        List<int> weightedIndices = new List<int>();

        // Erstellen Sie eine Liste von gewichteten Indizes basierend auf den Priorit�ten der ChildModuleInfo-Objekte
        int currentIndex = 0;
        foreach (ChildModuleInfo childInfo in possibleChildModules)
        {
            // F�gen Sie den Index nur hinzu, wenn die Priorit�t gr��er als 0 ist
            if (childInfo.Priority > 0)
            {
                for (int i = 0; i < childInfo.Priority; i++)
                {
                    weightedIndices.Add(currentIndex);
                }
            }
            currentIndex++;
        }

        // W�hlen Sie einen zuf�lligen Index aus der gewichteten Liste
        int randomIndex = weightedIndices[Random.Range(0, weightedIndices.Count)];
        return randomIndex;
    }

    private void SpawnNextObject(GameObject customPrefab = null)
    {
        if (spawnedObjects.Count == 0)
        {
            GameObject spawnedObject = Instantiate(initPrefab, Vector3.zero, Quaternion.Euler(0, 90, 0));
            spawnedObject.name = spawnedObject.name.Replace("(Clone)", "");
            spawnedObjects.Add(spawnedObject);
        }

        // Bestimmen Sie das Ground_Module, das gerade gespawnt wurde
        GameObject lastSpawnedObject = spawnedObjects[spawnedObjects.Count - 1];
        GroundModule lastGroundModule = lastSpawnedObject.GetComponent<GroundModule>();

        // Bestimmen Sie die m�glichen Child-Module f�r das gerade gespawnte Ground_Module
        List<ChildModuleInfo> possibleChildModules = lastGroundModule.ChildObjects;

        if (customPrefab != null)
        {
            // Wenn ein benutzerdefiniertes Prefab �bergeben wurde, spawnen Sie es zu 100%
            GameObject spawnedObject = Instantiate(customPrefab, lastGroundModule.GetAnchorPosition(), Quaternion.Euler(0, 90, 0));
            spawnedObject.name = spawnedObject.name.Replace("(Clone)", "");
            spawnedObjects.Add(spawnedObject);
        }
        else
        {
            // W�hlen Sie das n�chste Objekt basierend auf den Wahrscheinlichkeiten und spawnen Sie es
            int randomIndex = ChooseRandomObjectIndex(possibleChildModules);
            GameObject randomPrefab = possibleChildModules[randomIndex].childObject;
            GameObject spawnedObject = spawnedObject = Instantiate(randomPrefab, lastGroundModule.GetAnchorPosition(), Quaternion.Euler(0, 90, 0));
            spawnedObject.name = spawnedObject.name.Replace("(Clone)", "");
            spawnedObjects.Add(spawnedObject);
        }

        // Erstellen Sie eine zusammengefasste Debug-Nachricht
        string debugMessage = "Parent-Objekt: " + lastSpawnedObject.name + "\n";
        debugMessage += "Gespawntes Objekt: " + spawnedObjects[spawnedObjects.Count - 1].name + "\n";

        foreach (ChildModuleInfo childInfo in possibleChildModules)
        {
            debugMessage += "Child-Objekt: " + childInfo.childObject.name + ", Priorit�t: " + childInfo.Priority + "\n";
        }

        // Debug-Nachricht ausgeben
        Debug.Log(debugMessage);
    }
}
