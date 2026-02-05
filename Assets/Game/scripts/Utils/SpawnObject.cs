using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public bool Emited = false;
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

        Emited = true;
    }

    [ContextMenu("Destoy Objects")]
    public void DestoyObjects()
    {
        if(transform.childCount == 0) return;
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        Emited = false;
    }
}
