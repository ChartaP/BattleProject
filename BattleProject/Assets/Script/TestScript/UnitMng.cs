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

    public void CreateUnit(Vector3 unitPos, PlayerCtrl Owner, eUnitType unitType = eUnitType.People)
    {
        UnitCtrl unitTemp;
        switch (unitType)
        {
            case eUnitType.Leader:
                unitTemp = Instantiate(gUnitList[0], this.transform).GetComponent<UnitCtrl>();
                unitTemp.SetUnit(this,unitType,Owner,unitPos);
                break;
            case eUnitType.People:
                unitTemp = Instantiate(gUnitList[1], this.transform).GetComponent<UnitCtrl>();
                unitTemp.SetUnit(this, unitType, Owner, unitPos);
                break;
        }
    }
    
    public int UnitjobToUnitID(eUnitJob job)
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
