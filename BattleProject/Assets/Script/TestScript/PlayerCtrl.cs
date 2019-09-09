using System.Collections;
using System.Collections.Generic;
using GameSys.Lib;
using UnityEngine;

namespace GameSys
{
    namespace Lib
    {
        public class PlayerInfo
        {
            public int nID;
            public string sName;
            public ePlayerType eType;
            public eDifficulty nDifficulty;

            public PlayerInfo(int nID,string sName,ePlayerType eType,eDifficulty nDifficulty)
            {
                this.nID = nID;
                this.sName = sName;
                this.eType = eType;
                this.nDifficulty = nDifficulty;
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

    public void SelectUnit(UnitCtrl unit)
    {

    }

    /// <summary>
    /// 유닛 다중 선택 메서드, 
    /// 자료출처 : https://youtu.be/ceMyupol6AQ
    /// </summary>
    /// <param name="mPosStart">드래그 시작점</param>
    /// <param name="mPosEnd">드래그 도착점</param>
    public void SelectUnits(Vector3 mPosStart, Vector3 mPosEnd)
    {
        List<UnitCtrl> RemUnit = new List<UnitCtrl>();
        Rect selectRect = new Rect(mPosStart.x, mPosStart.y, mPosEnd.x - mPosStart.x, mPosEnd.y - mPosStart.y);

        foreach (UnitCtrl unit in UnitList)
        {
            if (selectRect.Contains(Camera.main.WorldToViewportPoint(unit.transform.position), true))
            {
                if (!selectableUnit.Contains(unit))
                {
                    selectableUnit.Add(unit);
                }
            }
            else
            {
                RemUnit.Add(unit);
            }
        }

        if (RemUnit.Count > 0 && RemUnit.Count != UnitList.Count)
        {
            foreach (UnitCtrl rem in RemUnit)
            {
                rem.SelectMesh.enabled = true;
                selectableUnit.Remove(rem);
            }
            RemUnit.Clear();
        }
    }

    public void ClearSelectableUnit()
    {
        foreach (UnitCtrl unit in selectableUnit)
        {
            unit.SelectMesh.enabled = false;
        }
        selectableUnit.Clear();
    }



}
