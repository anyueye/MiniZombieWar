using System;
using UnityEngine;

[Serializable]
public abstract class AccessoryObjectData : EntityData
{
    [SerializeField]
    private int m_OwnerId = 0;


    public AccessoryObjectData(int entityId, int tableId, int ownerId)
        : base(entityId, tableId)
    {
        m_OwnerId = ownerId;
    }

    /// <summary>
    /// 拥有者编号。
    /// </summary>
    public int OwnerId
    {
        get
        {
            return m_OwnerId;
        }
    }
}