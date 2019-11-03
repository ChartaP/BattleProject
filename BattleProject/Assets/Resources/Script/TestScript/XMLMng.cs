using System.Collections;
using System.Collections.Generic;
using System.Xml;
using GameSys.Item;
using GameSys.Unit;
using GameSys.Building;
using GameSys.Lib;
using UnityEngine;

public class XMLMng : MonoBehaviour
{
    public string sItemXmlFileName = "ItemInfo";
    public string sUnitXmlFileName = "UnitInfo";
    public string sBuildingXmlFileName = "BuildingInfo";
    void Awake()
    {
        LoadItemXML(sItemXmlFileName);
        LoadUnitXML(sUnitXmlFileName);
        LoadBuildingXML(sBuildingXmlFileName);
    }
    
    private void LoadItemXML(string sFileName)
    {
        if (ItemInfoMng.Instance.set)
            return;
        //XML파일을 텍스트에셋으로 불러오기
        TextAsset textAsset = (TextAsset)Resources.Load("XML/" + sFileName);
        //불러온 텍스트에셋을 XmlDocument 형식으로 불러오는 작업
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);

        //아이템 노드 리스트 불러오기
        XmlNodeList ItemNodes = xmlDoc.SelectNodes("ItemInfo/Item/Field");
        foreach(XmlNode node in ItemNodes)
        {
            //노드의 내용물 불러와서 아이템 정보에 저장 후 아이템 매니져에 추가하는 작업
            byte id = byte.Parse(node.Attributes.GetNamedItem("id").Value);
            byte.TryParse( node.Attributes.GetNamedItem("id").InnerText, out id );
            string name = node.Attributes.GetNamedItem("name").InnerText;
            string icon = node.Attributes.GetNamedItem("icon").InnerText;
            string gameobject = node.Attributes.GetNamedItem("gameobject").InnerText;

            eItemType type = eItemType.Block;

            switch (node.Attributes.GetNamedItem("type").InnerText)
            {
                case "Block":
                    type = eItemType.Block;
                    break;
                case "Resource":
                    type = eItemType.Resource;
                    break;
                case "Tool":
                    type = eItemType.Tool;
                    break;
                case "Equip":
                    type = eItemType.Equip;
                    break;
            }

            int size=int.Parse(node.Attributes.GetNamedItem("size").InnerText);

            ItemInfo info = new ItemInfo(id,name,icon,type,gameobject,size);
            ItemInfoMng.Instance.AddItem(info);
        }

        //블록 노드 리스트 불러오기
        XmlNodeList BlockNodes = xmlDoc.SelectNodes("ItemInfo/Block/Field");
        foreach (XmlNode node in BlockNodes)
        {
            //노드의 내용물 불러와서 블록 정보에 저장 후 블록 매니져에 추가하는 작업
            byte id = byte.Parse(node.Attributes.GetNamedItem("id").InnerText);
            string name = node.Attributes.GetNamedItem("name").InnerText;
            string texture = node.Attributes.GetNamedItem("texture").InnerText;

            string color = node.Attributes.GetNamedItem("color").InnerText;

            int strength= int.Parse(node.Attributes.GetNamedItem("strength").InnerText);

            BlockInfo info = new BlockInfo(id, name, texture, color, strength);
            BlockInfoMng.Instance.AddBlock(info);
        }
    }

    private void LoadUnitXML(string sFileName)
    {
        if (JobInfoMng.Instance.set)
            return;
        //XML파일을 텍스트에셋으로 불러오기
        TextAsset textAsset = (TextAsset)Resources.Load("XML/" + sFileName);
        //불러온 텍스트에셋을 XmlDocument 형식으로 불러오는 작업
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);

        //인간 노드 리스트 불러오기
        XmlNodeList JobNodes = xmlDoc.SelectNodes("UnitInfo/Job/Field");
        foreach (XmlNode node in JobNodes)
        {
            //노드의 내용물 불러와서 아이템 정보에 저장 후 아이템 매니져에 추가하는 작업
            byte id; //= byte.Parse(node.Attributes.GetNamedItem("id").Value);
            byte.TryParse(node.Attributes.GetNamedItem("id").InnerText, out id);
            string name = node.Attributes.GetNamedItem("name").InnerText;
            string face = node.Attributes.GetNamedItem("face").InnerText;

            float Size = float.Parse(node.Attributes.GetNamedItem("size").InnerText);
            float ColRadius = float.Parse(node.Attributes.GetNamedItem("colradius").InnerText);
            float ColHeight = float.Parse(node.Attributes.GetNamedItem("colheight").InnerText);
            float MoveSpeed = float.Parse(node.Attributes.GetNamedItem("movespeed").InnerText);
            float RotateSpeed = float.Parse(node.Attributes.GetNamedItem("rotatespeed").InnerText);
            float AtkSpeed = float.Parse(node.Attributes.GetNamedItem("atkspeed").InnerText);
            float ViewRange = float.Parse(node.Attributes.GetNamedItem("viewrange").InnerText);
            float AtkRange = float.Parse(node.Attributes.GetNamedItem("atkrange").InnerText);

            int Health = int.Parse(node.Attributes.GetNamedItem("health").InnerText);
            int AtkPower = int.Parse(node.Attributes.GetNamedItem("atkpower").InnerText);
            int DefValue = int.Parse(node.Attributes.GetNamedItem("defvalue").InnerText);
            //Debug.Log(id+","+ name + "," + face + "," + Size + "," + ColRadius + "," + ColHeight + "," + MoveSpeed + "," + RotateSpeed + "," + Health + "," + AtkPower + "," + AtkSpeed + "," + DefValue + "," + ViewRange + "," + AtkRange);
            JobInfo info = new JobInfo(id,name,face,Size,ColRadius,ColHeight,MoveSpeed,RotateSpeed,Health,AtkPower,AtkSpeed,DefValue,ViewRange,AtkRange);
            JobInfoMng.Instance.AddJob(info);
        }

        //직업장비 노드 리스트 불러오기
        XmlNodeList JobEquipNodes = xmlDoc.SelectNodes("UnitInfo/JobEquip/Field");
        foreach (XmlNode node in JobEquipNodes)
        {
            //노드의 내용물 불러와서 블록 정보에 저장 후 블록 매니져에 추가하는 작업
            byte id = byte.Parse(node.Attributes.GetNamedItem("id").InnerText);

            string head = node.Attributes.GetNamedItem("head").InnerText;
            string rhand = node.Attributes.GetNamedItem("rhand").InnerText;
            string lhand = node.Attributes.GetNamedItem("lhand").InnerText;
            string chest = node.Attributes.GetNamedItem("chest").InnerText;
            string projectile = node.Attributes.GetNamedItem("projectile").InnerText;
            string atkreadyani = node.Attributes.GetNamedItem("atkreadyani").InnerText;
            string atkingani = node.Attributes.GetNamedItem("atkingani").InnerText;

            JobEquipInfo info = new JobEquipInfo(id, head,rhand,lhand,chest,projectile,atkreadyani,atkingani);
            JobEquipInfoMng.Instance.AddJobEquip(info);
        }
        
    }

    private void LoadBuildingXML(string sFileName)
    {
        if (BuildingInfoMng.Instance.set)
            return;
        //XML파일을 텍스트에셋으로 불러오기
        TextAsset textAsset = (TextAsset)Resources.Load("XML/" + sFileName);
        //불러온 텍스트에셋을 XmlDocument 형식으로 불러오는 작업
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);

        //건물 노드 리스트 불러오기
        XmlNodeList JobNodes = xmlDoc.SelectNodes("BuildingInfo/Building/Field");
        foreach (XmlNode node in JobNodes)
        {
            //노드의 내용물 불러와서 건물 정보에 저장 후 아이템 매니져에 추가하는 작업
            byte id = byte.Parse(node.SelectSingleNode("id").InnerText);
            string name = node.SelectSingleNode("name").InnerText;
            string component = node.SelectSingleNode("component").InnerText;
            int health = int.Parse( node.SelectSingleNode("health").InnerText);
            Dictionary<string, int> dicCost = new Dictionary<string, int>();
            foreach(XmlNode cost in node.SelectSingleNode("cost").ChildNodes)
            {
                //Debug.Log(cost.Name);
                dicCost.Add(cost.Name, int.Parse(cost.InnerText));
            }
            //Debug.Log("-" + name + "-" + component + "-"+health);
            BuildingInfo info = new BuildingInfo(id, name, component,health, dicCost);
            BuildingInfoMng.Instance.AddBuilding(info);
        }
        

    }

}
