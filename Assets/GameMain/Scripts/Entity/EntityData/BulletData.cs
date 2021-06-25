
using UnityEngine;

public class BulletData:EntityData
{
    [SerializeField] private int m_OwnerId = 0;
    [SerializeField] private int m_Attack = 0;
    [SerializeField] private float maxLength = 0;
    public BulletData(int entityId, int tableId,int ownerId,int attack,float length) : base(entityId, tableId)
    {
        m_OwnerId = ownerId;
        m_Attack = attack;
        maxLength = length*1.5f;
    }
    public int OwnerId
    {
        get
        {
            return m_OwnerId;
        }
    }
    

    public int Attack
    {
        get
        {
            return m_Attack;
        }
    }

    public float MaxLength
    {
        get
        {
            return maxLength;
        }
    }
}
