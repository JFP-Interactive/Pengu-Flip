using UnityEngine;

public class GroundModule : MonoBehaviour
{
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