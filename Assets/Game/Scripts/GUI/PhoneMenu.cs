using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
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
    [Header("Text's")]
    [SerializeField] private string apartmentText = "Apartment number: ";
    [SerializeField] private string timeForDeliveryText = "Time for delivery: ";
    
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
            if(packagesData.stage == changeStage.currentStageNumber) currentStagePackages.Add(packagesData);
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

        if (GetFullPackagesCount() == GetDeliveredPackagesOnThisPage()) allPackagesDelivered = true;

        for (int i = 0; i < currentStagePackages.Count; i++)
        {
            GameObject container = Instantiate(deliveredPackageCardPrefab,Vector3.zero,quaternion.Euler(Vector3.zero),containerForDeliveredPackageCards.transform);
            GUIPackageCard packageCard = container.GetComponent<GUIPackageCard>();
            packageCard.RoomNumberText.text = $"{apartmentText}{currentStagePackages[i]}";
            packageCard.TimeForDeliveryText.text = $"{timeForDeliveryText}{currentStagePackages[i]}";
        }
    }

    void OnGUI()
    {
        timeText.text = $"{sceneProperties.SceneTime.GetHMTime()} am";
        packagesGameObjectText.text = $"{packagesText}{GetFullPackagesCount()}/{GetDeliveredPackagesOnThisPage()}";

        if(pickUpPackage.CurrentPackage != null)
        {            
            currentPackageCard.RoomNumberText.text = $"{apartmentText}{pickUpPackage.CurrentPackage.RoomNumber}";
            currentPackageCard.TimeForDeliveryText.text = $"{timeForDeliveryText}{pickUpPackage.CurrentPackage.TimeForDelivery}";
        }
        else
        {
            currentPackageCard.RoomNumberText.text = $"";
            currentPackageCard.TimeForDeliveryText.text = $"";
        }
    }

    public void OpenMenu(int MenuIndex)
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
