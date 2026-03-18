using System;
using UnityEngine;

[SelectionBase]
public class Package : PhysicalObject
{
    public PackageStatus status = PackageStatus.Delivering;
    public SceneProperties sceneProperties;
    public string TimeForDelivery;
    public string TimeOfCollected;
    public event Action OnDelivered;

    public int RoomNumber = 0;
    public PickUpPackageArea PackagesArea = null;

    void Awake() => OnDestroyed += Destroyed;

    public void Destroyed()
    {
        status = PackageStatus.Destroyed;
        Interactable = false;
    }

    public void Delivered()
    {
        OnDelivered?.Invoke();
        ToggleTakingDamage();
    }

    private void Attack()
    {
        
    }

    void LateUpdate()
    {
        if (sceneProperties.SceneTime == null) return;
        if (TimeForDelivery == sceneProperties.SceneTime.GetHMTime())
        {
            Attack();
        }
    }
}
