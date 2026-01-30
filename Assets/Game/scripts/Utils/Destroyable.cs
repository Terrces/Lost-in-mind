using System;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public event Action OnDestroyed;
    public bool AvaliableTakingDamage = true;
    public int Health = 1;
    [SerializeField] private GameObject DestroyedModel;
    
    public void TakingDamage(int value)
    {
        if(!AvaliableTakingDamage) return;

        Health -= value;

        if (Health <= 0)
        {
            Destroy();
        }
    }

    public void ToggleTakingDamage() => AvaliableTakingDamage = !AvaliableTakingDamage;

    public void Destroy()
    {
        OnDestroyed?.Invoke();

        Destroy(gameObject);
        if (DestroyedModel != null)
        {
            Instantiate(DestroyedModel,transform.position,transform.rotation,transform.parent);
        }
    }
}
