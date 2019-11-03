using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSys
{
    namespace Building
    {
        public class BuildingInfo
        {
            private byte b_ID;
            private string sName;
            private string sComponent;
            private int nHealth;
            private Dictionary<string, int> dicCost = new Dictionary<string, int>();
            public BuildingInfo(byte id,string name,string component,int health,Dictionary<string,int> cost)
            {
                b_ID = id;
                sName = name;
                sComponent = component;
                nHealth = health;
                dicCost = cost;
            }

            public byte ID
            {
                get
                {
                    return b_ID;
                }
            }
            public string Name
            {
                get
                {
                    return sName;
                }
            }
            public string Component
            {
                get
                {
                    return sComponent;
                }
            }

            public int Health
            {
                get
                {
                    return nHealth;
                }
            }

            public Dictionary<string,int> Cost
            {
                get
                {
                    return dicCost;
                }
            }

        }

        public class BuildingInfoMng
        {
            public bool set = false;
            private static BuildingInfoMng instance = null;

            public static BuildingInfoMng Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new BuildingInfoMng();
                    }
                    return instance;
                }
            }

            private Dictionary<byte, BuildingInfo> buildingDic = new Dictionary<byte, BuildingInfo>();

            public void AddBuilding(BuildingInfo info)
            {
                set = true;
                if (buildingDic.ContainsKey(info.ID)) return;

                buildingDic.Add(info.ID, info);
            }

            public BuildingInfo Building(byte ID)
            {
                return buildingDic[ID];
            }
        }
    }
}