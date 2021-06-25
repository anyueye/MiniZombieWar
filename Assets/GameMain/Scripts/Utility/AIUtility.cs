
using System;
using UnityEngine;

public class AIUtility
{
    public static float GetNearset(string EntityGroup,TargetableObject entity,out TargetableObject target)
    {
        var pg = MyGameEntry.Entity.GetEntityGroup(EntityGroup).GetAllEntities();
        var distance = Single.MaxValue;
        TargetableObject nearestTaget = null;
        foreach (var e in pg)
        {
            var t = (UnityGameFramework.Runtime.Entity) e;
            var  dist = Vector2.Distance(t.Logic.CachedTransform.position, entity.CachedTransform.position);
            if (!(dist < distance)) continue;
            nearestTaget = t.Logic as TargetableObject;
            distance = dist;
        }
        target = nearestTaget;
        return distance;
    }

    public static void PerformCollision(TargetableObject entity, Entity other)
    {
        if (entity == null || other == null)
        {
            return;
        }
        TargetableObject target = other as TargetableObject;
        if (target != null)
        {
            ImpactData entityImpactData = entity.GetImpactData();
            ImpactData targetImpactData = target.GetImpactData();
            
            int entityDamageHP = CalcDamageHP(targetImpactData.Attack, entityImpactData.Defense);
            int targetDamageHP = CalcDamageHP(entityImpactData.Attack, targetImpactData.Defense);

            entity.ApplyDamage(target,entityDamageHP);
            target.ApplyDamage(entity, targetDamageHP);
        }
    }

    private static int CalcDamageHP(int attack, int defense)
    {
        if (attack<=0)
        {
            return 0;
        }

        if (defense<0)
        {
            defense = 0;
        }

        var result = attack - defense;
        if (result<0)
        {
            result = 0;
        }

        return result;
    }
}
