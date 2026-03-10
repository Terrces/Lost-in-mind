using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class PhoneMenuStatus
{
    public static int MenuID = 0;
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
    [SerializeField] private Button menuButton;
    [SerializeField] private GameObject[] menus;
    [SerializeField] private GUIPackageCard currentPackageCard;
    [SerializeField] private GameObject containerForDeliveredPackageCards;
    [SerializeField] private GameObject deliveredPackageCardPrefab;
    private GameObject currentMenu;

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

        for (int i = 0; i < menus.Length; i++)
        {
            if(i == PhoneMenuStatus.MenuID)
            {
                currentMenu = menus[PhoneMenuStatus.MenuID];
                menus[i].SetActive(true);
                continue;
            }
            menus[i].SetActive(false);
        }

        if (GetFullPackagesCount() == GetDeliveredPackagesOnThisPage())
        {
           allPackagesDelivered = true; 
        }
    }

    void OnGUI()
    {
        timeText.text = $"{sceneProperties.SceneTime.GetHMTime()} am";
        packagesGameObjectText.text = $"{packagesText}{GetFullPackagesCount()}/{GetDeliveredPackagesOnThisPage()}";

        if(pickUpPackage.CurrentPackage != null)
        {            
            currentPackageCard.RoomNumberText.text = $"Room: {pickUpPackage.CurrentPackage.RoomNumber}";
            Debug.Log(pickUpPackage.CurrentPackage.TimeForDelivery);
            currentPackageCard.TimeForDeliveryText.text = $"Time for delivery: {pickUpPackage.CurrentPackage.TimeForDelivery}";
        }
        else
        {
            currentPackageCard.RoomNumberText.text = $"";
            currentPackageCard.TimeForDeliveryText.text = $"";
        }
    }

    public void OpenNewMenu(int MenuIndex)
    {
        MenuIndex = Mathf.Min(MenuIndex,menus.Length);

        if(currentMenu != null) currentMenu.SetActive(false);

        if(MenuIndex == 0) menuButton.interactable = false;
        else menuButton.interactable = true;
        
        currentMenu = menus[MenuIndex];
        PhoneMenuStatus.MenuID = MenuIndex;

        currentMenu.SetActive(true);
        
    }

    int GetFullPackagesCount() => currentStagePackages.Count;
    int GetDeliveredPackagesOnThisPage() => changeStage.Stages[changeStage.currentStageNumber].PackagesNeedForComplite;
}
