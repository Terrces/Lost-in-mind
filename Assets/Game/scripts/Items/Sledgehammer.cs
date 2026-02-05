using UnityEngine;

public class Sledgehammer : MonoBehaviour,Iusable
{
    private Animator Animation;

    private bool IsAttacked;

    [SerializeField] private float maxDistance = 1.5f;

    public void Attack()
    {
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

                if(hit.collider.TryGetComponent(out Destroyable destroyable))
                {
                    destroyable.TakingDamage(1);
                    return;
                }
            }
        }
    }

    public void Use()
    {
        if (IsAttacked) return;
        if (TryGetComponent(out Animator attackAnimation))
        {
            Animation = attackAnimation;
            Animation.Play("Attack");
            IsAttacked = false;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void AttackEnd()
    {
        IsAttacked = false;
    }
}
