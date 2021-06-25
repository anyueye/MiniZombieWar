
using UnityEngine;
using ZombieWar;

public class ZombieData : TargetableObjectData
{
    [SerializeField]
    private readonly int m_MaxHP = 0;

    [SerializeField] private readonly float m_PursueRange = 0;
    [SerializeField] private readonly float m_AttackRange = 0;
    [SerializeField] private readonly float m_AtkInterval = 0;
    [SerializeField] private readonly int m_Attack = 0;
    public ZombieData(int entityId, int tableId) : base(entityId, tableId)
    {
        var dtEnemy= MyGameEntry.DataTable.GetDataTable<DREnemy>();
        if (dtEnemy==null)
        {
            return;
        }

        var drE = dtEnemy.GetDataRow(tableId);
        
        m_PursueRange = drE.PursueRange;
        m_AttackRange = drE.AttackRange;
        m_AtkInterval = drE.AtkInterVal;
        m_Attack = drE.Attack;
        Hp = m_MaxHP = drE.Hp;;
    }

    public override int MaxHp => m_MaxHP;
    public float PursueRange => m_PursueRange;
    public float AttackRange => m_AttackRange;
    public float AtkInterval => m_AtkInterval;
    public int Attack => m_Attack;
}
