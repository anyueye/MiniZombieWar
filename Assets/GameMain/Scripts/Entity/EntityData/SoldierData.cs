
using UnityEngine;
using ZombieWar;

public class SoldierData:TargetableObjectData
{
    [SerializeField] private readonly WeaponData m_WeaponData;
    public SoldierData(int entityId, int tableId) : base(entityId, tableId)
    {
        var dtS= MyGameEntry.DataTable.GetDataTable<DRSoldier>();
        if (dtS==null)
        {
            return;
        }

        var drS = dtS.GetDataRow(tableId);
        MaxHp = Hp = drS.Hp;
        m_WeaponData = new WeaponData(MyGameEntry.Entity.GenerateSerialId(), 30000, entityId);
    }

    public WeaponData WeaponData => m_WeaponData;

    public override int MaxHp { get; }
}
