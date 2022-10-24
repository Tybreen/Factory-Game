using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public Container container;

    public GameObject slot;

    public Transform background;

    public List<ContainerSlot> slots = new List<ContainerSlot>();

    public void PopulateInventory(Container c)
    {
        container = c;

        List<GameObject> objectsToKill = GetChildren(background);

        for (int i = 0; i < objectsToKill.Count; i++)
        {
            Destroy(objectsToKill[i]);
        }

        slots.Clear();

        for(int i = 0; i < container.Capacity; i++)
        {
            slots.Add(Instantiate(slot, background).GetComponent<ContainerSlot>());
        }

        for(int i = 0; i < container.itemStacks.Count; i++)
        {
            slots[i].icon.sprite = container.itemStacks[i].itemStack[0].WorldSprite;

            slots[i].stack.text = container.itemStacks[i].itemStack.Count.ToString();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    List<GameObject> GetChildren(Transform t)
    {
        List<GameObject> children = new List<GameObject>();

        for (int i = 0; i < t.childCount - 1; i++)
        {
            children.Add(t.GetChild(i).gameObject);
        }

        return children;
    }

}
