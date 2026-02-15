using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Transform point;
    public LayerMask interactionMask;
    public LayerMask PhysicsObjectExclude;

    public float pickedUpMoveObjectSpeed = 15;
    private PhysicalObject carriedObject = null;
    private Rigidbody carriedObjectRigidbodyComponent = null;
    public bool ObjectIsCarried = false;

    private Player player => GetComponent<Player>();
    private Inventory inventory => GetComponent<Inventory>();
    
    void FixedUpdate()
    {
        if (!carriedObject) return;

        Vector3 target = point.TransformPoint(carriedObject.offset);

        Vector3 delta = target - carriedObjectRigidbodyComponent.position;

        if (delta.sqrMagnitude > 4)
        {
            DropObject();
            return;
        }

        carriedObjectRigidbodyComponent.rotation = point.rotation * Quaternion.Euler(Vector3.forward);
        carriedObjectRigidbodyComponent.linearVelocity = delta * pickedUpMoveObjectSpeed;
    }

    public Vector3 GetRaycastHit()
    {
        Ray ray = new Ray(player.cameraTransform.position,player.cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionMask))
        {
            return hit.point;
        }
        return hit.point;
    }

    public Vector3 GetPointPosition() => point.position;

    public void TryInteract()
    {

        if (carriedObject != null)
        {
            DropObject(2f);
            return;
        }

        Ray ray = new Ray(player.cameraTransform.position,player.cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 2, interactionMask))
        {
            if (hit.collider.TryGetComponent(out Iinteractable interaction))
            {
                interaction.Interact();
            }
            if (hit.collider.TryGetComponent(out PhysicalObject physicsInteractable))
            {
                PickUpPhysicsObjects(physicsInteractable);
            }
        }
    }

    public void PickUpPhysicsObjects(PhysicalObject physicsInteractable)
    {
        if(!physicsInteractable.Interactable) return;
        ObjectIsCarried = true;

        inventory.HideItem();

        carriedObject = physicsInteractable;
        carriedObjectRigidbodyComponent = carriedObject.GetComponent<Rigidbody>();

        carriedObjectRigidbodyComponent.excludeLayers += PhysicsObjectExclude;
        carriedObjectRigidbodyComponent.collisionDetectionMode = CollisionDetectionMode.Continuous;
        carriedObjectRigidbodyComponent.interpolation = RigidbodyInterpolation.Interpolate;
        carriedObjectRigidbodyComponent.linearVelocity = Vector3.zero;
        carriedObjectRigidbodyComponent.freezeRotation = true;
        carriedObjectRigidbodyComponent.isKinematic = false;
        carriedObjectRigidbodyComponent.useGravity = false;
    }

    public void DropObject(float force = 0f)
    {
        if(!carriedObject) return;
        Vector3 dir = point ? point.forward : Vector3.zero;

        ObjectIsCarried = false;
        carriedObjectRigidbodyComponent.AddForce(dir * force, ForceMode.Impulse);

        carriedObjectRigidbodyComponent.excludeLayers -= PhysicsObjectExclude;
        carriedObjectRigidbodyComponent.useGravity = true;
        carriedObjectRigidbodyComponent.freezeRotation = false;
        carriedObjectRigidbodyComponent.isKinematic = false;

        carriedObjectRigidbodyComponent = null;
        carriedObject = null;

        inventory.RestoreItem();
    }
}
