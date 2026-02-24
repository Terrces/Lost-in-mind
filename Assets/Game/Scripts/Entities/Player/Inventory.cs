using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform Point;
    private GameObject currentItem;
    public bool HideCurrentItem;
    private int currentItemIdx;
    private int currentSlotIndex = 0;

    public MainGUI gui;

    void Start()
    {
        gui = FindFirstObjectByType<MainGUI>();
        GetItem();
    }

    public void GetItem(int value = 0)
    {
        if(value > InventoryHandler.Items.Count) value = 0;
        
        currentSlotIndex = value;

        int itemIndex = currentSlotIndex-1;

        if (currentSlotIndex == 0)
        {
            HideCurrentItem = true;
            HideItem();
        }
        else
        {
            HideCurrentItem = false;
            if (!GetComponent<Interaction>().ObjectIsCarried)
            {
                HideItem();
                SetItem(itemIndex);
            }
        }

        currentItemIdx = itemIndex;
        gui.SetItemIcon(currentSlotIndex);
    }

    public void ToggleItem() => GetItem(currentSlotIndex + 1);
    
    public void UseItem()
    {
        if (currentItem != null && currentItem.TryGetComponent(out Iusable use)) use.Use();
    }

    public void AddItem(GameObject _item = null, bool autoChoose = true)
    {
        if(!_item) return;
        
        bool addItemAvailable = true;

        for (int i = 0; i < InventoryHandler.Items.Count; i++)
        {
            if(InventoryHandler.Items[i] == _item)
            {
                addItemAvailable = false;
            }
        }

        if(addItemAvailable) InventoryHandler.Items.Add(_item);

        if (autoChoose)
        {
            for (int i = 0; i < InventoryHandler.Items.Count; i++)
            {
                if (InventoryHandler.Items[i].gameObject.name == _item.gameObject.name)
                {
                    GetItem(i+1);
                }
            }
        }
    }

    public void SetItem(int value)
    {
        currentItemIdx = value;
        currentItem = Instantiate(InventoryHandler.Items[value],Point.transform);
        if(HideCurrentItem) Destroy(currentItem);
    }

    public void RestoreItem()
    {
        if(InventoryHandler.Items.Count == 0 || HideCurrentItem) return;
        if(currentItem == null && InventoryHandler.Items[currentItemIdx] != null)
        {
            currentItem = Instantiate(InventoryHandler.Items[currentItemIdx],Point.transform);
        }
    }

    public void HideItem()
    {
        if (currentItem != null && currentItem.TryGetComponent(out Animator animator))
        {
            animator.Play("Hide");
            currentItem = null;
        }
        else
        {
            Destroy(currentItem);
            currentItem = null;
        }
    }

    /// <summary>
    /// Function for animation player
    /// </summary>
    public void Destroy() => Destroy(gameObject);
}
