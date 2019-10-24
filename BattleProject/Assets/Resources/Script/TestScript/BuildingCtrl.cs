using System.Collections;
using System.Collections.Generic;
using GameSys.Lib;
using GameSys.Order;
using GameSys.Building;
using UnityEngine;

public class BuildingCtrl : ObjectCtrl
{
    private static int nBuildingCnt = 0;
    public BuildingMng buildingMng = null;
    private BuildingInfo myBuildingInfo = null;


    public void SetBuilding(BuildingMng buildingMng,byte id,PlayerCtrl owner,Vector3 unitPos)
    {
        this.buildingMng = buildingMng;
        this.owner = owner;
        transform.localPosition = unitPos;
        transform.name = (nBuildingCnt++) + "-" + Owner.name + "-building";
        OnGround();
        //Owner.RegisterPlayerBuilding(this);

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
        docStats.Add("Size", 1);
        docStats.Add("Radius", 1);
    }
    public override float Stat(string name)
    {
        if (docStats.ContainsKey(name))
            return docStats[name];
        else
            return 0.0f;
    }

}
