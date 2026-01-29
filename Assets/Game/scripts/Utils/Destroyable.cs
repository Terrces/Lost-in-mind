using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public int Health = 1;
    [SerializeField] private GameObject DestroyedModel;
    
    public void TakingDamage(int value)
    {
        Health -= value;

        if (Health <= 0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
        if (DestroyedModel != null)
        {
            Instantiate(DestroyedModel,transform.position,transform.rotation);
        }
    }
}
