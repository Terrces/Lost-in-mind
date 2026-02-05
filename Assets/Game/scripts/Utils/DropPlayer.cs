using System.Collections;
using UnityEngine;

public class DropPlayer : MonoBehaviour
{

    bool enable = true;

    void OnCollisionEnter(Collision collision) => Drop(collision.collider);
    void OnCollisionStay(Collision collision) => Drop(collision.collider);
    void OnCollisionExit(Collision collision) => Drop(collision.collider);
    void OnTriggerEnter(Collider other) => Drop(other);

    public void Drop(Collider collider)
    {
        if (enable && collider.tag == "Player" && collider.TryGetComponent(out RigidbodyController rigidbodyPlayer) && !rigidbodyPlayer.RigidbodyIsActive)
        {
            enable = false;
            rigidbodyPlayer.SetRigidbodyActive();
            StartCoroutine(restore());
        }
    }
    public void Drop()
    {
        if (enable)
        {
            enable = false;
            StartCoroutine(restore());
        }
    }

    IEnumerator restore()
    {
        yield return new WaitForSeconds(4f);
        enable = true;
    }
}
