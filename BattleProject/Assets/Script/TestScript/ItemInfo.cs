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
            private byte b_ID;
            private string sName;
            private string sIcon;
            private eItemType eType;
            private string sGameobject;
            private int nSize;

            public ItemInfo(byte b_ID, string sName,string sIcon,eItemType eType,string sGameobject, int nSize)
            {
                this.b_ID = b_ID;
                this.sName = sName;
                this.sIcon = sIcon;
                this.eType = eType;
                this.sGameobject = sGameobject;
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

            public string Gameobject
            {
                get { return sGameobject; }
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
        public class ItemInfoMng
        {
            //싱글톤 형식으로 제작
            private static ItemInfoMng instance = null;

            public static ItemInfoMng Instance
            {
                get
                {
                    if(instance == null)
                    {
                        instance = new ItemInfoMng();
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
        public class BlockInfoMng
        {
            //싱글톤 형식으로 제작
            private static BlockInfoMng instance = null;

            public static BlockInfoMng Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new BlockInfoMng();
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
        /// 아이템 인벤토리 칸에서 작동할 아이템 객체
        /// </summary>
        public class Item
        {
            private ItemInfo itemInfo;



            public ItemInfo Info
            {
                get { return itemInfo; }
            }
        }

        /// <summary>
        /// 타일의 소유물로서 작동할 블록 객체
        /// </summary>
        public class Block
        {
            private BlockInfo blockInfo;
            private GameObject model;

            public Block(BlockInfo blockInfo,GameObject model)
            {
                this.blockInfo = blockInfo;
                this.model = model;
            }

            public void MeshEnabled(bool val)
            {
                model.transform.Find("Cube").GetComponent<MeshRenderer>().enabled = val;
            }

            public BlockInfo Info
            {
                get { return blockInfo; }
            }
        }
    }
}

