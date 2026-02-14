using System;

public enum PackageStatus {Delivered, Destroyed, Delivering}

[Serializable]
public class PackagesData
{
    public int PackageRoomNumber;
    public PackageStatus Status;
}