using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Transform Point;
    public LayerMask interactionMask;
    public LayerMask PhysicsObjectExclude;

    public float pickedUpMoveObjectSpeed = 15;
    private GameObject carriedObject = null;
    private Rigidbody carriedObjectRigidbodyComponent = null;

    private Player player => GetComponent<Player>();
    private Inventory inventory => GetComponent<Inventory>();
    
    void FixedUpdate()
    {
        if (!carriedObject) inventory.RestoreItem();
        if (!carriedObject) return;

        Vector3 target = Point.TransformPoint(new Vector3(0,0,1));
        Vector3 delta = target - carriedObjectRigidbodyComponent.position;

        if (delta.sqrMagnitude > 25)
        {
            DropObject();
            return;
        }

        carriedObjectRigidbodyComponent.rotation = Point.rotation * Quaternion.Euler(Vector3.forward);
        carriedObjectRigidbodyComponent.linearVelocity = delta * pickedUpMoveObjectSpeed;
    }

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
            if (hit.collider.TryGetComponent(out IPhysicsInteractable physicsInteractable))
            {
                PickUpPhysicsObjects(physicsInteractable);
            }
        }
    }

    public void PickUpPhysicsObjects(IPhysicsInteractable physicsInteractable)
    {
        if(!physicsInteractable.Interactable) return;

        inventory.HideItem();

        carriedObject = physicsInteractable.Interact();
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
        Vector3 dir = Point ? Point.forward : Vector3.zero;

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
