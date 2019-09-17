using System.Collections;
using System.Collections.Generic;
using GameSys;

namespace GameSys
{
    namespace Lib {
        public enum eDifficulty
        {
            GodBless = 0,
            Lucky = 1,
            Commonness = 2,
            Adventure = 3,
            Apocalypse = 4,
        }

        public enum ePlayerType
        {
            Player = 0,
            Computer = 1,
            Observer = 2
        }

        public enum eGeoType
        {
            Flat = 0,
            Water = 1,
            Sea = 2,
            Hill = 3
        }

        public enum eItemType
        {
            Block = 0,
            Item = 1
        }

        public enum ePlants
        {
            Null=-1,
            Grass = 0,
            Tree = 1
        }

        /// <summary>
        /// 유닛 타입
        /// </summary>
        public enum eUnitType
        {
            Null    = -1,
            Leader  = 0,//리더 유닛
            People  = 1,
            Army    = 2,
            Animal  = 3
        }
        /// <summary>
        /// 유닛 직업
        /// </summary>
        public enum eUnitJob
        {
            Null        = -1,
            Leader      = 0,//지도자
            Jobless     = 100,//백수
            Farmer      = 101,//농부
            Miner       = 102,//광부
            Laborer     = 104,//인부
            Stoneman    = 200,//돌도끼병
            Spearman    = 201,//창병
            Bowman      = 202,//궁병
            Swordman    = 203,//검사
            Cavalry     = 204,//기병

        }
    }
}