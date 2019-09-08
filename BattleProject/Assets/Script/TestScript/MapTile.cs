using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public MapMng myMapMng = null;
    public bool bOpen = true;
    public MeshRenderer tileMesh = null;
    public MeshRenderer buttomMesh = null;
    public MeshRenderer enableEffect = null;
    public Material buttom;

    // Start is called before the first frame update
    void Start()
    {
        myMapMng = transform.parent.GetComponent<MapMng>();
        int cnt = myMapMng.transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        enableEffect.enabled = bOpen;
    }
    

    public void SetOpen(bool active)
    {
        bOpen = active;
    }

    public bool GetOpen()
    {
        return bOpen;
    }

    public void SetHeight(float height)
    {
        transform.localScale = new Vector3(1,(height/4)+0.0001f, 1);
        if(height == 0)
        {
            buttomMesh.material = buttom;
        }
    }
}
