//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2021-06-24 10:52:06.487
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ZombieWar
{
    /// <summary>
    /// 敌人。
    /// </summary>
    public class DREnemy : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取怪。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取。
        /// </summary>
        public int Hp
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取追赶范围。
        /// </summary>
        public float PursueRange
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取攻击范围。
        /// </summary>
        public float AttackRange
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取攻击间隔。
        /// </summary>
        public float AtkInterVal
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取。
        /// </summary>
        public int Attack
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取死亡特效编号。
        /// </summary>
        public int DeadEffectId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取死亡声音编号。
        /// </summary>
        public int DeadSoundId
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            Hp = int.Parse(columnStrings[index++]);
            PursueRange = float.Parse(columnStrings[index++]);
            AttackRange = float.Parse(columnStrings[index++]);
            AtkInterVal = float.Parse(columnStrings[index++]);
            Attack = int.Parse(columnStrings[index++]);
            DeadEffectId = int.Parse(columnStrings[index++]);
            DeadSoundId = int.Parse(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    Hp = binaryReader.Read7BitEncodedInt32();
                    PursueRange = binaryReader.ReadSingle();
                    AttackRange = binaryReader.ReadSingle();
                    AtkInterVal = binaryReader.ReadSingle();
                    Attack = binaryReader.Read7BitEncodedInt32();
                    DeadEffectId = binaryReader.Read7BitEncodedInt32();
                    DeadSoundId = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
