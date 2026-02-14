using System;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public event Action OnDestroyed;
    public bool AvaliableTakingDamage = true;
    public int Health = 1;
    [SerializeField] private GameObject DestroyedModel;
    [SerializeField] private GameObject Items;
    [SerializeField] private int repeatCountForDestroyedModel = 1;
    
    public void TakingDamage(int value)
    {
        if(!AvaliableTakingDamage) return;

        Health -= value;

        if (Health <= 0) Destroy();
    }

    public void ToggleTakingDamage() => AvaliableTakingDamage = !AvaliableTakingDamage;

    public void InvokeDestroy()
    {
        OnDestroyed?.Invoke();
        AvaliableTakingDamage = false;
    }

    public void Destroy()
    {
        OnDestroyed?.Invoke();

        Destroy(gameObject);

        if (Items != null) Instantiate(Items,transform.position,transform.rotation,transform.parent);

        if (DestroyedModel != null)
        {
            for (int i = 0; i < repeatCountForDestroyedModel; i++) Instantiate(DestroyedModel,transform.position,transform.rotation,transform.parent);
        }
    }
}
