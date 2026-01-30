using DG.Tweening;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomNumber;
    public Transform PackagePoint;

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Package package))
        {
            if (package.RoomNumber == roomNumber)
            {
                package.Delivered();
                if (PackagePoint != null && package.TryGetComponent(out Rigidbody rb))
                {
                    FindFirstObjectByType<Interaction>().DropObject();
                    // rb.excludeLayers += LayerMask.NameToLayer("Player");
                    package.transform.DOMove(new Vector3(PackagePoint.position.x, package.transform.position.y, PackagePoint.position.z), 0.2f);
                    package.Interactable = false;
                }
                else
                {
                    Destroy(package.gameObject);
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        
    }

}
