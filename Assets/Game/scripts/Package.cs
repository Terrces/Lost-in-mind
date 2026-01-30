using System;
using UnityEngine;

public class Package : Destroyable, IPhysicsInteractable
{
    public PackageStatus status = PackageStatus.Delivering;
    public event Action OnDelivered;

    public int RoomNumber = 0;
    public PickUpPackageArea PackagesArea = null;
    public bool Interactable {get; set;} = true;

    public GameObject Interact()
    {
        return gameObject;
    }

    public void Delivered()
    {
        OnDelivered?.Invoke();
        ToggleTakingDamage();
    }
}
