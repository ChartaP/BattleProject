using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Map;

public class MapMng : MonoBehaviour
{
    public GameMng gameMng;
    // Start is called before the first frame update
    public GameObject gTilePre = null;
    public List<GameObject> gPlantsPre = null;
    public GameObject gBuildPre = null;
    public int xSize =10;
    public int ySize =10;
    public MapTile[,] gTileList;
    public List<PlantsCtrl> gPlantsList;
    public List<Material> TileMaterials;
    public AnimationCurve curveX = AnimationCurve.Linear(0, 1, 1, 0);

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
        gTileList = new MapTile[xSize, ySize];
        gPlantsList = new List<PlantsCtrl>();
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject Temp = Instantiate(gTilePre, transform.Find("TileTrans"));
                Temp.transform.localPosition = new Vector3(x, 0, y);
                Temp.name = "(" + x + "," + y + ")" + "Tile";
                gTileList[x, y] = Temp.GetComponent<MapTile>();
            }
        }
        SetMap();
    }

    public void SetMap()
    {
        MapTable mapTable = GameSys.Map.CreateMap.Create(xSize, ySize, 0);
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                TileData curTile = mapTable.GetTile(x, y);
                int curType = curTile.GetTileType();
                int curPlant = curTile.GetPlants();
                if (curType > 0)
                    gTileList[x, y].buttomMesh.enabled = false;
                else
                    gTileList[x, y].buttomMesh.enabled = true;
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
                gTileList[x, y].SetHeight(curTile.GetHeight());
                if (curPlant != -1)
                {
                    GameObject Temp = Instantiate(gPlantsPre[curPlant], transform.Find("PlantsTrans"));
                    Temp.transform.position = gTileList[x, y].transform.Find("Quad").transform.position;
                    Temp.name = "(" + x + "," + y + ")" + "Plants";
                    gPlantsList.Add(Temp.GetComponent<PlantsCtrl>());
                }
                
            }
        }
    }

    public void ResetMap()
    {
        if (gPlantsList.Count != 0)
        {
            foreach (PlantsCtrl temp in gPlantsList)
            {
                Destroy(temp.gameObject);
            }
            gPlantsList.Clear();
        }
        if (gTileList.Length != 0)
        {
            foreach (MapTile temp in gTileList)
            {
                Destroy(temp.gameObject);
            }
            gTileList = new MapTile[xSize, ySize];
        }
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject Temp = Instantiate(gTilePre, transform.Find("TileTrans"));
                Temp.transform.localPosition = new Vector3(x, 0, y);
                Temp.name = "(" + x + "," + y + ")" + "Tile";
                gTileList[x, y] = Temp.GetComponent<MapTile>();
            }
        }
        SetMap();
    }
    
    public int GetHeight(int x,int y)
   {
       return gTileList[x, y].nHeight;
    }

}
