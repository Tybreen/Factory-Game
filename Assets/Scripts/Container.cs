using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MovementControl
{

    public List<ItemWorks> Items = new List<ItemWorks>();

    public List<StackOfItems> itemStacks = new List<StackOfItems>();

    public GameObject inventoryPrefab;

    private Inventory inventory;

    public int Capacity;

    public bool canStack = false;


    [System.Serializable]
    public class StackOfItems
    {
        public List<ItemWorks> itemStack = new List<ItemWorks>();
    }

    public void SpawnInventory(Transform t)
    {
        if (inventory != null) Destroy(inventory.gameObject);

        inventory = null;

        inventory = Instantiate(inventoryPrefab, t).GetComponent<Inventory>();

        inventory.PopulateInventory(this);
    }

    public void AddItem(ItemWorks Item)
    {
        if(canStack)
        {
            List<ItemWorks> listToAddTo = CheckStack(Item);
            if (listToAddTo != null && Item.maxStack > 0)
            {
                listToAddTo.Add(Item);
            }

            else
            {
                StackOfItems newStack = new StackOfItems();

                newStack.itemStack.Add(Item);

                itemStacks.Add(newStack);

                

            }

            if(inventory != null)
            {
                inventory.PopulateInventory(this);
            }
        }

        else
        {
            Items.Add(Item);
        }
    }

    public bool IsFull(ItemWorks item)
    {
        if (canStack)
        {
            if (itemStacks.Count >= Capacity)
            {
                print(CheckStack(item));
                return CheckStack(item) == null;
            }

            else return false;
        }

        else
        {
            if (Items.Count >= Capacity) return true;
            else return false;
        }
    }



    List<ItemWorks> CheckStack(ItemWorks item)
    {
        foreach (StackOfItems stack in itemStacks)
        {
            if (item.ItemName == stack.itemStack[0].ItemName)
            {
                if(item.maxStack > stack.itemStack.Count) return stack.itemStack;
            }
        }

        return null;
    }

}
