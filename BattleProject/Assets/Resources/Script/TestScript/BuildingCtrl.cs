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
    protected Dictionary<string, int> dicCost = new Dictionary<string, int>();
    protected float fComplete = 0;

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

    [SerializeField]
    protected eBuildingState buildingState = eBuildingState.Construction;
    [SerializeField]
    protected MeshRenderer model;
    [SerializeField]
    protected MeshRenderer constModel;

    protected int nEnteredCnt = 0;
    public int nEnterMax = 0;

    public virtual void SetBuilding(BuildingMng buildingMng, BuildingInfo buildingInfo, PlayerCtrl owner,Vector3 unitPos)
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
        StartCoroutine("Construction");
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Decide");
    }

    // Update is called once per frame
    void Update()
    {
    }

    protected virtual IEnumerator Decide()
    {
        yield break;
    }

    protected virtual IEnumerator Work()
    {
        yield break;
    }

    protected virtual IEnumerator Standby()
    {
        yield break;
    }

    protected override void RegisterStats()
    {
        docStats = new Dictionary<string, float>();
        docStats.Add("Size", 0.6f);
        docStats.Add("Radius", 0.6f);
        docStats.Add("Health", myBuildingInfo.Health);
        dicCost = myBuildingInfo.Cost;
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
            if (unit.Job != eUnitJob.Worker)
                continue;
            if (!enteredUnits.Contains(unit))
            {
                unit.receiptOrder(new MoveTarget(myTarget));
            }
        }
    }
    public virtual void RegisterUnit(UnitCtrl unit)
    {
        if (unit.Job != eUnitJob.Worker || regUnits.Contains(unit))
            return;

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
    
    public bool isEmpty
    {
        get
        {
            return enteredUnits.Count == 0;
        }
    }

    public void RegisterFindJoblessUnit()
    {
        foreach (UnitCtrl unit in Owner.UnitList)
        {
            if (unit.Job != eUnitJob.Worker)
                continue;
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
        if (enterUnit.Job != eUnitJob.Worker)
            return;
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

    protected virtual void Destruction()
    {
        GameMng.Instance.mapMng.bOpen[(int)X, (int)Y] = true;
        Owner.UnregisterPlayerBuilding(this);
        buildingMng.RemoveBuilding(this);
    }

    protected IEnumerator Construction()
    {
        yield return null;
        buildingState = eBuildingState.Construction;
        Target.MyBar.SetProgress(dicCost["time"]);
        model.enabled = false;
        while (true)
        {
            Target.MyBar.CurProgress((int)fComplete);
            if(fComplete >= dicCost["time"])
            {
                ChangeState(eBuildingState.Standby);
                model.enabled = true;
                constModel.enabled = false;
                Target.MyBar.InProgress();
                yield break;
            }
            yield return null;
        }

        yield break;
    }

    public bool WorkConstruction(UnitCtrl Worker,float fContribution)
    {
        fComplete += fContribution;
        RegisterUnit(Worker);
        if (buildingState == eBuildingState.Construction)
            return false;
        else
            return true;
    }

    public eBuildingState BuildingState
    {
        get
        {
            return buildingState;
        }
    }

    protected virtual void ChangeState(eBuildingState state)
    {
        if (buildingState != state)
        {
            switch (buildingState)
            {
                case eBuildingState.Construction:
                    break;
                case eBuildingState.Standby:
                    StopCoroutine("Standby");
                    break;
                case eBuildingState.Work:
                    StopCoroutine("Work");
                    break;
                case eBuildingState.Sleep:
                    break;

            }
            buildingState = state;
            switch (state)
            {
                case eBuildingState.Construction:
                    break;
                case eBuildingState.Standby:
                    StartCoroutine("Standby");
                    break;
                case eBuildingState.Work:
                    StartCoroutine("Work");
                    break;
                case eBuildingState.Sleep:
                    break;
            }
        }
    }

    public List<UnitCtrl> RegUnits
    {
        get
        {
            return regUnits;
        }
    }
}
