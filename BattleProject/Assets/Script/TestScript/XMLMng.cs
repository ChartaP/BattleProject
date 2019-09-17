using System.Collections;
using System.Collections.Generic;
using System.Xml;
using GameSys.Item;
using GameSys.Lib;
using UnityEngine;

public class XMLMng : MonoBehaviour
{
    public string sXmlFileName = "ItemInfo";
    void Awake()
    {
        LoadXML(sXmlFileName);
    }
    
    private void LoadXML(string sFileName)
    {
        //XML파일을 텍스트에셋으로 불러오기
        TextAsset textAsset = (TextAsset)Resources.Load( sFileName);
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

            eItemType type = eItemType.Item;

            switch (node.Attributes.GetNamedItem("type").InnerText)
            {
                case "Block":
                    type = eItemType.Block;
                    break;
                case "Item":
                    type = eItemType.Item;
                    break;
            }

            int size=int.Parse(node.Attributes.GetNamedItem("size").InnerText);

            ItemInfo info = new ItemInfo(id,name,icon,type,size);
            ItemMng.Instance.AddItem(info);
        }

        //블록 노드 리스트 불러오기
        XmlNodeList BlockNodes = xmlDoc.SelectNodes("ItemInfo/Block/Field");
        foreach (XmlNode node in BlockNodes)
        {
            //노드의 내용물 불러와서 블록 정보에 저장 후 블록 매니져에 추가하는 작업
            byte id = byte.Parse(node.Attributes.GetNamedItem("id").InnerText);
            string name = node.Attributes.GetNamedItem("name").InnerText;
            string gameobject = node.Attributes.GetNamedItem("gameobject").InnerText;

            string color = node.Attributes.GetNamedItem("color").InnerText;

            int strength= int.Parse(node.Attributes.GetNamedItem("strength").InnerText);

            BlockInfo info = new BlockInfo(id, name, gameobject, color, strength);
            BlockMng.Instance.AddBlock(info);
        }



    }

}
