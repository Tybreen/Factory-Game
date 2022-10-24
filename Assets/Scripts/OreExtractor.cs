using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreExtractor : MovementControl
{

    private Vector3 topLeftOffset = new Vector2(-1, 1);
    private Vector3 objectCenter;

    public int numOreDeposit;
    public int oreInStorage;
    public int maxStorage = 50;

    public int containerOn = 0;

    public ItemWorks ore;

    public List<Container> containers = new List<Container>();

    // Start is called before the first frame update
    void Start()
    {
        objectCenter = transform.GetChild(0).position;

        for (int i = 0; i < 8; i++) containers.Add(null);

        StartCoroutine(OreSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        numOreDeposit = 0;

        Collider2D[] list = Physics2D.OverlapAreaAll(objectCenter - topLeftOffset, objectCenter + topLeftOffset);

        if (list.Length > 0)
        {
            foreach (Collider2D collider in list)
            {
                ObjectTag tag = collider.gameObject.GetComponent<ObjectTag>();
                if(tag != null)
                {
                    if (tag.IronOreDeposit) numOreDeposit++; // later "Iron"
                }
            }
        }

        containers[0] = CheckContainer((objectCenter - transform.up * 1.2f) - transform.right * 0.1f, (objectCenter - transform.up * 1.3f) - transform.right * 0.2f);
        containers[1] = CheckContainer((objectCenter - transform.up * 1.2f) + transform.right * 0.1f, (objectCenter - transform.up * 1.3f) + transform.right * 0.2f);
        containers[2] = CheckContainer((objectCenter - transform.right * 1.2f) - transform.up * 0.1f, (objectCenter - transform.right * 1.3f) - transform.up * 0.2f);
        containers[3] = CheckContainer((objectCenter - transform.right * 1.2f) + transform.up * 0.1f, (objectCenter - transform.right * 1.3f) + transform.up * 0.2f);
        containers[4] = CheckContainer((objectCenter + transform.up * 1.2f) - transform.right * 0.1f, (objectCenter + transform.up * 1.3f) - transform.right * 0.2f);
        containers[5] = CheckContainer((objectCenter + transform.up * 1.2f) + transform.right * 0.1f, (objectCenter + transform.up * 1.3f) + transform.right * 0.2f);
        containers[6] = CheckContainer((objectCenter + transform.right * 1.2f) - transform.up * 0.1f, (objectCenter + transform.right * 1.3f) - transform.up * 0.2f);
        containers[7] = CheckContainer((objectCenter + transform.right * 1.2f) + transform.up * 0.1f, (objectCenter + transform.right * 1.3f) + transform.up * 0.2f);

    }

    IEnumerator OreSpawn()
    {
        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i < numOreDeposit; i++)
        {
            if (oreInStorage < maxStorage) oreInStorage++;
        }

        StartCoroutine(OreSpawn());
    }

    Container CheckContainer(Vector3 Pos1, Vector3 Pos2)
    {
        Collider2D[] list = Physics2D.OverlapAreaAll(Pos1, Pos2);

        if (list.Length > 0)
        {
            foreach (Collider2D collider in list)
            {
                Container Cont = collider.gameObject.GetComponent<Container>();

                if (Cont != null && Cont != this)
                {
                    Conveyor Conv = collider.gameObject.GetComponent<Conveyor>();

                    if (Conv != null)
                    { 
                        return Cont;
                       
                    }
                }
            }
        }

        return null;
    }

    public override void MoveLoop()
    {
        for (int i = 0; i < containers.Count; i++)
        {
            int Index = i + containerOn;
            Index %= containers.Count;

            if (oreInStorage > 0)
            {
                if (containers[Index] != null && containers[Index].Items[3].ItemName == "Null")
                {
                    containers[Index].Items[3] = ore;

                    oreInStorage--;
                }
            }
        }

        containerOn++;
        if (containerOn >= containers.Count) containerOn = 0;


    }
}
