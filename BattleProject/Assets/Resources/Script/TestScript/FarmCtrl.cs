using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Lib;
using GameSys.Order;
using GameSys.Building;

public class FarmCtrl : BuildingCtrl
{
    [SerializeField]
    private float fSow = 0;
    [SerializeField]
    private float fWeeding = 0;
    [SerializeField]
    private float fYield = 0;

    protected void Start()
    {
        StartCoroutine("Decide");
    }

    protected void Update()
    {
        
    }

    protected override IEnumerator Work()
    {
        int cnt = 0;
        while (buildingState == eBuildingState.Work)
        {
            if(TimeMng.Instance.CurSeason == eSeason.SPRING)
            {
                cnt = 0;
                fSow += 20.0f * Time.deltaTime * TimeMng.Instance.DateSpeed / 24;
            }
            else if (TimeMng.Instance.CurSeason == eSeason.SUMMER)
            {
                fWeeding += 20.0f * Time.deltaTime * TimeMng.Instance.DateSpeed / 24;
            }
            else if (TimeMng.Instance.CurSeason == eSeason.FALL)
            {
                
                fYield += 20.0f * Time.deltaTime * TimeMng.Instance.DateSpeed / 24;

                if(fYield > 1.0f)
                {
                    if (Mathf.Min(fSow, fWeeding) > cnt)
                    {
                        Owner.GetResource("Food", (int)fYield);
                        fYield = 0.0f;
                        cnt++;
                    }
                }
            }
            else if (TimeMng.Instance.CurSeason == eSeason.WINTER)
            {
                fSow = 0.0f;
                fWeeding = 0.0f;
                fYield = 0.0f;
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
            switch (buildingState)
            {
                case eBuildingState.Standby:
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
                            ChangeState(eBuildingState.Work);
                        }
                    }
                    break;
                case eBuildingState.Work:
                    if (TimeMng.Instance.isNight)
                    {
                        if (enteredUnits.Count != 0)
                        {
                            ExitBuilding();
                        }
                        else
                        {
                            ChangeState(eBuildingState.Standby);
                        }
                    }
                    break;
            }
            yield return new WaitForSecondsRealtime(0.4f);
        }
    }

    public override void SetBuilding(BuildingMng buildingMng, BuildingInfo buildingInfo, PlayerCtrl owner, Vector3 unitPos)
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
