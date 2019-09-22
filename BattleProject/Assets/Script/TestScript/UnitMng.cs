using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Lib;

public class UnitMng : MonoBehaviour
{
    public GameMng gameMng;
    public List<UnitCtrl> unitList=new List<UnitCtrl>();
    public List<GameObject> gUnitList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public UnitCtrl CreateUnit(Vector3 unitPos, PlayerCtrl Owner, eUnitType unitType = eUnitType.People)
    {
        UnitCtrl unitTemp = null;
        switch (unitType)
        {
            case eUnitType.People:
                unitTemp = Instantiate(gUnitList[0], this.transform).GetComponent<UnitCtrl>();
                unitTemp.SetUnit(this, unitType, Owner, unitPos);
                break;
        }
        return unitTemp;
    }

    public void ChangeJob(UnitCtrl unit,eUnitJob job)
    {
        unit.SetJob(job);
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
    
    
}
