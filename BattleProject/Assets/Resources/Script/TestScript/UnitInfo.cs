using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSys
{
    namespace Unit
    {
        public class JobInfo
        {
            private byte b_ID;
            private string sName, sFace;
            private float fSize,fColRadius,fColHeight, fMoveSpeed,fRotateSpeed, fAtkSpeed, fViewRange, fAtkRange;
            private int nHealth,nAtkPower,nDefValue;
            public JobInfo(byte b_ID,string sName,string sFace,float fSize,float fColRadius,float fColHeight,float fMoveSpeed,float fRotateSpeed,
                int nHealth,int nAtkPower,float fAtkSpeed,int nDefValue,float fViewRange,float fAtkRange)
            {
                this.b_ID = b_ID;
                this.sName = sName;
                this.sFace = sFace;
                this.fSize = fSize;
                this.fColRadius = fColRadius;
                this.fColHeight = fColHeight;
                this.fMoveSpeed = fMoveSpeed;
                this.fRotateSpeed = fRotateSpeed;
                this.nHealth = nHealth;
                this.nAtkPower = nAtkPower;
                this.fAtkSpeed = fAtkSpeed;
                this.nDefValue = nDefValue;
                this.fViewRange = fViewRange;
                this.fAtkRange = fAtkRange;
            }

            public byte ID
            {
                get { return b_ID; }
            }
            public string Name {
                get {
                    return sName;
                }
            }
            public string Face
            {
                get
                {
                    return sFace;
                }
            }
            public float Size
            {
                get { return fSize; }
            }
            public float ColRadius
            {
                get { return fColRadius; }
            }
            public float ColHeight
            {
                get { return fColHeight; }
            }
            public float MoveSpeed
            {
                get
                {
                    return fMoveSpeed;
                }
            }
            public float RoateSpeed
            {
                get
                {
                    return fRotateSpeed;
                }
            }
            public int Health
            {
                get
                {
                    return nHealth;
                }
            }
            public int AtkPower
            {
                get
                {
                    return nAtkPower;
                }
            }
            public float AtkSpeed
            {
                get
                {
                    return fAtkSpeed;
                }
            }
            public int DefValue
            {
                get
                {
                    return nDefValue;
                }
            }
            public float ViewRange
            {
                get
                {
                    return fViewRange;
                }
            }
            public float AtkRange
            {
                get
                {
                    return fAtkRange;
                }
            }
        }

        public class JobEquipInfo
        {
            private byte b_ID;
            private string sHead,sRHand,sLHand,sChest, sProjectile , sAtkReadyAni , sAtkingAni;
            public JobEquipInfo(byte b_ID,string sHead,string sRHand,string sLHand,string sChest,string sProjectile, string sAtkReadyAni, string sAtkingAni)
            {
                this.b_ID = b_ID;
                this.sHead = sHead;
                this.sRHand = sRHand;
                this.sLHand = sLHand;
                this.sChest = sChest;
                this.sProjectile = sProjectile;
                this.sAtkReadyAni = sAtkReadyAni;
                this.sAtkingAni = sAtkingAni;
            }
            public byte ID
            {
                get { return b_ID; }
            }
            public string Head
            {
                get { return sHead; }
            }
            public string RHand
            {
                get { return sRHand; }
            }
            public string LHand
            {
                get { return sLHand; }
            }
            public string Chest
            {
                get { return sChest; }
            }
            public string Projectile
            {
                get { return sProjectile; }
            }
            public string AtkReadyAni
            {
                get { return sAtkReadyAni; }
            }
            public string AtkingAni
            {
                get { return sAtkingAni; }
            }
        }

        public class JobInfoMng
        {
            public bool set = false;
            private static JobInfoMng instance = null;

            public static JobInfoMng Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new JobInfoMng();
                    }
                    return instance;
                }
            }

            private Dictionary<byte, JobInfo> jobDic = new Dictionary<byte, JobInfo>();

            public void AddJob(JobInfo info)
            {
                set = true;
                if (jobDic.ContainsKey(info.ID)) return;

                jobDic.Add(info.ID, info);
            }

            public JobInfo Job(byte ID)
            {
                return jobDic[ID];
            }

            
            
        }

        public class JobEquipInfoMng
        {
            private static JobEquipInfoMng instance = null;

            public static JobEquipInfoMng Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new JobEquipInfoMng();
                    }
                    return instance;
                }
            }

            private Dictionary<byte, JobEquipInfo> jobEquipDic = new Dictionary<byte, JobEquipInfo>();

            public void AddJobEquip(JobEquipInfo info)
            {
                if (jobEquipDic.ContainsKey(info.ID)) return;

                jobEquipDic.Add(info.ID, info);
            }

            public JobEquipInfo JobEquip(byte ID)
            {
                return jobEquipDic[ID];
            }
        }
    }
}
