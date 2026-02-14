using System.Collections;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomNumber;
    public Transform PackagePoint;
    public Light _light;

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Package package))
        {
            if (package.RoomNumber == roomNumber)
            {
                package.Delivered();
                _light.enabled = false;
                if (PackagePoint != null && package.TryGetComponent(out Rigidbody rigidbody))
                {
                    StartCoroutine(delivered(package));
                }
                else
                {
                    Destroy(package.gameObject);
                }
            }
        }
    }

    IEnumerator delivered(Package package)
    {
        FindFirstObjectByType<Interaction>().DropObject();
        package.Interactable = false;
        yield return null;
    }
}
