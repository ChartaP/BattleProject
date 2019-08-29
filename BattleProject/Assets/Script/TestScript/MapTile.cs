using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direct
{
    D_Up = 0,
    D_Down = 1,
    D_Left = 2,
    D_Right = 3
};

public class MapTile : MonoBehaviour
{
    public MapMng myMapMng = null;
    public MapTile[] NextTile = new MapTile[4];
    public bool bOpen = true;
    public MeshRenderer enableEffect = null;

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

    public void SetNextTile(MapTile nextTile,Direct dir=Direct.D_Down)
    {
        switch (dir)
        {
            case Direct.D_Up:
                NextTile[(int)dir] = nextTile;
                nextTile.NextTile[(int)Direct.D_Down] = this;
                break;
            case Direct.D_Down:
                NextTile[(int)dir] = nextTile;
                nextTile.NextTile[(int)Direct.D_Up] = this;
                break;
            case Direct.D_Left:
                NextTile[(int)dir] = nextTile;
                nextTile.NextTile[(int)Direct.D_Right] = this;
                break;
            case Direct.D_Right:
                NextTile[(int)dir] = nextTile;
                nextTile.NextTile[(int)Direct.D_Left] = this;
                break;
        }
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
        transform.localScale = new Vector3(1,height+0.0001f, 1);
    }
}
