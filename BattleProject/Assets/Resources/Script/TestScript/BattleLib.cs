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
            Player = 0,//인간 플레이어
            Computer = 1,//컴퓨터 플레이어
            barbarian = 2,//야만인
            Animal = 3,//동물
            Plants = 4,//식물
            Observer = 5//관전자
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
            Resource = 1,
            Tool = 2,
            Equip =3
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
            People  = 0,//인간
            Animal  = 1//동물
        }

        public enum eTargetType
        {
            Unit    =1,
            Building=2,
            Animal  =3,
            Plants  =4
        }
        /// <summary>
        /// 유닛 직업
        /// </summary>
        public enum eUnitJob
        {
            Null        = -1,
            Leader      = 0,//지도자
            Worker     = 100,//일꾼
            Stoneman    = 200,//돌도끼병
            Spearman    = 201,//창병
            Bowman      = 202,//궁병
            Swordman    = 203,//검사
            Cavalry     = 204,//기병
        }

        public enum eUnitWorkState
        {
            Jobless     =0,
            Working     =1
        }

        public enum eUnitState
        {
            Standby     =0,//대기중
            Move      =1,//이동중
            Atk       =2,//공격중
            Dead        =3,
            Work        =4
        }

        public enum eOrder
        {
            Null = -1,
            MoveTarget = 0,
            MovePos = 1,
            AtkTarget = 2,
            AtkPos  =3,
            PTR = 4,
            Hold = 5
        }

        public enum eSeason
        {
            SPRING = 0,
            SUMMER = 1,
            FALL   = 2,
            WINTER = 3
        }

        public enum eDayState
        {
            DAY = 0,
            NIGHT = 1
        }

        public enum eInputState
        {
            CONTROL_OBJECT = 0,
            CREATE_OBJECT = 1
        }
    }
}