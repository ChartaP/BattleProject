using System.Collections;
using System.Collections.Generic;
using GameSys.Lib;
using GameSys.Order;
using GameSys.Building;
using UnityEngine;

public class BuildingCtrl : ObjectCtrl
{
    protected static int nBuildingCnt = 0;
    public BuildingMng buildingMng = null;
    protected BuildingInfo myBuildingInfo = null;

    /// <summary>
    /// 건물에 들어온 유닛 목록
    /// </summary>
    [SerializeField]
    protected List<UnitCtrl> enteredUnits = new List<UnitCtrl>();
    /// <summary>
    /// 건물에 등록된 유닛 목록
    /// </summary>
    [SerializeField]
    protected List<UnitCtrl> regUnits = new List<UnitCtrl>();

    protected int nEnteredCnt = 0;
    public int nEnterMax = 0;

    public void SetBuilding(BuildingMng buildingMng, BuildingInfo buildingInfo, PlayerCtrl owner,Vector3 unitPos)
    {
        this.buildingMng = buildingMng;
        this.owner = owner;
        myBuildingInfo = buildingInfo;
        RegisterStats();
        curHealth = docStats["Health"];
        foreach (MeshRenderer mesh in transform.GetComponentsInChildren<MeshRenderer>())
        {
            if(mesh.tag == "building")
                mesh.material = Owner.playerMater;
        }
        transform.localPosition = unitPos;
        GameMng.Instance.mapMng.bOpen[(int)X, (int)Y] = false;
        sName = myBuildingInfo.Name;
        sIcon = myBuildingInfo.Name + "Icon";
        transform.name = (nBuildingCnt++) + "-" + Owner.name + "-building";
        OnGround();
        Owner.RegisterPlayerBuilding(this);

    }
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void RangeUpdate()
    {

    }

    protected override void RegisterStats()
    {
        docStats = new Dictionary<string, float>();
        docStats.Add("Size", 0.6f);
        docStats.Add("Radius", 0.6f);
        docStats.Add("Health", myBuildingInfo.Health);
    }
    public override float Stat(string name)
    {
        if (docStats.ContainsKey(name))
            return docStats[name];
        else
            return 0.0f;
    }

    protected void CallRegisterUnit()
    {
        foreach(UnitCtrl unit in regUnits)
        {
            if (!enteredUnits.Contains(unit))
            {
                unit.receiptOrder(new MoveTarget(myTarget));
            }
        }
    }
    public void RegisterUnit(UnitCtrl unit)
    {
        if (regUnits.Count < nEnterMax)
        {
            regUnits.Add(unit);
            unit.GetWork(this);
        }
        else
        {
            UnregisterUnit(regUnits[0]);
            regUnits.Add(unit);
            unit.GetWork(this);
        }
    }

    public void UnregisterUnit(UnitCtrl unit)
    {
        ExitBuilding(unit);
        regUnits.Remove(unit);
        unit.Fired();
    }
    
    public bool isEnterUnitFull
    {
        get
        {
            return enteredUnits.Count == nEnterMax;
        }
    }

    public bool isRegisterUnitFull
    {
        get
        {
            return regUnits.Count == nEnterMax;
        }
    }
    

    public void RegisterFindJoblessUnit()
    {
        foreach (UnitCtrl unit in Owner.UnitList)
        {
            if (regUnits.Count >= nEnterMax)
            {
                break;
            }
            if (unit.isJobless)
            {
                RegisterUnit(unit);
            }
            
        }
    }

    public void EnterBuilding(UnitCtrl enterUnit)
    {
        if (nEnteredCnt < nEnterMax)
        {
            if (!enteredUnits.Contains(enterUnit))
            {
                RegisterUnit(enterUnit);
            }
            nEnteredCnt++;
            enteredUnits.Add( enterUnit);
            enterUnit.StateChange(eUnitState.Work);
        }
    }

    public void ExitBuilding(UnitCtrl enterUnit)
    {
        if (enteredUnits.Contains(enterUnit))
        {
            nEnteredCnt--;
            enteredUnits.Remove(enterUnit);
            enterUnit.StateChange(eUnitState.Standby);
        }
    }

    public void ExitBuilding()
    {
        foreach(UnitCtrl unit in enteredUnits)
        {
            unit.StateChange(eUnitState.Standby);
        }
        nEnteredCnt=0;
        enteredUnits.Clear();
        
    }

    public override void GetDamage(float damage)
    {
        if (damage > 0)
        {
            DamageParicle.Play();
        }
        curHealth = (curHealth - damage <= 0) ? 0 : (curHealth - damage);
        if (curHealth == 0)
        {
            Destruction();
        }
    }

    protected void Destruction()
    {
        GameMng.Instance.mapMng.bOpen[(int)X, (int)Y] = true;
        Owner.UnregisterPlayerBuilding(this);
        buildingMng.RemoveBuilding(this);
    }
}
