﻿using System.Collections;
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
            private byte b_ID;
            private string sName;
            private string sIcon;
            private eItemType eType;
            private int nSize;

            public ItemInfo(byte b_ID, string sName,string sIcon,eItemType eType,int nSize)
            {
                this.b_ID = b_ID;
                this.sName = sName;
                this.sIcon = sIcon;
                this.eType = eType;
                this.nSize = nSize;
            }

            public byte ID
            {
                get { return b_ID; }
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
            private byte b_ID;
            private string sName;
            private string sObject;
            private Color32 color;
            private int nStrength;

            public BlockInfo(byte b_ID, string sName,string sObject,string Color,int nStrength)
            {
                this.b_ID = b_ID;
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

            public byte ID
            {
                get { return b_ID; }
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
        /// 참고 https://phiru.tistory.com/328
        /// </summary>
        public class ItemMng
        {
            //싱글톤 형식으로 제작
            private static ItemMng instance = null;

            public static ItemMng Instance
            {
                get
                {
                    if(instance == null)
                    {
                        instance = new ItemMng();
                    }
                    return instance;
                }
            }
            //싱글톤 만들기 끝

            private Dictionary<int, ItemInfo> itemDic = new Dictionary<int, ItemInfo>();

            public void AddItem(ItemInfo info)
            {
                if (itemDic.ContainsKey(info.ID)) return;

                itemDic.Add(info.ID, info);
            }

            /// <summary>
            /// 해당 ID의 아이템 정보 반환
            /// </summary>
            /// <param name="ID">아이템 ID</param>
            /// <returns>아이템 정보</returns>
            public ItemInfo Item(int ID)
            {
                return itemDic[ID];
            }

            public string ItemName(int ID)
            {
                return itemDic[ID].Name;
            }
            
        }

        /// <summary>
        /// 게임에 존재하는 블록들의 정보 제공 클래스
        /// </summary>
        public class BlockMng
        {
            //싱글톤 형식으로 제작
            private static BlockMng instance = null;

            public static BlockMng Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new BlockMng();
                    }
                    return instance;
                }
            }
            //싱글톤 만들기 끝
            private Dictionary<int, BlockInfo> blockDic = new Dictionary<int, BlockInfo>();

            public void AddBlock(BlockInfo info)
            {
                if (blockDic.ContainsKey(info.ID)) return;

                blockDic.Add(info.ID, info);
            }

            /// <summary>
            /// 해당 ID의 블록 정보 반환
            /// </summary>
            /// <param name="ID">블록 ID</param>
            /// <returns>블록 정보</returns>
            public BlockInfo Block(int ID)
            {
                return blockDic[ID];
            }

            public string BlockName(int ID)
            {
                return blockDic[ID].Name;
            }
            
        }

        /// <summary>
        /// 아이템 클래스
        /// </summary>
        public class Item
        {

        }
        
        public class Block
        {
            public BlockInfo blockInfo;
            private GameObject model;

            public Block(BlockInfo blockInfo, GameObject model)
            {
                this.blockInfo = blockInfo;
                this.model = model;
            }

            public void MeshEnabled(bool val)
            {
                model.transform.Find("Cube").GetComponent<MeshRenderer>().enabled = val;
            }
        }
    }
}

