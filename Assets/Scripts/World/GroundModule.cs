using UnityEngine;
using System.Collections.Generic;

public class GroundModule : MonoBehaviour
{
    [SerializeField]
    private List<ChildModuleInfo> childObjects = new List<ChildModuleInfo>(); // Liste der m�glichen Nachfolger-Module

    public List<ChildModuleInfo> ChildObjects
    {
        get { return childObjects; }
    }

    // Ein �ffentlicher Ankerpunkt f�r dieses Objekt
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
    public GameObject childObject; // Das m�gliche Nachfolger-Modul
    public int priority = 1; // Priorit�t des Nachfolger-Moduls (Bereich von 1-10)

    public int Priority
    {
        get { return priority; }
        set { priority = Mathf.Clamp(value, 0, 100); }
    }
}
