using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity Characteristics")]
    public int HealthPoints = 5;
    public event Action TakingDamage;
    public event Action IsRegeneration;
    public bool TakingDamageIsAvailable = true;
    public bool RegenerationIsAvailable = false;
    public bool IsDead {get; private set;} = false;
    
    public void TakeDamage(int damage)
    {
        if (HealthPoints <= 0 || IsDead || !TakingDamageIsAvailable) return;
        TakingDamage.Invoke();

        HealthPoints -= math.abs(damage);

        if (HealthPoints <= 0)
        {
            IsDead = true;
        }
    }

    private IEnumerator Regeneration()
    {
        IsRegeneration.Invoke();
        yield return null;
    }
}
