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

            /// <summary>
            /// 명령 수행 메서드
            /// </summary>
            /// <param name="unit"></param>
            public abstract void Works(UnitCtrl unit);

            /// <summary>
            /// 명령 달성 확인 메서드
            /// </summary>
            /// <param name="unit"></param>
            /// <returns></returns>
            public abstract bool Achievement(UnitCtrl unit);
        }

        /// <summary>
        /// 무조건 이동 명령
        /// </summary>
        public class Move : Order
        {
            private Transform targetTrans = null;
            public Move(Vector2 targetPos)
            {
                this.targetPos = targetPos;
                type = eOrder.Move;
            }

            public Move(Transform target)
            {
                this.targetTrans = target;
                type = eOrder.Move;
            }

            public override void Works(UnitCtrl unit)
            {
                if (targetTrans != null)
                {
                    targetPos = new Vector2(targetTrans.localPosition.x, targetTrans.localPosition.z);
                    if (Vector2.Distance(unit.Pos, targetPos) > unit.Size*1.5)
                    {
                        unit.Move(targetPos);
                    }
                    else
                    {
                        unit.StateChange(eUnitState.Standby);
                    }
                }
                else
                {
                    unit.Move(targetPos);
                }
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

        /// <summary>
        /// 적 공격 명령
        /// </summary>
        public class ATK : Order
        {
            private Transform targetTrans = null;
            public ATK(Vector2 targetPos)
            {
                this.targetPos = targetPos;
                type = eOrder.ATK;
            }
            public ATK(Transform target)
            {
                targetTrans = target;
                type = eOrder.ATK;
            }
            public override void Works(UnitCtrl unit)
            {
                if(targetTrans != null)
                {
                    targetPos = new Vector2(targetTrans.localPosition.x, targetTrans.localPosition.z);
                    float distance = Vector2.Distance(unit.Pos, targetPos);
                    if (distance < unit.AtkRange)
                    {//공격범위 안
                        unit.AtkTarget(targetTrans.GetComponent<Target>());
                    }
                    else
                    {//공격범위 밖
                        //Debug.Log(targetPos);
                        unit.Move(targetPos);
                    }
                }
                else
                {
                    unit.AtkInView();
                }
            }
            public override bool Achievement(UnitCtrl unit)
            {
                if (targetTrans != null)
                {
                    targetPos = new Vector2(targetTrans.localPosition.x, targetTrans.localPosition.z);
                    float distance = Vector2.Distance(unit.Pos, targetPos);
                    if(distance > unit.ViewRange)
                    {
                        return true;
                    }
                }
                else
                {
                    if (unit.transform.localPosition.x == targetPos.x && unit.transform.localPosition.z == targetPos.y)
                    {
                        unit.Stop();
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 좌표 A,B를 왕복하면서 적 발견시 공격 명령
        /// </summary>
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

        /// <summary>
        /// 좌표 A에 고정되어 적이 사정거리 안에 들어올시 공격 명령
        /// </summary>
        public class Hold : Order
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
