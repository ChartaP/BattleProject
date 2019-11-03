using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Lib;

public class BootCampCtrl : BuildingCtrl
{
    [SerializeField]
    private List<eUnitJob> ProductSchedule = new List<eUnitJob>();
    private float fProgress = 0.0f;
    private float fDesired = 1.0f;

    private void Start()
    {
        StartCoroutine("Decide");
    }

    public void AddProductUnit(eUnitJob job)
    {
        if(Owner.UseResource("Food", 10))
            ProductSchedule.Add(job);

    }

    protected override IEnumerator Work()
    {
        while (buildingState == eBuildingState.Work)
        {
            if(ProductSchedule.Count != 0)
            {
                fProgress += Time.timeScale * 1.0f;
                if(fProgress >= fDesired)
                {
                    fProgress = 0.0f;
                    UnitCtrl unit = GameMng.Instance.unitMng.CreateUnit(VicinityPos(), Owner, eUnitType.People);
                    GameMng.Instance.unitMng.ChangeJob(unit, ProductSchedule[0]);
                    ProductSchedule.RemoveAt(0);
                }
            }
            else
            {
                ChangeState(eBuildingState.Standby);
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
                    if (ProductSchedule.Count != 0)
                        ChangeState(eBuildingState.Work);
                    break;
                case eBuildingState.Work:
                    break;
            }

            yield return new WaitForSecondsRealtime(0.4f);
        }
    }

    private Vector3 VicinityPos()
    {
        Vector3 temp = new Vector3(X+Random.Range(-1,1), Height, Y + Random.Range(-1, 1));

        return temp;
    }

}
