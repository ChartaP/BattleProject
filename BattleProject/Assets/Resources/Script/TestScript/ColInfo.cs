using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColInfo : MonoBehaviour
{
    public bool isTrigger;
    private void OnTriggerEnter(Collider other)
    {
        isTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isTrigger = false;
    }
}
