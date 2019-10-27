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
            protected Target target;

            public Vector2 TargetPos
            {
                get { return targetPos; }
            }

            public Target Target
            {
                get { return target; }
            }

            public eOrder Type
            {
                get { return type; }
            }

            public abstract void Start(ObjectCtrl unit);

            /// <summary>
            /// 명령 수행 메서드
            /// </summary>
            /// <param name="unit"></param>
            public abstract void Works(ObjectCtrl unit);

            /// <summary>
            /// 명령 달성 확인 메서드
            /// </summary>
            /// <param name="unit"></param>
            /// <returns></returns>
            public abstract bool Achievement(ObjectCtrl unit);
        }

        /// <summary>
        /// 타겟 따라가기 명령
        /// </summary>
        public class MoveTarget : Order
        {
            public MoveTarget(Target target)
            {
                this.target = target;
                type = eOrder.MoveTarget;
            }

            public override void Start(ObjectCtrl unit)
            {
                Debug.Log("StartMoveTarget"+Target.name+Target.Pos);
            }

            public override void Works(ObjectCtrl unit)
            {
                UnitCtrl unitCtrl = unit as UnitCtrl;
                if (target != null)
                {
                    //Debug.Log(Vector2.Distance(unitCtrl.Pos, target.Pos)+"/"+ (unitCtrl.Stat("Radius") + target.Radius + 0.1f));
                    if (Vector2.Distance(unitCtrl.Pos, target.Pos) > unitCtrl.Stat("Radius") +target.Radius +0.1f)
                    {
                        unitCtrl.MoveTarget(target);
                    }
                    else
                    {
                        Debug.Log("wow");
                        if(target.Type == eTargetType.Building)
                        {
                            target.transform.GetComponent<BuildingCtrl>().EnterBuilding(unitCtrl);
                            target = null;
                        }
                        unitCtrl.Stop();
                    }
                }
            }

            public override bool Achievement(ObjectCtrl unit)
            {
                UnitCtrl unitCtrl = unit as UnitCtrl;
                if (target == null)
                {
                    Debug.Log("No");
                    unitCtrl.Stop();
                    return true;
                }
                return false;
                
            }
        }

        /// <summary>
        /// 목표 지점 이동 명령
        /// </summary>
        public class MovePos : Order
        {
            public MovePos(Vector2 targetPos)
            {
                this.targetPos = targetPos;
                type = eOrder.MovePos;
            }
            public override void Start(ObjectCtrl unit)
            {
                UnitCtrl unitCtrl = unit as UnitCtrl;
                unitCtrl.MovePos(targetPos);
            }
            public override void Works(ObjectCtrl unit)
            {
                
            }

            public override bool Achievement(ObjectCtrl unit)
            {
                UnitCtrl unitCtrl = unit as UnitCtrl;
                if (unitCtrl.X == targetPos.x && unitCtrl.Y == targetPos.y)
                {
                    unitCtrl.Stop();
                    return true;
                }
                return false;

            }
        }

        /// <summary>
        /// 타겟 공격 명령
        /// </summary>
        public class AtkTarget : Order
        {
            public AtkTarget(Target target)
            {
                this.target = target;
                type = eOrder.MoveTarget;
            }
            public override void Start(ObjectCtrl unit)
            {
            }
            public override void Works(ObjectCtrl unit)
            {
                UnitCtrl unitCtrl = unit as UnitCtrl;
                if (target != null)
                {
                    if (Vector2.Distance(unitCtrl.Pos, target.Pos) < unitCtrl.Stat("AtkRange"))
                    {
                        unitCtrl.AtkTarget(target);
                    }
                    else
                    {
                        unitCtrl.MoveTarget(target);
                    }
                }
            }
            public override bool Achievement(ObjectCtrl unit)
            {
                UnitCtrl unitCtrl = unit as UnitCtrl;
                if (target == null)
                {
                    unitCtrl.Stop();
                    return true;
                }
                target = unitCtrl.EnemyInView();
                return false;
            }
        }

        public class AtkPos : Order
        {
            public AtkPos( Vector2 targetPos)
            {
                this.targetPos = targetPos;
                type = eOrder.AtkPos;
            }
            public override void Start(ObjectCtrl unit)
            {
                UnitCtrl unitCtrl = unit as UnitCtrl;
                unitCtrl.MovePos(targetPos);
            }
            public override void Works(ObjectCtrl unit)
            {
                UnitCtrl unitCtrl = unit as UnitCtrl;
                if (target != null)
                {
                    if (Vector2.Distance(unitCtrl.Pos, target.Pos) < unitCtrl.Stat("AtkRange"))
                    {
                        unitCtrl.AtkTarget(target);
                    }
                    else
                    {
                        unitCtrl.MoveTarget(target);
                    }
                }
                else
                {
                    target = unitCtrl.EnemyInView();
                }
            }
            public override bool Achievement(ObjectCtrl unit)
            {
                UnitCtrl unitCtrl = unit as UnitCtrl;
                if (unitCtrl.X == targetPos.x && unitCtrl.Y == targetPos.y)
                {
                    unitCtrl.Stop();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 좌표 A,B를 왕복하면서 적 발견시 공격 명령
        /// </summary>
        public class PTR : Order
        {
            public override void Start(ObjectCtrl unit)
            {
            }
            public override void Works(ObjectCtrl unit)
            {

            }
            public override bool Achievement(ObjectCtrl unit)
            {
                return true;
            }
        }

        /// <summary>
        /// 좌표 A에 고정되어 적이 사정거리 안에 들어올시 공격 명령
        /// </summary>
        public class Hold : Order
        {
            public override void Start(ObjectCtrl unit)
            {
            }
            public override void Works(ObjectCtrl unit)
            {

            }
            public override bool Achievement(ObjectCtrl unit)
            {
                return true;
            }
        }

        
    }
}
