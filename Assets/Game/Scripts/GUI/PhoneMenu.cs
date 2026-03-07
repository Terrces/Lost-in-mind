using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class PhoneMenuStatus
{
    public enum PhoneStates {MENU,TASKS,PAUSE}
    public static PhoneStates PhoneState; 
}

public class PhoneMenu : MonoBehaviour
{
    [SerializeField] private string packagesText;

    [Header("Label's")]
    [SerializeField] private TextMeshProUGUI packagesGameObjectText;
    [SerializeField] private TextMeshProUGUI timeText;
    
    private bool allPackagesDelivered = false;

    private PickUpPackageArea pickUpPackage;
    private ChangeStage changeStage;

    private List<PackagesData> currentStagePackages = new List<PackagesData>();

    private SceneProperties sceneProperties;
    public GameObject[] menus;

    void Awake()
    {
        pickUpPackage = FindFirstObjectByType<PickUpPackageArea>();
        changeStage = FindFirstObjectByType<ChangeStage>();
        sceneProperties = FindFirstObjectByType<SceneProperties>();
        
        foreach (PackagesData packagesData in pickUpPackage.AllPackages)
        {
            if(packagesData.stage == changeStage.currentStageNumber)
            {
                currentStagePackages.Add(packagesData);
            }
        }
        int deliveredPackagesCount = currentStagePackages.Count;
        
        int allPackagesCountOnThisStage = changeStage.Stages[changeStage.currentStageNumber].PackagesNeedForComplite;

        packagesGameObjectText.text = $"{packagesText}{deliveredPackagesCount}/{allPackagesCountOnThisStage}";
        
        if (deliveredPackagesCount == allPackagesCountOnThisStage)
        {
           allPackagesDelivered = true; 
        }
    }

    void OnGUI()
    {
        timeText.text = $"{sceneProperties.SceneTime.GetHMTime()} am";
    }
}
