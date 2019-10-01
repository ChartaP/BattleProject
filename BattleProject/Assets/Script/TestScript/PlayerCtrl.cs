using System.Collections;
using System.Collections.Generic;
using GameSys.Lib;
using GameSys.Order;
using UnityEngine;

namespace GameSys
{
    namespace Lib
    {
        public class PlayerInfo
        {
            private static int PlayerCnt = 0;
            private int nID;
            private string sName;
            private ePlayerType eType;
            private eDifficulty eDifficulty;

            public PlayerInfo(string sName,ePlayerType eType,eDifficulty eDifficulty)
            {
                nID = PlayerCnt++;
                this.sName = sName;
                this.eType = eType;
                this.eDifficulty = eDifficulty;
            }

            public int ID
            {
                get { return nID; }
            }
            public string Name
            {
                get { return sName; }
            }
            public ePlayerType Type
            {
                get { return eType; }
            }
            public eDifficulty Difficulty
            {
                get { return eDifficulty; }
            }
        }
    }
}

public class PlayerCtrl : MonoBehaviour
{
    public int PlayerID;
    public PlayerMng playerMng;
    public PlayerInfo playerInfo;

    public List<UnitCtrl> UnitList;
    public List<BuildingCtrl> BuildingList;
    public List<UnitCtrl> selectableUnit = new List<UnitCtrl>();

    public UnitCtrl LeaderUnit;

    public int[,] FoW; //0 안개, 1 보이는곳, 2 한번 봤던 곳

    public int nPeople;
    public int nFood;
    public int nResource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
    public void UnselectUnits()
    {
        foreach (UnitCtrl unit in selectableUnit)
        {
            unit.SelectMesh.enabled = false;
        }
        selectableUnit.Clear();
    }

    /// <summary>
    /// 지정 범위의 유닛 다중 선택 메서드
    /// 자료출처 : https://youtu.be/ceMyupol6AQ
    /// </summary>
    /// <param name="mPosStart">드래그 시작점</param>
    /// <param name="mPosEnd">드래그 도착점</param>
    public void SelectObject(Vector3 mPosStart, Vector3 mPosEnd)
    {
        //찾은 유닛 리스트
        List<UnitCtrl> findUnits = new List<UnitCtrl>();
        //찾은 건물 리스트
        Rect selectRect = new Rect(mPosStart.x, mPosStart.y, mPosEnd.x - mPosStart.x, mPosEnd.y - mPosStart.y);
        foreach (UnitCtrl unit in UnitList)
        {
            if (selectRect.Contains(Camera.main.WorldToViewportPoint(unit.transform.position), true))
            {
                findUnits.Add(unit);
            }
        }

        if(findUnits.Count == 0 )//찾은 내 유닛 목록수0
        {
            //내 건물 목록 찾기


            if (true)//찾은 내 건물 목록수 0
            {
                foreach(PlayerCtrl player in playerMng.PlayerList)
                {
                    if (player == this)//만약 검색한 플레이어가 나일 경우 무시
                        continue;
                    
                    foreach (UnitCtrl unit in player.UnitList)
                    {
                        if (selectRect.Contains(Camera.main.WorldToViewportPoint(unit.transform.position), true))
                        {
                            findUnits.Add(unit);
                            break;//현재 플레이어가 아닌 플레이어의 유닛은 여럿 찾을 필요 없으므로 반복문 탈출
                        }
                    }
                    if(findUnits.Count > 0)//이 플레이어의 유닛 존재
                    {
                        break;
                    }
                    else//유닛이 존재하지 않음
                    {
                        //내 건물 목록 찾기
                    }
                }
            }
        }
        if (findUnits.Count > 0)
        {
            UnselectUnits();
            foreach (UnitCtrl unit in findUnits)
            {
                unit.SelectMesh.enabled = true;
                selectableUnit.Add(unit);
            }
        }
    }

    /// <summary>
    /// 넘겨받은 유닛리스트를 선택
    /// </summary>
    /// <param name="unitList">유닛리스트</param>
    public void SelectUnits(List<UnitCtrl> unitList)
    {
        if (unitList.Count == 0)
        {
        }
        else
        {
            foreach (UnitCtrl unit in unitList)
            {
                unit.SelectMesh.enabled = false;
                if (UnitList.Contains(unit))
                {
                    selectableUnit.Add(unit);
                    unit.SelectMesh.enabled = true;
                }
            }
        }
    }
    
    /// <summary>
    /// 유닛 선택 메서드
    /// </summary>
    /// <param name="unit"></param>
    public void SelectUnit(UnitCtrl unit)
    {
        if (unit != null)
        {
            if (UnitList.Contains(unit))
            {
                UnselectUnits();
                selectableUnit.Add(unit);
                unit.SelectMesh.enabled = true;
            }
        }
    }

    /// <summary>
    /// 플레이어 유닛 등록 메서드
    /// </summary>
    /// <param name="unit"></param>
    public void RegisterPlayerUnit(UnitCtrl unit)
    {
        UnitList.Add(unit);
    }
    /// <summary>
    /// 플레이어 유닛 해지 메서드
    /// </summary>
    /// <param name="unit"></param>
    public void UnregisterPlayerUnit(UnitCtrl unit)
    {
        UnitList.Remove(unit);
        selectableUnit.Remove(unit);
        if (unit.Job == eUnitJob.Leader)
            playerMng.fallPlayer(this);
    }
    
    /// <summary>
    /// 선택된 유닛에 명령 메서드
    /// </summary>
    /// <param name="target"></param>
    public void OrderUnits(eOrder type, Transform target)
    {
        if (selectableUnit.Count == 0)
            return;
        if (selectableUnit[0].Owner != this)
            return;

        Vector2 center = UnitsCenterPos();

        foreach (UnitCtrl unit in selectableUnit)
        {
            switch (type)
            {
                case eOrder.MoveTarget:
                    unit.receiptOrder(new MoveTarget(target.GetComponent<Target>()));
                    break;
                case eOrder.MovePos:
                    unit.receiptOrder(new MovePos( new Vector2(unit.X-UnitsCenterPos().x + target.localPosition.x, unit.Y- UnitsCenterPos().y+ target.localPosition.z )));
                    break;
                case eOrder.AtkTarget:
                    unit.receiptOrder(new AtkTarget(target.GetComponent<Target>()));
                    break;
                case eOrder.AtkPos:
                    unit.receiptOrder(new AtkPos(new Vector2(unit.X-UnitsCenterPos().x+ target.localPosition.x, unit.Y-UnitsCenterPos().y + target.localPosition.z)));
                    break;
            }
        }
    }

    public Vector2 UnitsCenterPos()
    {
        float X=0;
        float Y=0;
        foreach(UnitCtrl unit in selectableUnit)
        {
            X += unit.X;
            Y += unit.Y;
        }
        X /= selectableUnit.Count;
        Y /= selectableUnit.Count;

        return new Vector2(X, Y);
    }

    public ePlayerType Type
    {
        get { return playerInfo.Type; }
    }
}
