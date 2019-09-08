using System.Collections;
using System.Collections.Generic;
using GameSys;

namespace GameSys
{
    namespace Lib {
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
    }
}