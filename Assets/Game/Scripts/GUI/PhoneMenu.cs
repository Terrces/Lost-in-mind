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
    private List<GameObject> packageCards = new List<GameObject>();

    void Awake()
    {
        pickUpPackage = FindFirstObjectByType<PickUpPackageArea>();
        changeStage = FindFirstObjectByType<ChangeStage>();
        sceneProperties = FindFirstObjectByType<SceneProperties>();

        //Adding Delivered packages in current stage
        foreach (PackagesData packagesData in pickUpPackage.AllPackages)
        {
            if(packagesData.stage == changeStage.currentStageNumber) currentStagePackages.Add(packagesData);
        }

        //Set start menu
        for (int i = 0; i < menus.Length; i++)
        {
            if(i == PhoneMenuStatus.MenuID)
            {
                MenuButtonToggle(i);
                currentMenu = menus[PhoneMenuStatus.MenuID];
                menus[i].SetActive(true);
                continue;
            }
            
            menus[i].SetActive(false);
        }

        //Checking all packages delivered?
        if (GetFullPackagesCount() == GetDeliveredPackagesOnThisPage()) allPackagesDelivered = true;

        deliveredPackagesUpdate();
    }

    void deliveredPackagesUpdate()
    {
        if(packageCards.Count != 0)
        {
            for (int i = 0; i < packageCards.Count; i++)
            {
                Destroy(packageCards[i]);
            }

            packageCards.Clear();
        }

        for (int i = 0; i < currentStagePackages.Count; i++)
        {
            GameObject container = Instantiate(deliveredPackageCardPrefab,Vector3.zero,quaternion.Euler(Vector3.zero),containerForDeliveredPackageCards.transform);
            packageCards.Add(container);
            GUIPackageCard packageCard = container.GetComponent<GUIPackageCard>();
            packageCard.RoomNumberText.text = $"{apartmentText}{currentStagePackages[i].PackageRoomNumber}";
            packageCard.TimeForDeliveryText.text = $"{timeForDeliveryText}{currentStagePackages[i].Time}";
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
            currentPackageCard.RoomNumberText.text = $"Pick up the package";
            currentPackageCard.TimeForDeliveryText.text = $"[Pick up the package]";
        }
    }

    private void MenuButtonToggle(int MenuIndex)
    {
        if(MenuIndex == 0) menuButton.interactable = false;
        else menuButton.interactable = true;
    }

    public void OpenMenu(int MenuIndex)
    {
        MenuIndex = Mathf.Min(MenuIndex,menus.Length);

        if(currentMenu != null) currentMenu.SetActive(false);

        MenuButtonToggle(MenuIndex);
        
        currentMenu = menus[MenuIndex];
        PhoneMenuStatus.MenuID = MenuIndex;

        currentMenu.SetActive(true);
    }

    int GetFullPackagesCount() => currentStagePackages.Count;
    int GetDeliveredPackagesOnThisPage() => changeStage.Stages[changeStage.currentStageNumber].PackagesNeedForComplite;
}
