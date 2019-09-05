using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Map;

public class MapMng : MonoBehaviour
{
    public GameMng gameMng;
    // Start is called before the first frame update
    public bool CreateEnabled = true;
    public GameObject gTilePre = null;
    public List<GameObject> gPlantsPre = null;
    public GameObject gBuildPre = null;
    public int xSize =10;
    public int ySize =10;
    public MapTile[,] gTileList;
    public List<Material> TileMaterials;
    public AnimationCurve curveX = AnimationCurve.Linear(0, 1, 1, 0);

    private void Awake()
    {
        CreateMap();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateMap()
    {
        gTileList = new MapTile[xSize, ySize];
        if (CreateEnabled)
        {
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    GameObject Temp = Instantiate(gTilePre, transform);
                    Temp.transform.localPosition = new Vector3(x, 0, y);
                    Temp.name = "(" + x + "," + y + ")" + "Tile";
                    gTileList[x, y] = Temp.GetComponent<MapTile>();
                }
            }
            SetMap();
        }
    }

    public void SetMap()
    {
        MapTable mapTable = GameSys.Map.CreateMap.Create(xSize, ySize, 0);
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                int curType = mapTable.GetTile(x, y).GetTileType();
                gTileList[x, y].tileMesh.material = TileMaterials[curType];
                switch (curType)
                {
                    case 0:
                        gTileList[x, y].enableEffect.material.color = new Color(255, 255, 255);
                        break;
                    case 1:
                    case 2:
                        gTileList[x, y].enableEffect.material.color = new Color(0, 0, 0);
                        break;
                        
                }
                gTileList[x, y].SetHeight(mapTable.GetTile(x, y).GetHeight());
            }
        }
    }
    

}
