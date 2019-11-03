using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSys.Lib;
using GameSys.Order;
using GameSys.Unit;

public class HouseCtrl : BuildingCtrl
{
    
    private void Start()
    {
        StartCoroutine("Decide");
    }
    protected override IEnumerator Decide()
    {
        while (true)
        {
            while (buildingState == eBuildingState.Construction)
            {
                yield return new WaitForSecondsRealtime(0.4f);
            }
            if (!isRegisterUnitFull)
            {
                RegisterFindHomelessUnit();
            }
            if (TimeMng.Instance.isDay)
            {
                if (!isEmpty)
                {
                    ExitBuilding();
                }
            }
            if (TimeMng.Instance.isNight)
            {
                if (enteredUnits.Count != regUnits.Count)
                {
                    CallRegisterUnit();
                }
                else
                {
                    foreach(UnitCtrl unit in regUnits)
                    {
                        if (unit.isHungry)
                        {
                            unit.Eat();
                        }
                    }
                }
            }

            yield return new WaitForSecondsRealtime(0.4f);
        }
    }

    public void RegisterFindHomelessUnit()
    {
        foreach (UnitCtrl unit in Owner.UnitList)
        {
            if (unit.Job != eUnitJob.Worker)
                continue;
            if (regUnits.Count >= nEnterMax)
            {
                break;
            }
            if (unit.isHomeless)
            {
                RegisterUnit(unit);
            }

        }
    }

    public override void RegisterUnit(UnitCtrl unit)
    {
        if (unit.Job != eUnitJob.Worker || regUnits.Contains(unit))
            return;

        if (regUnits.Count < nEnterMax)
        {
            regUnits.Add(unit);
            unit.GetHome(this);
        }
        else
        {
            UnregisterUnit(regUnits[0]);
            regUnits.Add(unit);
            unit.GetHome(this);
        }
    }

}
