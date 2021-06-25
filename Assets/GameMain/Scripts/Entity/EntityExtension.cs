
using System;
using UnityGameFramework.Runtime;
using ZombieWar;

public static class EntityExtension
{
    private static int _serialId = 0;

    public static void HideEntity(this EntityComponent entityComponent, Entity entity)
    {
        if (!MyGameEntry.Entity.IsValidEntity(entity.Entity))
        {
            return;
        }
        entityComponent.HideEntity(entity.Entity);
    }

    public static void ShowPlayer(this EntityComponent entityComponent, SoldierData data)
    {
        entityComponent.ShowEntity(typeof(Soldier), "Player", 0, data);
    }
    
    public static void ShowWeapon(this EntityComponent entityComponent, WeaponData data)
    {
        entityComponent.ShowEntity(typeof(Weapon), "Weapon", 0, data);
    }
    
    public static void ShowEnemy(this EntityComponent entityComponent, ZombieData data)
    {
        entityComponent.ShowEntity(typeof(Zombie), "Enemy", 0, data);
    }

    public static void ShowBullet(this EntityComponent entityComponent, BulletData data)
    {
        entityComponent.ShowEntity(typeof(Bullet), "Bullet", 0, data);
    }

    private static void ShowEntity(this EntityComponent entityComponent, Type logicType, string entityGroup, int priority, EntityData data)
    {
        if (data==null)
        {
            return;
        }
        var drEntities=MyGameEntry.DataTable.GetDataTable<DREntity>();
        DREntity drE = drEntities.GetDataRow(data.TableId);
        if (drE==null)
        {
            return;
        }
        entityComponent.ShowEntity(data.EntityId, logicType, AssetUtility.GetEntityAsset(drE.AssetName), entityGroup, priority, data);
    }
    public static int GenerateSerialId(this EntityComponent entityComponent)
    {
        return --_serialId;
    }
    
}
