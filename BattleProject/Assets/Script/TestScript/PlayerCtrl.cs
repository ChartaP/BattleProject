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
    

    /// <summary>
    /// 지정 범위의 유닛 다중 선택 메서드
    /// 자료출처 : https://youtu.be/ceMyupol6AQ
    /// </summary>
    /// <param name="mPosStart">드래그 시작점</param>
    /// <param name="mPosEnd">드래그 도착점</param>
    public void SelectUnits(Vector3 mPosStart, Vector3 mPosEnd)
    {
        Rect selectRect = new Rect(mPosStart.x, mPosStart.y, mPosEnd.x - mPosStart.x, mPosEnd.y - mPosStart.y);

        foreach (UnitCtrl unit in UnitList)
        {
            if (selectRect.Contains(Camera.main.WorldToViewportPoint(unit.transform.position), true))
            {
                unit.SelectMesh.enabled = true;
                if (!selectableUnit.Contains(unit))
                {
                    selectableUnit.Add(unit);
                }
            }
        }
    }

    /// <summary>
    /// 넘겨받은 유닛리스트를 선택유닛리스트에 추가
    /// </summary>
    /// <param name="unitList">유닛리스트</param>
    public void SelectUnits(List<UnitCtrl> unitList)
    {
        if (unitList.Count == 0)
        {
            selectableUnit.Clear();
        }
        else
        {
            selectableUnit.Clear();
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
            selectableUnit.Add(unit);
            unit.SelectMesh.enabled = true;
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
    /// 플레이어 유닛 해제 메서드
    /// </summary>
    /// <param name="unit"></param>
    public void UnregisterPlayerUnit(UnitCtrl unit)
    {
        UnitList.Remove(unit);
    }

    public void OrderUnits(Vector3 target)
    {
        foreach(UnitCtrl unit in selectableUnit)
        {
            unit.receiptOrder(new Move(new Vector2(target.x,target.z)));
        }
    }

}
