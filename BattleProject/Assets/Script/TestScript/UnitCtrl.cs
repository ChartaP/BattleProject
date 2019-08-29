using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ANode 
{
    private Vector3Int position;
    private int cnt;
    private float weight;
    private ANode parent;

    public ANode()
    {
        position = Vector3Int.zero;
        weight = 0;
    }

    public ANode(ANode parent, Vector3Int pos, int cnt, float weight)
    {
        this.parent = parent;
        position = pos;
        this.cnt = cnt;
        this.weight = weight;
    }
    public ANode GetParent()
    {
        return parent;
    }

    public int GetCnt() { return cnt; }

    public Vector3Int GetPos() { return position; }
    public float GetWeight() { return weight; }

    public static bool operator >(ANode A,ANode B)
    {
        return A.weight + A.cnt > B.weight + B.cnt;
    }
    public static bool operator <(ANode A, ANode B)
    {
        return A.weight + A.cnt < B.weight + B.cnt;
    }
    public static bool operator >=(ANode A, ANode B)
    {
        return A.weight + A.cnt >= B.weight + B.cnt;
    }
    public static bool operator <=(ANode A, ANode B)
    {
        return A.weight + A.cnt <= B.weight + B.cnt;
    }
};

/// <summary>
/// 우선순위 큐 , 출처 : https://gist.github.com/trevordixon/10401462
/// </summary>
public class PriorityQueue
{
    private List<ANode> queue;

    public PriorityQueue()
    {
        this.queue = new List<ANode>();
    }

    public void Sort()
    {
        int size = queue.Count;
        int index = 0;
        while (index < size-1)
        {
            for (int minus = size - 1; minus >= index; minus--)
            {
                if (queue[index] > queue[ minus])
                {
                    ANode temp = queue[ minus];
                    queue[minus] = queue[index];
                    queue[index] = temp;
                }
            }
            index++;
        }
    }

    public void EnQueue(ANode data)
    {
        this.queue.Add(data);
        this.Sort();
    }

    public ANode DeQueue()
    {
        ANode temp = queue[0];
        queue.Remove(temp);
        return temp;
    }

    public ANode Find(Vector3Int pos)
    {
        foreach(ANode node in queue)
        {
            if (node.GetPos().Equals(pos))
            {
                ANode temp = node;
                queue.Remove(node);
                return temp;
            }
        }
        Debug.Log("목표를 못찾았다!");
        return null;
    }

    public int Count()
    {
        return queue.Count;
    }

    public bool Contains(Vector3Int pos) 
    {
        foreach(ANode node in queue)
        {
            if (node.GetPos().Equals(pos))
            {
                return true;
            }
        }
        return false;
    }
}

    public class UnitCtrl : MonoBehaviour
{
    public UnitMng unitMng;
    public Vector3Int unitPos;
    public Vector3Int tgtPos;
    public numFace unitNum;
    public int unitID;
    public int playerID;
    public MeshRenderer SelectMesh;
    // Start is called before the first frame update

    private Stack<Vector3Int> wayPoint { get; set; }
    private Coroutine coroutionMove = null;
    private Coroutine coroutionSelect = null;
    void Start()
    {
        wayPoint = new Stack<Vector3Int>();
        unitPos.x = (int)transform.localPosition.x;
        unitPos.z = (int)transform.localPosition.z;
        unitMng.gameMng.mapMng.SetTileOpen(unitPos, false);
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// 이동 명령을 내리는 메서드
    /// 목표까지 wayPoint를 설정한다
    /// </summary>
    /// <param name="target"></param>
    public void ReceiveMoveOrder(Vector3Int target)
    {
        if (wayPoint != null)
        {
            if (wayPoint.Count != 0)//없으면 오류남
                wayPoint.Clear();
        }
        tgtPos = target;


        if (coroutionMove != null)
        {
            StopCoroutine(coroutionMove);
        }
        coroutionMove = StartCoroutine(MoveToPos());

    
    }

    public void ReceiveSelectOrder()
    {
        SelectMesh.enabled = true;
        if (coroutionSelect == null)
            coroutionSelect = StartCoroutine(SelectEffect());
    }

    public void UnableSelect()
    {
        SelectMesh.enabled = false;
        if (coroutionSelect != null)
            StopCoroutine(coroutionSelect);
        coroutionSelect = null;
    }

    /// <summary>
    /// 웨이포인트가 존재 할 때 웨이포인트의 첫번째 지점을 pop하여 불러와 그 지점까지 이동
    /// UnitPos를 바꿔주는 유일한 요소
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToPos()
    {
        int cnt = 0;
        if (wayPoint.Count == 0)
        {
            while (!SearchRoute(unitPos))
            {
                cnt++;
                if (cnt > 10)
                {
                    Debug.Log(transform.name + "찾기 포기");
                    yield break;
                }
                Debug.Log(transform.name + "다시 찾기 실패 재시도");
                yield return new WaitForSeconds(0.5f + (0.01f * unitID));
            }
            Debug.Log(transform.name + "찾기 성공!");
        }
        cnt = 0;
        while (!unitMng.gameMng.mapMng.GetTileOpen(wayPoint.Peek()))
        {
            if (cnt >= 10)
            {
                cnt = 0;
                Debug.Log(transform.name + "더는 못 기다린다");

                wayPoint.Clear();
                while(!SearchRoute(unitPos))
                {
                    cnt++;
                    if(cnt > 10)
                    {
                        Debug.Log(transform.name + "다시 찾기 포기");
                        yield break;
                    }
                    Debug.Log(transform.name + "다시 찾기 실패 재시도");
                    yield return new WaitForSeconds(0.5f + (0.01f * unitID));
                }
                cnt = 0;
                break;
            }
            else if(cnt == 0)
            {
                Debug.Log(transform.name + "앞에 누가 있음 기다림" + cnt);
            }
            yield return new WaitForSeconds(0.03f +( 0.0001f* unitID));
            cnt++;
        }
        Debug.Log(wayPoint.Peek());
        unitMng.gameMng.mapMng.SetTileOpen(unitPos, true);
        unitPos = wayPoint.Pop();
        unitMng.gameMng.mapMng.SetTileOpen(unitPos, false);

        while (true)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, unitPos, 0.1f);

            if (transform.localPosition == unitPos)
            {
                break;
            }
            yield return new WaitForSeconds(0.015f);
        }
        if (wayPoint.Count > 0)
        {
            coroutionMove= StartCoroutine(MoveToPos());
        }
        else
        {
            tgtPos = unitPos;
            yield break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="curPos">현재 읽고 있는 타일 위치</param>
    /// <param name="CloseTile">현재 검색 중인 경로에서 갔던 길</param>
    /// <returns></returns>
    public bool SearchRoute(Vector3Int curPos)
    {
        PriorityQueue OpenTile = new PriorityQueue();
        PriorityQueue ClosedTile = new PriorityQueue();

        OpenTile.EnQueue(new ANode(null,curPos,0, 0));

        int maxStep = unitMng.gameMng.mapMng.xSize * unitMng.gameMng.mapMng.ySize;
        int cnt = 0;
        ANode temp = null;
        Vector3Int prePos = Vector3Int.zero;
        ANode preNode = null;

        Vector3Int reTgt = new Vector3Int();

        if (!unitMng.gameMng.mapMng.GetTileOpen(tgtPos))
        {
            reTgt = unitMng.gameMng.mapMng.FindApproachTile(curPos ,tgtPos);
        }
        else
        {
            reTgt = tgtPos;
        }

        while (OpenTile.Count() > 0)
        {
            temp = OpenTile.DeQueue();
            ClosedTile.EnQueue(temp);

            if (temp.GetPos().Equals(reTgt))
            {
                break;
            }

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    prePos.Set(temp.GetPos().x + x, 0, temp.GetPos().z + y);
                    preNode = new ANode(temp, prePos,temp.GetCnt()*10, Heuristic(curPos, prePos,reTgt));
                    if (ClosedTile.Contains(prePos))
                    {
                        continue;
                    }
                    if (unitMng.gameMng.mapMng.isOutMapPos(prePos))
                    {
                        continue;
                    }
                    if (!unitMng.gameMng.mapMng.GetTileOpen(prePos))
                    {
                        continue;
                    }
                    OpenTile.EnQueue(preNode);
                }
            }
            cnt++;
            if (cnt > maxStep)
            {
                Debug.Log(transform.name + "너무 찾기 힘들어서 파업");
                Debug.Log(reTgt.x + "," + reTgt.y);
                break;
            }
        }
        if(OpenTile.Count() == 0)
        {
            return false;
        }
        if (wayPoint == null)
            wayPoint = new Stack<Vector3Int>();
        temp = ClosedTile.Find(reTgt);
        cnt=0;
        if(temp == null)
        {
            //목표
            return false;
        }
        while (temp != null)
        {
            if (cnt > maxStep)
            {
                Debug.Log(transform.name + "도착지에서 출발지 찾기 힘들어 파업");
                break;
            }
            
            wayPoint.Push(temp.GetPos());
            temp = temp.GetParent();
            
        }
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="orgPos">유닛의 위치</param>
    /// <param name="curPos">이동할 타일의 위치</param>
    /// <returns></returns>
    private float Heuristic(Vector3 orgPos, Vector3 curPos,Vector3 tgtPos)
    {
        //return Mathf.Abs(curPos.x - orgPos.x) + Mathf.Abs(curPos.y - orgPos.y) + Mathf.Abs(tgtPos.x - curPos.x) + Mathf.Abs(tgtPos.y - curPos.y);
        //위에 것은 성능 쓰레기 distance가 빠름
        return Vector3.Distance(curPos, tgtPos);
    }


    /// <summary>
    /// 유닛 선택시 효과를 내는 코루틴
    /// 유닛 선택 해제시 종료
    /// </summary>
    /// <returns></returns>
    IEnumerator SelectEffect()
    {
        while (true)
        {
            Quaternion rotation = Quaternion.identity;
            rotation.eulerAngles = new Vector3(0, 2, 0);
            SelectMesh.transform.rotation *= rotation;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
