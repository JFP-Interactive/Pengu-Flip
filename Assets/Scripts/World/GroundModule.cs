using UnityEngine;
using System.Collections.Generic;

public class GroundModule : MonoBehaviour
{
    [SerializeField]
    private List<ChildModuleInfo> childObjects = new List<ChildModuleInfo>(); // Liste der möglichen Nachfolger-Module

    public List<ChildModuleInfo> ChildObjects
    {
        get { return childObjects; }
    }

    // Ein öffentlicher Ankerpunkt für dieses Objekt
    public Transform anchorPoint;

    public Vector3 GetAnchorPosition()
    {
        if (anchorPoint != null)
        {
            return anchorPoint.position;
        }
        return transform.position; // Wenn kein Ankerpunkt zugewiesen ist, verwenden Sie die Position des Objekts selbst
    }
}

[System.Serializable]
public class ChildModuleInfo
{
    public GameObject childObject; // Das mögliche Nachfolger-Modul
    public int priority = 1; // Priorität des Nachfolger-Moduls (Bereich von 1-10)

    public int Priority
    {
        get { return priority; }
        set { priority = Mathf.Clamp(value, 0, 100); }
    }
}
