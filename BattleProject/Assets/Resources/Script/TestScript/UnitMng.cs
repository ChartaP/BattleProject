using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Lib;

public class UnitMng : MonoBehaviour
{
    public GameMng gameMng;
    public List<UnitCtrl> unitList=new List<UnitCtrl>();
    public List<GameObject> gUnitList;
    public Material[] RangeMater;

    public UnitCtrl CreateUnit(Vector3 unitPos, PlayerCtrl Owner, eUnitType unitType = eUnitType.People)
    {
        UnitCtrl unitTemp = null;
        switch (unitType)
        {
            case eUnitType.People:
                unitTemp = Instantiate(gUnitList[0], this.transform).GetComponent<UnitCtrl>();
                unitTemp.SetUnit(this, unitType, Owner, unitPos);
                unitList.Add(unitTemp);

                foreach(MeshRenderer mesh in unitTemp.transform.GetComponentsInChildren<MeshRenderer>())
                {
                    if (mesh.transform.tag == "Unit")
                    {
                        mesh.material = Owner.playerMater;
                    }
                    if(mesh.transform.tag == "Interface")
                    {
                        if (Owner == gameMng.playerMng.CtrlPlayer)
                        {
                            mesh.material = RangeMater[0];
                        }
                        if (Owner != gameMng.playerMng.CtrlPlayer)
                        {
                            mesh.material = RangeMater[1];
                        }
                    }
                }
                break;
        }
        return unitTemp;
    }

    public void ChangeJob(UnitCtrl unit,eUnitJob job)
    {
        unit.SetJob(job);
    }

    public void RemoveUnit(UnitCtrl unit)
    {
        unitList.Remove(unit);
        unit.StopAllCoroutines();
        Destroy(unit.gameObject);
    }
    
    public int UnitjobToID(eUnitJob job)
    {
        switch (job)
        {
            case eUnitJob.Leader:
                return 0;
            case eUnitJob.Jobless:
                return 1;
            case eUnitJob.Farmer:
                return 2;
            case eUnitJob.Miner:
                return 3;
            case eUnitJob.Laborer:
                return 4;
            case eUnitJob.Stoneman:
                return 5;
            case eUnitJob.Spearman:
                return 6;
            case eUnitJob.Bowman:
                return 7;
            case eUnitJob.Swordman:
                return 8;
            case eUnitJob.Cavalry:
                return 9;
        }
        return -1;
    }
    
    public bool IsCollisionPos(UnitCtrl self,Vector2 pos)
    {
        foreach(UnitCtrl unit in unitList)
        {
            if (unit == self)
                continue;
            if(Vector2.Distance(unit.Pos,pos) < unit.Stat("Radius")/2+self.Stat("Radius")/2)
            {
                return true;
            }
        }
        return false;
    }

    public UnitCtrl UnitCollisionPos(UnitCtrl self, Vector2 pos)
    {
        foreach (UnitCtrl unit in unitList)
        {
            if (Vector2.Distance(unit.Pos, pos) < unit.Stat("Radius") / 2 + self.Stat("Radius") / 2)
            {
                return unit;
            }
        }
        return null;
    }
}
