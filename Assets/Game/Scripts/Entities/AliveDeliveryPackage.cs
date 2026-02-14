using System.Collections;
using UnityEngine;

/// <summary>
/// Need Refactoring and rewrite to NavMesh
/// </summary>

public class AliveDeliveryPackage : Entity
{
    public float WaitTime = 5.0f;
    private Rigidbody rigidBody;
    private bool IsAlive = false;
    private Package package;
    private Interaction interaction;

    void Start()
    {
        TakingDamageIsAvailable = false;
        interaction = FindFirstObjectByType<Interaction>();
        
        // if(TryGetComponent(out Package _package))
        // {
        //     package = _package;
        //     StartCoroutine(WaitForSetAlive());
        // }
        // package.OnDelivered += StopAllCoroutines;
    }

    private IEnumerator WaitForSetAlive()
    {
        yield return new WaitForSeconds(WaitTime);

        interaction.DropObject();
        package.InvokeDestroy();
        package.status = PackageStatus.Destroyed;
        package.Interactable = false;
        SetAlive();
    }

    [ContextMenu("Set Alive")]
    public void SetAlive()
    {
        if (IsAlive) return;

        if(!TryGetComponent(out Rigidbody rb)) rigidBody = gameObject.AddComponent<Rigidbody>();
        else rigidBody = rb;

        rigidBody.freezeRotation = true;
        rigidBody.MoveRotation(Quaternion.Euler(Vector3.zero));

        IsAlive = true;
    }
    bool axis;
    void FixedUpdate()
    {
        if(!IsAlive) return;

        Ray ray = new Ray(transform.position,transform.forward);

        if(Physics.SphereCast(ray, transform.localScale.x, out RaycastHit hit, transform.localScale.z + 1f))
        {
            Quaternion turnRotation;
            if(axis)
                turnRotation = Quaternion.Euler(0f, 15f, 0f);
            else
                turnRotation = Quaternion.Euler(0f, -15f, 0f);
                
            rigidBody.MoveRotation(rigidBody.rotation*turnRotation);
            return;
        }

        rigidBody.AddForce(transform.forward * 1.5f, ForceMode.Impulse);
        axis = !axis;

    }
}
