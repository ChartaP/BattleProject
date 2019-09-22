using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSys.Lib;

namespace GameSys
{
    namespace Order
    {
        public abstract class Order
        {
            protected eOrder type;
            protected Vector2 targetPos;

            public Vector2 TargetPos
            {
                get { return targetPos; }
            }

            public eOrder Type
            {
                get { return type; }
            }

            public abstract void Works(UnitCtrl unit);

            public abstract bool Achievement(UnitCtrl unit);
        }

        public class Move : Order
        {
            public Move(Vector2 targetPos)
            {
                this.targetPos = targetPos;
                type = eOrder.Move;
                
            }

            public override void Works(UnitCtrl unit)
            {
                unit.Move(targetPos);
            }

            public override bool Achievement(UnitCtrl unit)
            {
                if(unit.transform.localPosition.x == targetPos.x && unit.transform.localPosition.z == targetPos.y)
                {
                    unit.Stop();
                    return true;
                }
                return false;
                
            }
        }

        public class ATK : Order
        {
            public override void Works(UnitCtrl unit)
            {

            }
            public override bool Achievement(UnitCtrl unit)
            {
                return true;
            }
        }

        public class PTR : Order
        {
            public override void Works(UnitCtrl unit)
            {

            }
            public override bool Achievement(UnitCtrl unit)
            {
                return true;
            }
        }
    }
}
