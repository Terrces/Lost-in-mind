using System;

public enum PackageStatus {Delivered, Destroyed, Delivering}

[Serializable]
public class PackagesData
{
    public int stage;
    public int PackageRoomNumber;
    public PackageStatus Status;
}