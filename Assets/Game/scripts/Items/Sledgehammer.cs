using System.Collections;
using UnityEngine;

public class Sledgehammer : MonoBehaviour,Iusable
{
    private Animator Animation;

    private bool IsAttacked;

    [SerializeField] private float waitTime = 0.7f;
    [SerializeField] private float maxDistance = 2;

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(waitTime);
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 15f))
        {
            if(!IsAttacked && Vector3.Distance(hit.collider.transform.position, transform.parent.position) < maxDistance)
            {
                IsAttacked = true;

                if (hit.collider.TryGetComponent(out Rigidbody rigidbody))
                {
                    Vector3 direction = (hit.collider.transform.position - transform.position).normalized;

                    rigidbody.AddForce(direction * 15, ForceMode.Impulse);
                }

                if(hit.collider.TryGetComponent(out Idamageable idamageable))
                {
                    idamageable.TakeDamage(1);
                    yield return null;
                }
            }
        }
    }

    public void Use()
    {
        if (TryGetComponent(out Animator attackAnimation))
        {
            Animation = attackAnimation;
            attackAnimation.SetBool("Attack", true);
            IsAttacked = false;
            StartCoroutine(Attack());

        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void AttackEnd()
    {
        Animation.SetBool("Attack", false);
    }
}
