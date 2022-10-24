using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorTracker : MonoBehaviour
{

    public Container Con;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Container C = collision.GetComponent<Container>();

        if (C) Con = C;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
