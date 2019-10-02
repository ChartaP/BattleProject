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

            public abstract void Start(UnitCtrl unit);

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
        /// 타겟 따라가기 명령
        /// </summary>
        public class MoveTarget : Order
        {
            public MoveTarget(Target target)
            {
                this.target = target;
                type = eOrder.MoveTarget;
            }

            public override void Start(UnitCtrl unit)
            {
            }

            public override void Works(UnitCtrl unit)
            {
                if (target != null)
                {
                    if (Vector2.Distance(unit.Pos, target.Pos) > (unit.Radius+target.Radius)*1.5f)
                    {
                        unit.MoveTarget(target);
                    }
                    else
                    {
                        unit.Stop();
                    }
                }
            }

            public override bool Achievement(UnitCtrl unit)
            {
                if(target == null)
                {
                    unit.Stop();
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
            public override void Start(UnitCtrl unit)
            {
                unit.MovePos(targetPos);
            }
            public override void Works(UnitCtrl unit)
            {
                
            }

            public override bool Achievement(UnitCtrl unit)
            {
                if (unit.X == targetPos.x && unit.Y == targetPos.y)
                {
                    unit.Stop();
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
            public override void Start(UnitCtrl unit)
            {
            }
            public override void Works(UnitCtrl unit)
            {
                if (target != null)
                {
                    if (Vector2.Distance(unit.Pos, target.Pos) < unit.AtkRange)
                    {
                        unit.AtkTarget(target);
                    }
                    else
                    {
                        unit.MoveTarget(target);
                    }
                }
            }
            public override bool Achievement(UnitCtrl unit)
            {
                if (target == null)
                {
                    unit.Stop();
                    return true;
                }
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
            public override void Start(UnitCtrl unit)
            {
                unit.MovePos(targetPos);
            }
            public override void Works(UnitCtrl unit)
            {
                if(target != null)
                {
                    if (Vector2.Distance(unit.Pos, target.Pos) < unit.AtkRange)
                    {
                        unit.AtkTarget(target);
                    }
                    else
                    {
                        unit.MoveTarget(target);
                    }
                }
                else
                {
                    target = unit.EnemyInView();
                }
            }
            public override bool Achievement(UnitCtrl unit)
            {
                if (unit.X == targetPos.x && unit.Y == targetPos.y)
                {
                    unit.Stop();
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
            public override void Start(UnitCtrl unit)
            {
            }
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
            public override void Start(UnitCtrl unit)
            {
            }
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
