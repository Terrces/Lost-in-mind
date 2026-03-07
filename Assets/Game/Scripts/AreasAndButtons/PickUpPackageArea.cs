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
    public int[] randMinutes = new int[2] {25, 35};

    void Awake()
    {
        Interacted += ObjectInteraction;
        if(!interact) interact = FindAnyObjectByType<Interaction>();
        sceneProperties = FindFirstObjectByType<SceneProperties>();
    }

    public void ObjectInteraction()
    {
        if(CurrentPackage || (stage.PackagesDelivered == stage.PackagesNeedForComplite)) return;
        
        GameObject _obj;

        Vector3 spawnPostition = interact.GetPointPosition();

        if(objects.Count == 1)
        {
            _obj = Instantiate(
                objects[0],
                spawnPostition,Quaternion.Euler(Vector3.zero),
                stage.transform);
        }
        else
        {
            _obj = Instantiate(
                objects[Random.Range(0,objects.Count)],
                spawnPostition,Quaternion.Euler(Vector3.zero),
                stage.transform);
        }

        if(_obj.TryGetComponent(out Package package))
        {
            package.sceneProperties = sceneProperties;
            
            LocalTime time = new LocalTime();
            time.Hours = sceneProperties.SceneTime.Hours;
            time.Minutes = sceneProperties.SceneTime.Minutes;
            time.Seconds = sceneProperties.SceneTime.Seconds;

            time.Minutes = Random.Range(randMinutes[0], randMinutes[1]) + sceneProperties.SceneTime.Minutes;
            
            package.TimeForDelivery = $"{time.GetHMTime()}";
            package.TimeOfCollected = sceneProperties.SceneTime.GetHMTime();

            Debug.Log(package.TimeForDelivery);
            
            package.RoomNumber = Random.Range(1,stage.maxRoomNumber+1);
            package.PackagesArea = this;
            stage.rooms[package.RoomNumber-1]._light.enabled = true;

            package.OnDelivered += PackageDelivered;
            package.OnDestroyed += PackageDestroyed;

            CurrentPackage = package;
        }

        interact.PickUpPhysicsObjects(_obj.GetComponent<PhysicalObject>());
        gameObject.layer = LayerMask.NameToLayer("Triggers");
    }

    public void AddPackageData(int number, PackageStatus status)
    {
        gameObject.layer = LayerMask.NameToLayer("Default");

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
