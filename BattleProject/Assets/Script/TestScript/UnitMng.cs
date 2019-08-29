using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMng : MonoBehaviour
{
    public GameMng gameMng;
    public List<UnitCtrl> unitList=new List<UnitCtrl>();
    public List<UnitCtrl> selectableUnit;

    private void Awake()
    {
        selectableUnit = new List<UnitCtrl>();
    }
    // Start is called before the first frame update
    void Start()
    {
        int cnt=0;
        foreach(UnitCtrl unit in unitList)
        {
            unit.unitID = cnt++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FocusCurUnit(UnitCtrl unit)
    {
        if(unit == null)
        {
            gameMng.interfaceMng.ChangeFaceInterface(numFace.NullUnit);
        }
        else
        {
            if (!selectableUnit.Contains(unit))
            {
                selectableUnit.Add(unit);
                unit.ReceiveSelectOrder();
                gameMng.interfaceMng.ChangeFaceInterface(unit.unitNum);
            }
        }
    }

    /// <summary>
    /// 유닛 다중 선택 메서드, 
    /// 자료출처 : https://youtu.be/ceMyupol6AQ
    /// </summary>
    /// <param name="mPosStart">드래그 시작점</param>
    /// <param name="mPosEnd">드래그 도착점</param>
    public void SelectUnits(Vector3 mPosStart,Vector3 mPosEnd)
    {
        List<UnitCtrl> RemUnit = new List<UnitCtrl>();
        Rect selectRect = new Rect(mPosStart.x, mPosStart.y, mPosEnd.x - mPosStart.x, mPosEnd.y - mPosStart.y);

        foreach (UnitCtrl unit in unitList)
        {
            if (selectRect.Contains(Camera.main.WorldToViewportPoint(unit.transform.position), true))
            {
                FocusCurUnit(unit);
            }
            else
            {
                RemUnit.Add(unit);
            }
        }

        if(RemUnit.Count > 0)
        {
            foreach(UnitCtrl rem in RemUnit)
            {
                rem.UnableSelect();
                selectableUnit.Remove(rem);
            }
            RemUnit.Clear();
        }
    }

    public void ClearSelectableUnit()
    {
        foreach (UnitCtrl unit in selectableUnit)
        {
            unit.UnableSelect();
        }
        selectableUnit.Clear();
    }

    /// <summary>
    /// 현재 선택된 유닛에게 이동 명령을 내리는 메서드
    /// </summary>
    /// <param name="target">목표 지점</param>
    public void IssueMoveOrder(Vector3 target)
    {

        foreach(UnitCtrl unit in selectableUnit) {
            Vector3Int temp = new Vector3Int();
            temp.Set((int)target.x,0, (int)target.z);
            unit.ReceiveMoveOrder(temp);
        }
    }
}
