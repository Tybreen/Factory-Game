using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : Container
{

    public ItemWorks NullItem;

    public List<Container> containers = new List<Container>();

    int containerOn;

    Container front;
    Container back;
    Container right;
    Container left;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++) containers.Add(null);

    }

    // Update is called once per frame
    void Update()
    {



        containers[0] = CheckContainer(transform.position - transform.up * 0.6f, transform.position - transform.up * 1.3f);
        containers[1] = CheckContainer(transform.position + transform.up * 0.6f, transform.position + transform.up * 1.3f);
        containers[2] = CheckContainer(transform.position - transform.right * 0.6f, transform.position - transform.right * 1.3f);
        containers[3] = CheckContainer(transform.position + transform.right * 0.6f, transform.position + transform.right * 1.3f);


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
                        if (!Conv.IsFront)
                        {
                            return Cont;
                        }
                    }

                    else
                    {
                        return Cont;
                    }
                }
            }
        }

        return null;
    }
    // front 0, right 1, back 2, left 3;
    public override void MoveLoop()
    {
        for(int i = 0; i < containers.Count; i++)
        {
            int Index = i + containerOn;
            Index %= containers.Count;

            if(Items.Count > 0)
            {
                if (containers[Index] != null && containers[Index].Items[3].ItemName == "Null")
                {
                    containers[Index].Items[3] = Items[0];
                    Items.RemoveAt(0);
                }
            }
        }

        containerOn++;
        if (containerOn >= containers.Count) containerOn = 0;


    }

}
