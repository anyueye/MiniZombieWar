
using UnityEngine;
using ZombieWar;

public class WeaponData:AccessoryObjectData
{
    [SerializeField] private int m_BulletId;
    public WeaponData(int entityId, int tableId, int ownerId) : base(entityId, tableId, ownerId)
    {
        var dtWeapon = MyGameEntry.DataTable.GetDataTable<DRWeapon>();
        if (dtWeapon==null)
        {
            
            return;
        }
        
        var drWeapon = dtWeapon.GetDataRow(tableId);
        m_BulletId = drWeapon.BulletId;
    }

    public int BulletId => m_BulletId;

}
