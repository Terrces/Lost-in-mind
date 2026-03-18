using System.Collections.Generic;
using UnityEngine;

public class PickUpPackageArea : Interactable
{
    public List<GameObject> objects;
    public Stage stage;
    public List<PackagesData> AllPackages;
    public Package CurrentPackage;
    
    private Interaction interact;
    private SceneProperties sceneProperties;
    public GameObject AreaGameObject;
    public int[] randMinutes = new int[2] {25, 35};
    int packageModelNumber = 0;


    void Awake()
    {
        Interacted += ObjectInteraction;
        if(!interact) interact = FindAnyObjectByType<Interaction>();
        sceneProperties = FindFirstObjectByType<SceneProperties>();
        newRandomNumber();
    }

    void newRandomNumber()
    {
        if(objects.Count > 1)
        {
            packageModelNumber = Random.Range(0,objects.Count);
        }
    }

    public void ObjectInteraction()
    {
        if(CurrentPackage || (stage.PackagesDelivered == stage.PackagesNeedForComplite)) return;
        
        GameObject _obj;

        Vector3 spawnPostition = interact.GetPointPosition();

        _obj = Instantiate(
            objects[packageModelNumber],
            spawnPostition,Quaternion.Euler(Vector3.zero),
            stage.transform);

        if(_obj.TryGetComponent(out Package package))
        {
            package.sceneProperties = sceneProperties;
            
            LocalTime time = new LocalTime();
            time.Hours = sceneProperties.SceneTime.Hours;
            time.Minutes = sceneProperties.SceneTime.Minutes;
            time.Seconds = sceneProperties.SceneTime.Seconds;
            time.Minutes = sceneProperties.SceneTime.Minutes + Random.Range(randMinutes[0], randMinutes[1]);
            
            package.TimeForDelivery = $"{time.GetHMTime()}";
            package.TimeOfCollected = sceneProperties.SceneTime.GetHMTime();

            package.RoomNumber = Random.Range(1,stage.maxRoomNumber+1);
            package.PackagesArea = this;
            stage.rooms[package.RoomNumber-1]._light.enabled = true;

            package.OnDelivered += PackageDelivered;
            package.OnDestroyed += PackageDestroyed;

            CurrentPackage = package;
        }

        interact.PickUpPhysicsObjects(_obj.GetComponent<PhysicalObject>());
        AreaGameObject.gameObject.layer = LayerMask.NameToLayer("Triggers");
    }

    public void AddPackageData(int number, PackageStatus status)
    {
        AreaGameObject.gameObject.layer = LayerMask.NameToLayer("Default");
        newRandomNumber();

        stage.PackagesDelivered += 1;
        PackagesData packageData = new PackagesData();
        packageData.Time = CurrentPackage.TimeOfCollected;
        packageData.stage = stage.StageNumber;
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
        if(CurrentPackage == null) return;
        CurrentPackage.status = PackageStatus.Destroyed;

        stage.rooms[CurrentPackage.RoomNumber-1]._light.enabled = false;
        AddPackageData(CurrentPackage.RoomNumber, PackageStatus.Destroyed);

        CurrentPackage = null;
    }
}
