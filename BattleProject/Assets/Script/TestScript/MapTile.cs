using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Item;

public class MapTile : MonoBehaviour
{
    public MapMng myMapMng = null;
    public GameObject gBlock;
   
    private List<Block> blockList = new List<Block>();
    [SerializeField]
    private int nHeight = 0;
    private int nX;
    private int nY;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void SetPos(int X,int Y)
    {
        nX = X;
        nY = Y;
    }

    public void StackBlock(List<byte> Stratum)
    {
        for(int z = 0; z < Height; z++)
        {
            GameObject gBlock = Instantiate(this.gBlock, transform);
            gBlock.transform.localPosition = new Vector3(0, z, 0);
            Block block = new Block(BlockMng.Instance.Block((int)Stratum[z]),gBlock);
            blockList.Add(block);
        }
    }

    public void optimizeBlock()
    {
        int nU = myMapMng.GetHeight(nX, nY+1);
        int nD = myMapMng.GetHeight(nX, nY-1);
        int nL = myMapMng.GetHeight(nX+1, nY);
        int nR = myMapMng.GetHeight(nX-1, nY);

        int lower = Mathf.Min(nHeight, nU,nD,nL,nR);
        if (nX == 0 || nY == 0 || nX == GameInfo.nXSize || nY == GameInfo.nYSize)
            lower = 0;

        for(int z = 0; z < lower-1; z++)
        {
            blockList[z].MeshEnabled(false);
        }

        for(int z = lower; z < nHeight; z++)
        {
            blockList[z].MeshEnabled(true);
        }
    }
    public int Height
    {
        get { return nHeight; }
        set { nHeight = value; }
    }
}
