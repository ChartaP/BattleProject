using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMng : MonoBehaviour
{
    public GameMng gameMng;
    // Start is called before the first frame update
    public bool CreateEnabled = true;
    public GameObject gTilePre = null;
    public int xSize =10;
    public int ySize =10;
    public MapTile[,] gTileList;

    private void Awake()
    {
        CreateMap();
        GeoEditing();
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
            for (ushort x = 0; x < xSize; x++)
            {
                for (ushort y = 0; y < ySize; y++)
                {
                    GameObject Temp = Instantiate(gTilePre, transform);
                    Temp.transform.localPosition = new Vector3(x, 0, y);
                    Temp.name = "(" + x + "," + y + ")" + "Tile";
                    gTileList[x, y] = Temp.GetComponent<MapTile>();
                    if (y != 0)
                    {
                        gTileList[x, y].SetNextTile(gTileList[x, y - 1], Direct.D_Down);
                    }
                    if (x != 0)
                    {
                        gTileList[x, y].SetNextTile(gTileList[x - 1, y], Direct.D_Left);
                    }
                }
            }
        }
    }

    private void GeoEditing()
    {
        foreach(MapTile tile in gTileList)
        {
            int level = Random.RandomRange(0, 1);
            tile.SetHeight(level * 0.25f);
        }
    }

    public void SetTileOpen(int x, int y,bool init)
    {
        gTileList[x, y].SetOpen(init);
    }

    public void SetTileOpen(Vector3Int pos, bool init)
    {
        gTileList[pos.x, pos.z].SetOpen(init);
    }


    public bool GetTileOpen(int x, int y)
    {
        return gTileList[x, y].GetOpen();
    }

    public bool GetTileOpen(Vector3Int pos)
    {
        return gTileList[pos.x, pos.z].GetOpen();
    }

    public bool isOutMapPos(int x, int y)
    {
        return (x < 0 || y < 0 || x >= xSize || y>= ySize);
    }

    public bool isOutMapPos(Vector3Int pos)
    {
        return (pos.x < 0 || pos.z < 0 || pos.x >= xSize || pos.z >= ySize);
    }

    public Vector3Int FindApproachTile(Vector3Int orgPos, Vector3Int tgtPos)
    {
        int size = 0,cnt = 0;

        PriorityQueue OpenTile = new PriorityQueue();
        PriorityQueue ClosedTile = new PriorityQueue();
        ANode tempPos = null;
        int maxstep =xSize*ySize;

        Vector3Int prePos = Vector3Int.zero;
        ANode preNode = null;
        OpenTile.EnQueue(new ANode(null, tgtPos,0, 0));
        while (OpenTile.Count() > 0)
        {
            tempPos = OpenTile.DeQueue();
            ClosedTile.EnQueue(tempPos);

            if(GetTileOpen( tempPos.GetPos()))
            {
                return tempPos.GetPos();
            }
            cnt = 0;
            do
            {
                size++;
                cnt++;
                for (int x = -size; x <= size; x++)
                {
                    for (int y = -size; y <= size; y++)
                    {
                        prePos.Set(tgtPos.x + x, 0, tgtPos.z + y);
                        if (ClosedTile.Contains(prePos))
                        {
                            continue;
                        }
                        if (isOutMapPos(prePos))
                        {
                            continue;
                        }
                        if (!GetTileOpen(prePos))
                        {
                            continue;
                        }
                        preNode = new ANode(tempPos, prePos, size*10,Heuristic(orgPos, prePos, tgtPos));
                        OpenTile.EnQueue(preNode);
                    }
                }
                if(cnt > maxstep)
                {
                    break;
                }
            } while (OpenTile.Count() == 0);
        }

        
        return Vector3Int.zero;
    }
    private float Heuristic(Vector3 orgPos, Vector3 curPos,Vector3 tgtPos)
    {
        //return Mathf.Abs(curPos.x - orgPos.x) + Mathf.Abs(curPos.y - orgPos.y) + Mathf.Abs(tgtPos.x - curPos.x) + Mathf.Abs(tgtPos.y - curPos.y);
        //위에 것은 성능 쓰레기 distance가 빠름
        //유닛에서 이동할 타일의 거리 + 이동할 타일에서 실제 목적지의 거리
        return Vector3.Distance(curPos, tgtPos);
    }

}
