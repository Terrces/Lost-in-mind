using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhoneMenu : MonoBehaviour
{
    [SerializeField] private string packagesText;
    [SerializeField] private TextMeshProUGUI packagesGameObjectText;
    private bool allPackagesDelivered = false;

    private PickUpPackageArea pickUpPackage;
    private ChangeStage changeStage;

    private List<PackagesData> currentStagePackages = new List<PackagesData>();

    void Awake()
    {
        pickUpPackage = FindFirstObjectByType<PickUpPackageArea>();
        changeStage = FindFirstObjectByType<ChangeStage>();
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
}
