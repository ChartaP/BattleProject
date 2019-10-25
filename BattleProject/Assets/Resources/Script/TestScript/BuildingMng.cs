using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Building;

public class BuildingMng : MonoBehaviour
{
    public GameMng gameMng;
    public List<BuildingCtrl> buildingList = new List<BuildingCtrl>();
    public Material[] RangeMater;

    public BuildingCtrl CreateBuilding(PlayerCtrl Owner, BuildingInfo buildingInfo, Vector3 buildingPos)
    {
        GameObject pre = Resources.Load("Prefab/BuildingPre") as GameObject;
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
        return temp;
    }

    public void RemoveBuilding(BuildingCtrl building)
    {
        buildingList.Remove(building);
        building.StopAllCoroutines();
        Destroy(building.gameObject);
    }
}
