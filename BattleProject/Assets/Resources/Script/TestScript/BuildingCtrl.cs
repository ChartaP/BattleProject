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

    [SerializeField]
    private Transform transModel = null;

    public void SetBuilding(BuildingMng buildingMng, BuildingInfo buildingInfo, PlayerCtrl owner,Vector3 unitPos)
    {
        this.buildingMng = buildingMng;
        this.owner = owner;
        myBuildingInfo = buildingInfo;
        RegisterStats();
        curHealth = docStats["Health"];
        GameObject pre = Instantiate(Resources.Load("Prefab/" + myBuildingInfo.Name) as GameObject, transModel);
        foreach (MeshRenderer mesh in pre.transform.GetComponentsInChildren<MeshRenderer>())
        {
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
        docStats.Add("Size", 1);
        docStats.Add("Radius", 1);
        docStats.Add("Health", myBuildingInfo.Health);
    }
    public override float Stat(string name)
    {
        if (docStats.ContainsKey(name))
            return docStats[name];
        else
            return 0.0f;
    }

}
