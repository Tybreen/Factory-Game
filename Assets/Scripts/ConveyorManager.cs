using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorManager : MonoBehaviour
{

    MovementControl[] MC;

    // Start is called before the first frame update
    void Start()
    {
        MC = FindObjectsOfType<MovementControl>();

        foreach (MovementControl M in MC)
        {
            M.MoveLoop();
        }

        StartCoroutine(MoveUpdate());

    }



    IEnumerator MoveUpdate()
    {
        yield return new WaitForSeconds(0.2f);

        foreach (MovementControl M in MC)
        {
            M.MoveLoop();
        }

        StartCoroutine(MoveUpdate());
    }


}
