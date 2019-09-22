using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Item;

public class ItemObj : MonoBehaviour
{
    private byte id;
    // Start is called before the first frame update

    public void Set( byte id , Vector3 pos )
    {
        transform.parent = transform.Find("ItemTrans");
        this.id = id;
        transform.localPosition = pos;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public byte ID
    {
        get { return id; }
    }
}
