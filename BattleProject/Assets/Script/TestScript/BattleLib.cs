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
            People  = 0,//인간
            Animal  = 1//동물
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

        public class Pos
        {
            private static int cnt=0;
            private int id;
            private int nX;
            private int nY;
            public Pos(int nX,int nY)
            {
                this.nX = nX;
                this.nY = nY;
                id = cnt++;
            }
            public int X { get { return nX; } }
            public int Y { get { return nY; } }

            public static bool operator ==(Pos a,Pos b)
            {
                return (a.X == b.X && a.Y == b.Y) ;
            }

            public static bool operator !=(Pos a, Pos b)
            {
                return (a.X != b.X || a.Y != b.Y);
            }

            public override bool Equals(object a)
            {
                return ((Pos)a).X == X && ((Pos)a).Y == Y;
            }
            
            public override int GetHashCode()
            {
                return cnt;
            }
        }
    }
}