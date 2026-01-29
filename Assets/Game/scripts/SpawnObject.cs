using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private Transform point;
    [SerializeField] private GameObject Prefab;

    [ContextMenu("Emit")]
    public void Emit()
    {
        if(Prefab == null) return;
        if(point != null)
            Instantiate(Prefab, point.transform.position, point.transform.rotation, point.transform);
        else
            Instantiate(Prefab, transform.position, transform.rotation, transform);
    }
}
