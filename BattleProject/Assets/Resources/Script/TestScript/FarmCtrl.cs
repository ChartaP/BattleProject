using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Lib;

public class FarmCtrl : BuildingCtrl
{
    private float fSow = 0;
    private float fWeeding = 0;
    private float fYield = 0;

    protected void Start()
    {
        base.Start();
        RegisterFindJoblessUnit();
    }

    protected void Update()
    {
        base.Update();
        if (TimeMng.Instance.isDay && TimeMng.Instance.CurSeason != eSeason.WINTER)
        {
            if (!isEnterUnitFull)
            {
                if (!isRegisterUnitFull)
                {
                    RegisterFindJoblessUnit();
                }
                else
                {
                    Debug.Log("ComeCome");
                    CallRegisterUnit();
                }
            }
            else
            {

            }
        }
        if (TimeMng.Instance.isNight)
        {
            if (enteredUnits.Count != 0)
            {
                ExitBuilding();
            }
        }
    }
    
}
