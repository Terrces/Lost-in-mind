using UnityEngine;

public class WoodenBox : Destroyable, Idamageable, IPhysicsInteractable
{

    public GameObject Interact()
    {
        return gameObject;
    }
    public void TakeDamage(int value)
    {
        TakingDamage(value);
    }
}
