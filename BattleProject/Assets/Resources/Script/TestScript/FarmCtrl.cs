using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Lib;
using GameSys.Order;
using GameSys.Building;

public class FarmCtrl : BuildingCtrl
{
    private float fSow = 0;
    private float fWeeding = 0;
    private float fYield = 0;

    protected void Start()
    {
        base.Start();
    }

    protected void Update()
    {
        base.Update();
        
    }

    protected override IEnumerator Work()
    {
        while (buildingState == eBuildingState.Work)
        {
            if(TimeMng.Instance.CurSeason == eSeason.SPRING)
            {
                fSow += Time.deltaTime * 1.0f;
            }
            else if (TimeMng.Instance.CurSeason == eSeason.SUMMER)
            {
                
            }
            else if (TimeMng.Instance.CurSeason == eSeason.FALL)
            {

            }
            yield return null;
        }
        yield break;
    }

    protected override IEnumerator Decide()
    {
        while (true)
        {
            while (buildingState == eBuildingState.Construction)
            {
                yield return new WaitForSecondsRealtime(0.4f);
            }
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

            yield return new WaitForSecondsRealtime(0.4f);
        }
    }

    public void SetBuilding(BuildingMng buildingMng, BuildingInfo buildingInfo, PlayerCtrl owner, Vector3 unitPos)
    {
        this.buildingMng = buildingMng;
        this.owner = owner;

        myBuildingInfo = buildingInfo;
        RegisterStats();
        curHealth = docStats["Health"];
        

        foreach (MeshRenderer mesh in transform.GetComponentsInChildren<MeshRenderer>())
        {
            if (mesh.tag == "building")
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

}
