﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Lib;
using GameSys.Order;
using GameSys.Building;

public class StorageCtrl : BuildingCtrl
{
    protected void Start()
    {
        base.Start();
    }

    protected void Update()
    {
        base.Update();

    }

    protected override void Destruction()
    {
        GameMng.Instance.mapMng.bOpen[(int)X, (int)Y] = true;
        owner.RemoveStorage("Food",100);
        Owner.UnregisterPlayerBuilding(this);
        buildingMng.RemoveBuilding(this);
    }

    protected override void ChangeState(eBuildingState state)
    {
        if (buildingState != state)
        {
            switch (buildingState)
            {
                case eBuildingState.Construction:
                    owner.AddStorage("Food",  100);
                    break;
                case eBuildingState.Standby:
                    break;
                case eBuildingState.Work:
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
                    break;
                case eBuildingState.Work:

                    break;
                case eBuildingState.Sleep:
                    break;
            }
        }
    }
}
