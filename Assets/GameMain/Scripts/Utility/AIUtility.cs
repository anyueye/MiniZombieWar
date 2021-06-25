
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
}
