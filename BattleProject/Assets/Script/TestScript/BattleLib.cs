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

        public enum eTileType
        {
            Water = 0,
            Ground = 1,
            Stone = 2,
            GroundOrStone = 3
        }

        public enum eResource
        {
            Stone
        }

        public enum ePlants
        {
            Null=-1,
            Grass = 0,
            Tree = 1
        }

        public enum eUnits
        {
            Null = -1,
            People = 0,

        }
    }
}