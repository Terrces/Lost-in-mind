using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<GameObject> items = new List<GameObject>();
    public Transform Point;
    private GameObject currentItem;
    private int currentItemIdx;

    public void UseItem()
    {
        if (currentItem != null && currentItem.TryGetComponent(out Iusable use))
        {
            use.Use();
        }
    }

    public GameObject GetCurrentItem()
    {
        if(currentItem == null) return null;
        return currentItem;
    }

    public bool CheckAvailable(GameObject _item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != _item)
            {
                continue;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public void AddItem(GameObject _item)
    {
        if(!_item) return;
        
        items.Add(_item);

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == _item)
            {
                GetItem(i);
            }
        }


    }

    public void GetItem(int value)
    {
        currentItemIdx = value;
        currentItem = Instantiate(items[value],Point.transform);
    }

    public void RestoreItem()
    {
        if(currentItem == null && items[currentItemIdx] != null)
        {
            currentItem = Instantiate(items[currentItemIdx],Point.transform);
        }
    }

    public void HideItem()
    {
        if (currentItem.TryGetComponent(out Animator animator))
        {
            animator.Play("Hide");
            currentItem = null;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
