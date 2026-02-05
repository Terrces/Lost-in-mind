using System.Collections;
using DG.Tweening;
using UnityEngine;

public class RigidbodyController : MonoBehaviour
{
    public bool RigidbodyIsActive {get; private set;} = false;

    private CharacterController controller => GetComponent<CharacterController>();
    private Inventory inventory => GetComponent<Inventory>();
    private Interaction interaction => GetComponent<Interaction>();
    private Player player => GetComponent<Player>();
    public SphereCollider Collider {get; private set;}
    public Rigidbody Rigidbody {get; private set;}
    private Vector3 cameraStartPosition;

    private void Start() => cameraStartPosition = player.cameraTransform.localPosition;

    [ContextMenu("Set Active")]
    public void SetRigidbodyActive()
    {
        if(RigidbodyIsActive) return;
        
        RigidbodyIsActive = true;
        controller.enabled = false;
        player.movingAvailable = false;
        
        player.cameraTransform.DOLocalMove(Vector3.zero, 0.2f);

        interaction.DropObject(4f);
        inventory.HideItem();

        destroyComponents();
            
        Collider = gameObject.AddComponent<SphereCollider>();
        Rigidbody = gameObject.AddComponent<Rigidbody>();
        if(Rigidbody) Rigidbody.AddForce(Vector3.forward * 2);

        Collider.radius = 0.2f;
    }

    [ContextMenu("Set Disabled")]
    public void SetRigidbodyDisabled()
    {
        RigidbodyIsActive = false;
        controller.enabled = true;
        player.movingAvailable = true;

        player.cameraTransform.DOLocalMove(cameraStartPosition, 0.2f);
        player.transform.DORotate(new Vector3(0,transform.rotation.eulerAngles.y,0),0.1f);
        inventory.RestoreItem();

        destroyComponents();
    }

    private void destroyComponents()
    {
        if(Collider) 
            Destroy(Collider);
        if(Rigidbody) 
            Destroy(Rigidbody);
    }

    void OnCollisionStay(Collision collision)
    {
        if (RigidbodyIsActive) StartCoroutine(AutoStandUp());
    }

    IEnumerator AutoStandUp()
    {
        RigidbodyIsActive = false;
        yield return new WaitForSeconds(1f);
        SetRigidbodyDisabled();
    }

}
