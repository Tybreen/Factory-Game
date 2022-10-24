using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MovementControl
{

    public Container Cont;
    public Container depositContainer;

    public List<Conveyor> ConveyorLine = new List<Conveyor>();

    public SpriteRenderer[] spriteRen = new SpriteRenderer[4];
    public SpriteRenderer[] backSpriteRen = new SpriteRenderer[4];
    public SpriteRenderer[] rightSpriteRen = new SpriteRenderer[4];
    public SpriteRenderer[] leftSpriteRen = new SpriteRenderer[4];

    public ItemWorks NullItem;

    public bool HasFront;
    public bool HasBack;
    public bool IsFront;

    public bool hasDeposit;

    public Conveyor FrontConveyor;
    public Conveyor BackConveyor;

    private void Start()
    {
        Cont = GetComponent<Container>();

        UpdateConveyors();
    }

    public void UpdateConveyors()
    {
        Debug.Log("reached");
        bool back;
        bool right;
        bool left;

        IsFront = !CheckConveyors(true, transform.position - transform.up * 0.6f, transform.position - transform.up * 1.3f, false);
        back = CheckConveyors(false, transform.position + transform.up * 0.6f, transform.position + transform.up * 1.3f, false);
        left = CheckConveyors(false, transform.position - transform.right * 0.6f, transform.position - transform.right * 1.3f, true);
        right = CheckConveyors(false, transform.position + transform.right * 0.6f, transform.position + transform.right * 1.3f, true);

        for (int i = 0; i < 4; i++)
        {
            if (Cont.Items[i] == null) Cont.Items[i] = NullItem;
        }

        if (back) spriteRen = backSpriteRen;
        else if (left) spriteRen = leftSpriteRen;
        else if (right) spriteRen = rightSpriteRen;

        if (IsFront)
        {
            Collider2D[] list = Physics2D.OverlapAreaAll(transform.position - transform.up * 0.6f, transform.position - transform.up * 1.3f);

            if (list.Length > 0)
            {
                foreach (Collider2D collider in list)
                {
                    Container container = collider.gameObject.GetComponent<Container>();

                    if (container != null && container != Cont)
                    {
                        hasDeposit = true;
                        depositContainer = container;
                    }
                }
            }
            ConveyorLine.Clear();
            ConveyorLine.Add(this);
            bool Ended = false;
            int ConveyorOn = 0;

            while(!Ended)
            {
                if (ConveyorLine[ConveyorOn].BackConveyor != null)
                {
                    ConveyorLine.Add(ConveyorLine[ConveyorOn].BackConveyor);
                }

                else Ended = true;

                ConveyorOn++;
            }
        }
    }

    bool CheckConveyors(bool IsFront, Vector3 Pos1, Vector3 Pos2, bool isSide)
    {
        Collider2D[] list = Physics2D.OverlapAreaAll(Pos1, Pos2);

        if(list.Length > 0)
        {
            foreach(Collider2D collider in list)
            {
                Conveyor Conv = collider.gameObject.GetComponent<Conveyor>();

                if(Conv != null && Conv != this)
                {
                    if(IsFront)
                    {
                        HasFront = true;
                        FrontConveyor = Conv;
                        return true;
                    }

                    else
                    {
                        if(Conv.transform.rotation != transform.rotation
                            && Conv.transform.rotation.eulerAngles != transform.rotation.eulerAngles + new Vector3(0, 0, 180)
                            && isSide)
                        {
                            if(Vector3.Angle(Conv.transform.forward, transform.position - Conv.transform.position) < 5)
                            {
                                HasBack = true;
                                BackConveyor = Conv;
                                return true;
                            }
                        }

                        else if(!isSide)
                        {
                            HasBack = true;
                            BackConveyor = Conv;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        else
        {
            return false;
        }
    }

    public override void MoveLoop()
    {
        foreach(Conveyor c in ConveyorLine)
        {
            for(int i = 0; i < 4; i++)
            {
                if(c.Cont.Items[i].ItemName != "Null")
                {
                    if(i == 0)
                    {
                        if(c.IsFront)
                        {
                            if (hasDeposit)
                            {
                                if (!depositContainer.IsFull(c.Cont.Items[0]))
                                {
                                    depositContainer.AddItem(c.Cont.Items[0]);

                                    c.Cont.Items[0] = NullItem;
                                }
                            }
                        }

                        else if(c.FrontConveyor.Cont.Items[3].ItemName == "Null")
                        {
                            c.FrontConveyor.Cont.Items[3] = c.Cont.Items[0];
                            c.Cont.Items[i] = NullItem;
                        }
                    }

                    else if(c.Cont.Items[i - 1].ItemName == "Null")
                    {
                        c.Cont.Items[i - 1] = c.Cont.Items[i];
                        c.Cont.Items[i] = NullItem;
                    }
                }
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            spriteRen[i].sprite = Cont.Items[i].WorldSprite;
        }
    }
}
