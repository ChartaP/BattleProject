using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Lib;
using GameSys.Item;

namespace GameSys
{
    namespace Item
    {
        /// <summary>
        /// 아이템 정보 클래스
        /// </summary>
        public class ItemInfo
        {
            private int nID;
            private string sName;
            private string sIcon;
            private eItemType eType;
            private int nSize;

            public ItemInfo(int nID,string sName,string sIcon,eItemType eType,int nSize)
            {
                this.nID = nID;
                this.sName = sName;
                this.sIcon = sIcon;
                this.eType = eType;
                this.nSize = nSize;
            }

            public int ID
            {
                get { return nID; }
            }

            public string Name
            {
                get { return sName; }
            }

            public string Icon
            {
                get { return sIcon; }
            }

            public eItemType Type
            {
                get { return eType; }
            }

            public int Size
            {
                get { return nSize; }
            }

        }
        /// <summary>
        /// 블록 정보 클래스
        /// </summary>
        public class BlockInfo
        {
            private int nID;
            private string sName;
            private string sObject;
            private Color32 color;
            private int nStrength;

            public BlockInfo(int nID,string sName,string sObject,string Color,int nStrength)
            {
                this.nID = nID;
                this.sName = sName;
                this.sObject = sObject;
                switch (Color)
                {
                    case "Clear":
                        this.color = new Color32(0, 0, 0, 0);
                        break;
                    case "Brown":
                        this.color = new Color32(128, 64, 0, 255);
                        break;
                    case "Gray":
                        this.color = new Color32(128, 128, 128, 255);
                        break;
                    case "Blue":
                        this.color = new Color32(0, 0, 255, 255);
                        break;
                }
                this.nStrength = nStrength;
            }

            public int ID
            {
                get { return nID; }
            }

            public string Name
            {
                get { return sName; }
            }

            public string Object
            {
                get { return sObject; }
            }

            public Color32 Color
            {
                get { return color; }
            }
            
            public int Strength
            {
                get { return nStrength; }
            }
        }

        /// <summary>
        /// 게임에 존재하는 아이템들의 정보 제공 클래스
        /// </summary>
        public static class ItemMng
        {

        }

        /// <summary>
        /// 게임에 존재하는 블록들의 정보 제공 클래스
        /// </summary>
        public static class BlockMng
        {

        }

    }
}

