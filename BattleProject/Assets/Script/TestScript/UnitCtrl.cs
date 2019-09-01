using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
       
            yield break;
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="curPos">현재 읽고 있는 타일 위치</param>
    /// <param name="CloseTile">현재 검색 중인 경로에서 갔던 길</param>
    /// <returns></returns>
    public bool SearchRoute(Vector3Int curPos)
    {
       
        return true;
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
