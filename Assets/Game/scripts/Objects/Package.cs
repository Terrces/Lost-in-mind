using System;

public class Package : PhysicalObject
{
    public PackageStatus status = PackageStatus.Delivering;
    public event Action OnDelivered;

    public int RoomNumber = 0;
    public PickUpPackageArea PackagesArea = null;

    public void Delivered()
    {
        OnDelivered?.Invoke();
        ToggleTakingDamage();
    }
}
