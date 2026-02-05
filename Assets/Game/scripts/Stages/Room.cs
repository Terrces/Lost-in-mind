using System.Collections;
using DG.Tweening;
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
                if (PackagePoint != null && package.TryGetComponent(out Rigidbody rb))
                {
                    StartCoroutine(delivered(package, rb));
                }
                else
                {
                    Destroy(package.gameObject);
                }
            }
        }
    }

    IEnumerator delivered(Package package, Rigidbody rb)
    {
        FindFirstObjectByType<Interaction>().DropObject();
        package.Interactable = false;
        yield return null;
    }

    void OnTriggerExit(Collider other)
    {
        
    }

}
