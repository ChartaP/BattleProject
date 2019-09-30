using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Map;
using GameSys.Lib;
using GameSys.Item;

public class MapMng : MonoBehaviour
{
    public GameMng gameMng;
    // Start is called before the first frame update
    public GameObject gTilePre = null;
    public List<GameObject> gPlantsPre = null;
    public List<GameObject> gBuildPre = null;
    public Dictionary<int,MapTile> MapTiles;
    public List<Material> BlockMaterials;
    public bool[,] bOpen;

    private void Awake()
    {
        
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateMap()
    {
        MapTiles = new Dictionary<int, MapTile>();
        SetMap();
    }
    public void SetMap()
    {
        MapTable mapTable = GameSys.Map.CreateMap.Create(GameInfo.nXSize, GameInfo.nYSize, 0);
        bOpen = new bool[GameInfo.nXSize, GameInfo.nYSize];
        for(int x = 0; x < GameInfo.nXSize; x++)
        {
            for (int y = 0; y < GameInfo.nYSize; y++)
            {
                bOpen[x,y] = true;
            }
        }
        foreach (TileData tile in mapTable.TileList)
        {
            GameObject gTile = Instantiate(gTilePre, transform.Find("TileTrans"));
            gTile.transform.localPosition = new Vector3(tile.X, 0, tile.Y);
            MapTile cTile = gTile.GetComponent<MapTile>();
            cTile.SetPos(tile.X,tile.Y);
            cTile.myMapMng = this;
            cTile.Height = tile.Height;
            cTile.StackBlock(tile.Stratum);
            MapTiles.Add((tile.X  << 8)|tile.Y, cTile);
        }

        foreach(int key in MapTiles.Keys)
        {
            MapTiles[key].optimizeBlock();
        }
    }
    public void ResetMap()
    {
        MapTiles.Clear();

        SetMap();
    }
    
    public int GetHeight(int x,int y)
    {
        if (x < 0 || y < 0 || x >= GameInfo.nXSize || y >= GameInfo.nYSize)
        {
            return 256;
        }
        return MapTiles[(x << 8) | y].Height;
    }

}
