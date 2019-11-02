using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Building;

public class BuildingMng : MonoBehaviour
{
    public GameMng gameMng;
    public List<BuildingCtrl> buildingList = new List<BuildingCtrl>();
    public Material[] RangeMater;

    public BuildingCtrl CreateBuilding(PlayerCtrl Owner ,UnitCtrl Worker, BuildingInfo buildingInfo, Vector3 buildingPos)
    {
        foreach(string Cost in buildingInfo.Cost.Keys)
        {
            if (Cost == "time")
                continue;
            if(Owner.dicResource[Cost] - buildingInfo.Cost[Cost] < 0)
            {
                gameMng.interfaceMng.AlertText(Cost+"가 부족합니다");
                return null;
            }
        }
        foreach (string Cost in buildingInfo.Cost.Keys)
        {
            if (Cost == "time")
                continue;
            Owner.dicResource[Cost] -= buildingInfo.Cost[Cost];
        }
        GameObject pre = Resources.Load("Prefab/"+buildingInfo.Component) as GameObject;
        BuildingCtrl temp = Instantiate(pre, this.transform).GetComponent<BuildingCtrl>();
        temp.SetBuilding(this, buildingInfo, Owner, buildingPos);
        buildingList.Add(temp);

        foreach (MeshRenderer mesh in temp.transform.GetComponentsInChildren<MeshRenderer>())
        {
            if (mesh.transform.tag == "Unit")
            {
                mesh.material = Owner.playerMater;
            }
            if (mesh.transform.tag == "Interface")
            {
                if (Owner == gameMng.playerMng.CtrlPlayer)
                {
                    mesh.material = RangeMater[0];
                }
                if (Owner != gameMng.playerMng.CtrlPlayer)
                {
                    mesh.material = RangeMater[1];
                }
            }
        }
        Worker.receiptOrder(new GameSys.Order.Build(temp.Target));
        return temp;
    }

    public void RemoveBuilding(BuildingCtrl building)
    {
        buildingList.Remove(building);
        building.StopAllCoroutines();
        Destroy(building.gameObject);
    }
}
