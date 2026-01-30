using System.Collections.Generic;
using UnityEngine;

public class PickUpPackageArea : MonoBehaviour, Iinteractable
{
    public List<GameObject> objects;
    public Stage stage;
    public List<PackagesData> AllPackages;
    public Package CurrentPackage;
    private int previouslyRoomNumber = 0;

    public InteractionObjectTypes types {get;set;} = InteractionObjectTypes.Object;

    public void Interact()
    {
        if(CurrentPackage || (stage.PackagesDelivered == stage.PackagesNeedForComplite)) return;
        
        GameObject _obj = Instantiate(objects[0],transform.position,Quaternion.Euler(Vector3.zero), FindAnyObjectByType<Stage>().transform); 
        Interaction interact = FindAnyObjectByType<Interaction>();

        if(_obj.TryGetComponent(out Package package))
        {
            package.RoomNumber = Random.Range(1,stage.maxRoomNumber+1);
            package.PackagesArea = this;

            package.OnDelivered += PackageDelivered;
            package.OnDestroyed += PackageDestroyed;

            CurrentPackage = package;

            previouslyRoomNumber = package.RoomNumber;
        }
        interact.PickUpPhysicsObjects(_obj.GetComponent<IPhysicsInteractable>());
    }

    public void AddPackageData(int number, PackageStatus status)
    {
        stage.PackagesDelivered += 1;
        PackagesData packageData = new PackagesData();
        packageData.PackageRoomNumber = number;
        packageData.Status = status;

        AllPackages.Add(packageData);
    }

    public void PackageDelivered()
    {
        CurrentPackage.status = PackageStatus.Delivered;
        AddPackageData(CurrentPackage.RoomNumber, PackageStatus.Delivered);
        CurrentPackage.OnDelivered -= PackageDelivered;
        CurrentPackage.OnDestroyed -= PackageDestroyed;
        
        CurrentPackage = null;
    }

    public void PackageDestroyed()
    {
        CurrentPackage.status = PackageStatus.Destroyed;

        AddPackageData(CurrentPackage.RoomNumber, PackageStatus.Destroyed);

        CurrentPackage = null;
    }
}
